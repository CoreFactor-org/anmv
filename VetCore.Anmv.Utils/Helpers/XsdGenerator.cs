using System.Runtime.Serialization;
using System.Text;
using System.Xml.Schema;

namespace VetCore.Anmv.Utils.Helpers;

/// <summary>
/// Helper for generating xsd file
/// </summary>
internal static class XsdGenerator
{
    public static string GenerateXsdFromType(Type rootType)
    {
        var str = new StringBuilder();
        var exporter = new XsdDataContractExporter();
        if (exporter.CanExport(rootType))
        {
            exporter.Export(rootType);
            var mySchemas = exporter.Schemas;
            var xmlNameValue = exporter.GetRootElementName(rootType)!;
            var employeeNameSpace = xmlNameValue.Namespace;

            foreach (XmlSchema schema in mySchemas.Schemas(employeeNameSpace))
            {
                using (var writer = new StringWriter(str))
                {
                    schema.Write(writer);
                }
            }
        }

        return str.ToString();
    }
}