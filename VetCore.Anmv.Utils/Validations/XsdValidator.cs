using System.Xml;
using System.Xml.Schema;

namespace VetCore.Anmv.Utils.Validations;

internal sealed class XsdValidator
{
    public static XsdValidationResult ValidateXmlWithXsd(FileInfo xmlFilePath, FileInfo xsdFilePath)
    {
        // Load XSD schema
        var schema = new XmlSchemaSet();
        schema.Add("", xsdFilePath.FullName);

        // Configure validation settings
        var settings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            Schemas = schema,
        };

        var errors = new List<string>();
        var warnings = new List<string>();

        // retrieve Validation Error using local delegate
        ValidationEventHandler validationEventHandler = (sender, e) =>
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                warnings.Add($"Warning : {e.Message}");
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                errors.Add($"Error : {e.Message}");
            }
        };

        settings.ValidationEventHandler += validationEventHandler;

        // Read and validate XML
        using (var reader = XmlReader.Create(xmlFilePath.FullName, settings))
        {
            try
            {
                while (reader.Read())
                {
                    // Read the entire file to validate it
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la validation XML : {ex.Message}");
            }

            settings.ValidationEventHandler -= validationEventHandler;
        }

        return new XsdValidationResult(errors, warnings);
    }
}

public sealed class XsdValidationResult(List<string> errors, List<string> warnings)
{
    public IReadOnlyList<string> Warnings => errors;
    public IReadOnlyList<string> Errors => errors;

    public string PrintErrorsAndWarnings(string separator)
    {
        return $"""
               ==== WARNINGS ====
               {string.Join(separator, warnings)}
               ==== ERRORS ====
               {string.Join(separator, errors)}
               """;
    }
}