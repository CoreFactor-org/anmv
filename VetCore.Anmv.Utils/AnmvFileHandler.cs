using VetCore.Anmv.Utils.Helpers;
using VetCore.Anmv.Utils.Validations;
using VetCore.Anmv.Utils.Xsd;
using VetCore.Anmv.Xml.Data;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Utils;

/// <summary>
/// Class that allow deserialization, validation and manipulation of AMNV file
/// </summary>
public static class AnmvFileHandler
{
    /// <summary>
    /// Deserialize the description file provided and returns Dto structure
    /// </summary>
    public static DonneesReferenceGroupDto? DeserializeDescriptionFile(FileInfo file)
    {
        return XmlSerializerHelper.DeserializeFromXml<DonneesReferenceGroupDto>(file);
    }

    /// <summary>
    /// Deserialize the description xml string provided and returns Dto structure
    /// </summary>
    public static DonneesReferenceGroupDto? DeserializeDescriptionXmlString(string xmlString)
    {
        return XmlSerializerHelper.DeserializeFromXml<DonneesReferenceGroupDto>(xmlString);
    }

    /// <summary>
    /// Deserialize the data file provided and returns Dto structure
    /// </summary>
    public static MedicinalProductGroupDto? DeserializeDataFile(FileInfo file)
    {
        return XmlSerializerHelper.DeserializeFromXml<MedicinalProductGroupDto>(file);
    }

    /// <summary>
    /// Deserialize the data xml string provided and returns Dto structure
    /// </summary>
    public static MedicinalProductGroupDto? DeserializeDataXmlString(string xmlString)
    {
        return XmlSerializerHelper.DeserializeFromXml<MedicinalProductGroupDto>(xmlString);
    }

    /// <summary>
    /// Serialize a reference DTO to a xml string
    /// </summary>
    public static string SerializeAmnvToXml(this DonneesReferenceGroupDto dto, bool indent = false)
    {
        return XmlSerializerHelper.SerializeToXml(dto, indent);
    }

    /// <summary>
    /// Serialize a data DTO to a xml string
    /// </summary>
    public static string SerializeAmnvToXml(this MedicinalProductGroupDto dto, bool indent = false)
    {
        return XmlSerializerHelper.SerializeToXml(dto, indent);
    }

    /// <summary>
    /// Validate the Description DTO and out errors
    /// </summary>
    public static bool Validate(this DonneesReferenceGroupDto dto, out ValidationErrors errors)
    {
        return dto.ValidateDto(out errors);
    }

    /// <summary>
    /// Validate a xml file with a xsd file
    /// </summary>
    public static XsdValidationResult ValidateXmlWithXsd(FileInfo xmlFilePath, FileInfo xsdFilePath)
    {
        return ValidateXmlWithXsd(xmlFilePath, File.ReadAllText(xsdFilePath.FullName));
    }

    /// <summary>
    /// Validate a xml file with from a xsd string content
    /// </summary>
    public static XsdValidationResult ValidateXmlWithXsd(FileInfo xmlFilePath, string xsdFileContent)
    {
        return XsdValidator.ValidateXmlWithXsd(xmlFilePath, xsdFileContent);
    }

    /// <summary>
    /// Validate a xml string content with from a xsd string content
    /// </summary>
    public static XsdValidationResult ValidateXmlWithXsd(string xmlFileContent, string xsdFileContent)
    {
        return XsdValidator.ValidateXmlWithXsd(xmlFileContent, xsdFileContent);
    }

    /// <summary>
    /// Validate a xml file with from a xsd string content
    /// </summary>
    public static XsdValidationResult ValidateXml(FileInfo xmlFilePath, AmnvFilesKey kind)
    {
        return XsdValidator.ValidateXmlWithXsd(xmlFilePath, kind.GetXsdContent());
    }

    /// <summary>
    /// Validate a xml file content with from a xsd string content
    /// </summary>
    public static XsdValidationResult ValidateXml(string xmlFileContent, AmnvFilesKey kind)
    {
        return XsdValidator.ValidateXmlWithXsd(xmlFileContent, kind.GetXsdContent());
    }
}