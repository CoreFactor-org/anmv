using System.Xml;
using System.Xml.Schema;

namespace VetCore.Anmv.Utils.Validations;

internal static class XsdValidator
{
    /// <summary>
    /// Validates an XML file against an XSD provided as a string.
    /// </summary>
    /// <param name="xmlFile">The XML file to validate.</param>
    /// <param name="xsdContent">The XSD content as a string.</param>
    /// <returns>The validation result containing errors and warnings.</returns>
    public static XsdValidationResult ValidateXmlWithXsd(FileInfo xmlFile, string xsdContent)
    {
        var settings = CreateXmlReaderSettings(xsdContent);
        using var reader = XmlReader.Create(xmlFile.FullName, settings);
        return ValidateXml(reader, settings);
    }

    /// <summary>
    /// Validates an XML content (string) against an XSD provided as a string.
    /// </summary>
    /// <param name="xmlContent">The XML content to validate.</param>
    /// <param name="xsdContent">The XSD content as a string.</param>
    /// <returns>The validation result containing errors and warnings.</returns>
    public static XsdValidationResult ValidateXmlWithXsd(string xmlContent, string xsdContent)
    {
        var settings = CreateXmlReaderSettings(xsdContent);
        using var reader = XmlReader.Create(new StringReader(xmlContent), settings);
        return ValidateXml(reader, settings);
    }

    /// <summary>
    /// Creates and configures the XmlReaderSettings using the XSD content provided as a string.
    /// </summary>
    /// <param name="xsdContent">The XSD content as a string.</param>
    /// <returns>The configured XmlReaderSettings.</returns>
    private static XmlReaderSettings CreateXmlReaderSettings(string xsdContent)
    {
        // Load the XSD schema from the provided string.
        var schemaSet = new XmlSchemaSet();
        using (var xsdReader = new StringReader(xsdContent))
        using (var xmlSchemaReader = XmlReader.Create(xsdReader))
        {
            schemaSet.Add(null, xmlSchemaReader);
        }

        var settings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            Schemas = schemaSet,
        };

        return settings;
    }

    /// <summary>
    /// Reads the XML using the provided XmlReader and validates it against the configured settings.
    /// </summary>
    /// <param name="reader">The XmlReader for the XML to validate.</param>
    /// <param name="settings">The XmlReaderSettings that contain the XSD schema and validation configuration.</param>
    /// <returns>The result of the XSD validation.</returns>
    private static XsdValidationResult ValidateXml(XmlReader reader, XmlReaderSettings settings)
    {
        var errors = new List<string>();
        var warnings = new List<string>();

        // Local event handler to capture validation errors and warnings.
        ValidationEventHandler validationEventHandler = (sender, e) =>
        {
            var nodeInfo = (sender is XmlReader xmlReader) ? xmlReader.Name : "NO_NODE_INFO";
            if (e.Exception != null)
            {
                var line = $"Line {e.Exception.LineNumber}, Position {e.Exception.LinePosition}";
                if (e.Severity == XmlSeverityType.Warning)
                {
                    warnings.Add($"Warning [{line}] ({nodeInfo}) {e.Message}");
                }
                else if (e.Severity == XmlSeverityType.Error)
                {
                    errors.Add($"Error [{line}] ({nodeInfo}) {e.Message}");
                }
            }
            else
            {
                warnings.Add($"General warning: {e.Message}");
            }
        };

        // Attach the event handler.
        settings.ValidationEventHandler += validationEventHandler;

        try
        {
            while (reader.Read())
            {
                // Simply read through the XML to trigger validation.
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during XML validation: {ex.Message}");
        }
        finally
        {
            // Detach the event handler.
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