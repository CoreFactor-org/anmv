using VetCore.Anmv.Utils.Helpers;
using VetCore.Anmv.Utils.Validations;
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
    /// Serialize a reference DTO to a xml string
    /// </summary>
    public static string SerializeAmnvToXml(this DonneesReferenceGroupDto dto, bool indent = false)
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
}