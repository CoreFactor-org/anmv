using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace VetCore.Anmv.Utils.Helpers;

/// <summary>
/// Serialization and deserialization helper
/// </summary>
internal static class XmlSerializerHelper
{
    /// <summary>
    /// Synchronously deserialize an XML string into a T type object
    /// </summary>
    /// <typeparam name="T">the type of object to return</typeparam>
    /// <param name="xmlString">the string to deserialize (the content of a xml file)</param>
    /// <returns>The deserialized object</returns>
    public static T? DeserializeFromXml<T>(string xmlString) where T : new()
    {
        if (string.IsNullOrEmpty(xmlString)) return default;

        var deserializer = new XmlSerializer(typeof(T));
        using (var reader = new StringReader(xmlString))
        {
            return (T?)deserializer.Deserialize(reader);
        }
    }
    /// <summary>
    /// Synchronously deserialize an XML string into a T type object
    /// </summary>
    /// <typeparam name="T">the type of object to return</typeparam>
    /// <param name="xmlFile">the xml file to deserialize</param>
    /// <returns>The deserialized object</returns>
    public static T? DeserializeFromXml<T>(FileInfo xmlFile) where T : new()
    {
        var deserializer = new XmlSerializer(typeof(T));
        using (var reader = xmlFile.OpenText())
        {
            return (T?)deserializer.Deserialize(reader);
        }
    }

    /// <summary>
    /// Synchronously serialize an object into xml
    /// </summary>
    /// <typeparam name="T">the type of object to serialize</typeparam>
    /// <param name="data">the object to serialize</param>
    /// <returns>the string representation of the serialized object</returns>
    public static string SerializeToXml<T>(T data, bool indent = false) where T : new()
    {
        var serializerNamespace = new XmlSerializerNamespaces();
        serializerNamespace.Add(string.Empty, string.Empty);

        var serializer = new XmlSerializer(typeof(T));
        var str = new StringBuilder();

        using (var writer = new StringWriter(str))
        using (var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { Indent = indent, OmitXmlDeclaration = true }))
        {
            serializer.Serialize(xmlWriter, data, serializerNamespace);
        }

        return str.ToString();
    }
}