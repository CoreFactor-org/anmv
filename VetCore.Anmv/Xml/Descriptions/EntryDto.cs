using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using VetCore.Anmv.Json.Description;

namespace VetCore.Anmv.Xml.Descriptions
{
    public class EntryDto
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "La valeur doit être >= 0")] // minInclusive=0
        [XmlElement("source-code")]
        public int SourceCode { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "La longueur doit être comprise entre 1 et 255")] // minLength=1, maxLength=255
        [XmlElement("source-desc")]
        public string SourceDesc { get; set; }
    }

    public static class XmlDtoExtensions
    {
        /// <summary>
        /// Converts an XML EntryDto object to a JSON EntryDtoJson object.
        /// </summary>
        /// <param name="xmlDto">The XML EntryDto object.</param>
        /// <returns>A converted EntryDtoJson object.</returns>
        public static EntryDtoJson ToJsonDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new EntryDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }
    }
}