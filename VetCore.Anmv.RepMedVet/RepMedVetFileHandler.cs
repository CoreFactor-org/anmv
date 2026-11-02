using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using VetCore.Anmv.RepMedVet.Json;
using VetCore.Anmv.RepMedVet.Xml;

namespace VetCore.Anmv.RepMedVet
{
    /// <summary>
    /// Lecture, écriture et validation du fichier REPMEDVET.xml. Ce fichier n'a pas de schéma public : la lecture est donc stricte
    /// et refuse tout élément inconnu des DTO, plutôt que de l'ignorer et de perdre une évolution du format sans le voir.
    /// </summary>
    public static class RepMedVetFileHandler
    {
        private const int MAX_REPORTED_ISSUES = 20;

        private static readonly XmlSerializer SERIALIZER = new XmlSerializer(typeof(MedecineRepositoryDto));

        /// <summary>
        /// Deserialize the file provided and returns Dto structure
        /// </summary>
        /// <exception cref="RepMedVetFormatException">The document is not a REPMEDVET file known by the DTO</exception>
        public static MedecineRepositoryDto DeserializeFile(FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            using (var stream = file.OpenRead())
            {
                return DeserializeStream(stream);
            }
        }

        /// <summary>
        /// Deserialize the stream provided and returns Dto structure. The stream stays open.
        /// </summary>
        /// <exception cref="RepMedVetFormatException">The document is not a REPMEDVET file known by the DTO</exception>
        public static MedecineRepositoryDto DeserializeStream(Stream xmlStream)
        {
            if (xmlStream == null)
                throw new ArgumentNullException(nameof(xmlStream));

            using (var reader = XmlReader.Create(xmlStream, CreateReaderSettings()))
            {
                return Deserialize(reader);
            }
        }

        /// <summary>
        /// Deserialize the xml string provided and returns Dto structure
        /// </summary>
        /// <exception cref="RepMedVetFormatException">The document is not a REPMEDVET file known by the DTO</exception>
        public static MedecineRepositoryDto DeserializeXmlString(string xmlString)
        {
            if (xmlString == null)
                throw new ArgumentNullException(nameof(xmlString));

            using (var text = new StringReader(xmlString))
            using (var reader = XmlReader.Create(text, CreateReaderSettings()))
            {
                return Deserialize(reader);
            }
        }

        /// <summary>
        /// Serialize a DTO to a xml string, in the element order of the source file
        /// </summary>
        public static string SerializeToXml(this MedecineRepositoryDto dto, bool indent = false)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var content = new StringBuilder();
            using (var text = new StringWriter(content))
            using (var writer = XmlWriter.Create(text, new XmlWriterSettings { Indent = indent, OmitXmlDeclaration = true }))
            {
                SERIALIZER.Serialize(writer, dto, namespaces);
            }

            return content.ToString();
        }

        /// <summary>
        /// Validate the DTO and its children against their attributes. Without public schema, this is the only check of the required data.
        /// </summary>
        public static bool Validate(this MedecineRepositoryDtoJson dto, out IReadOnlyList<string> errors)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var found = new List<string>();
            ValidateNode(dto, "$", found);
            errors = found;
            return found.Count == 0;
        }

        // Le fichier vient du web : aucune DTD ni ressource externe n'est résolue.
        private static XmlReaderSettings CreateReaderSettings()
        {
            return new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null };
        }

        private static MedecineRepositoryDto Deserialize(XmlReader reader)
        {
            var issues = new List<string>();
            var events = new XmlDeserializationEvents
            {
                OnUnknownElement = (_, args) => issues.Add($"Unknown element '{args.Element.Name}' at line {args.LineNumber}, position {args.LinePosition}"),
                OnUnknownAttribute = (_, args) => issues.Add($"Unknown attribute '{args.Attr.Name}' at line {args.LineNumber}, position {args.LinePosition}"),
                OnUnknownNode = (_, args) =>
                {
                    // Les éléments et attributs inconnus ont déjà leur propre événement.
                    if (args.NodeType == XmlNodeType.Text || args.NodeType == XmlNodeType.CDATA)
                    {
                        issues.Add($"Unexpected text at line {args.LineNumber}, position {args.LinePosition}");
                    }
                },
            };

            MedecineRepositoryDto dto;
            try
            {
                dto = (MedecineRepositoryDto)SERIALIZER.Deserialize(reader, events);
            }
            catch (InvalidOperationException exception)
            {
                throw new RepMedVetFormatException(
                    $"The document cannot be read as a REPMEDVET file: {exception.Message} {exception.InnerException?.Message}".TrimEnd(),
                    exception);
            }

            if (issues.Count != 0)
            {
                throw new RepMedVetFormatException(
                    $"The document does not match the known REPMEDVET format ({issues.Count} issues): {string.Join("; ", issues.Take(MAX_REPORTED_ISSUES))}");
            }

            return dto;
        }

        private static void ValidateNode(object node, string path, ICollection<string> errors)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(node, new ValidationContext(node), results, validateAllProperties: true);
            foreach (var result in results)
            {
                errors.Add($"{path}: {result.ErrorMessage}");
            }

            foreach (var property in node.GetType().GetProperties())
            {
                var value = property.GetValue(node);
                if (value is Array items)
                {
                    for (var index = 0; index < items.Length; index++)
                    {
                        var item = items.GetValue(index);
                        if (IsDto(item))
                        {
                            ValidateNode(item, $"{path}.{property.Name}[{index}]", errors);
                        }
                    }
                }
                else if (IsDto(value))
                {
                    ValidateNode(value, $"{path}.{property.Name}", errors);
                }
            }
        }

        private static bool IsDto(object value)
        {
            return value != null && value.GetType().Assembly == typeof(RepMedVetFileHandler).Assembly;
        }
    }
}
