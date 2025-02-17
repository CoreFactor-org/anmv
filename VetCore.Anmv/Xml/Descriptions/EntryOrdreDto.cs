using System.Xml.Serialization;
using VetCore.Anmv.Json.Description;

namespace VetCore.Anmv.Xml.Descriptions
{
    public sealed class EntryOrdreDto : EntryDto
    {
        [XmlElement("ordre")]
        public int Ordre { get; set; }
    }

    public static class EntryOrdreDtoExtensions
    {
        /// <summary>
        /// Converts an XML EntryOrdreDto object to a JSON EntryOrdreDtoJson object.
        /// </summary>
        /// <param name="xmlDto">The XML EntryOrdreDto object.</param>
        /// <returns>A converted EntryOrdreDtoJson object.</returns>
        public static EntryOrdreDtoJson ToJsonDto(this EntryOrdreDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new EntryOrdreDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
                Ordre = xmlDto.Ordre,
            };
        }
    }
}