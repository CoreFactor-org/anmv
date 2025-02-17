using System.Xml.Serialization;
using VetCore.Anmv.Json.Description;

namespace VetCore.Anmv.Xml.Descriptions
{
    public class EntryDto
    {
        [XmlElement("source-code")]
        public int SourceCode { get; set; }

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