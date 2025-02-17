using System.Text.Json.Serialization;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Json.Description
{
    public sealed class EntryOrdreDtoJson : EntryDtoJson
    {
        [JsonPropertyName("ordre")]
        public int Ordre { get; set; }
    }

    public static class EntryOrdreDtoExtensions
    {
        /// <summary>
        /// Converts a JSON EntryOrdreDtoJson object to an XML EntryOrdreDto object.
        /// </summary>
        /// <param name="jsonDto">The JSON EntryOrdreDtoJson object.</param>
        /// <returns>A converted EntryOrdreDto object.</returns>
        public static EntryOrdreDto ToXmlDto(this EntryOrdreDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new EntryOrdreDto
            {
                SourceCode = jsonDto.SourceCode,
                SourceDesc = jsonDto.SourceDesc,
                Ordre = jsonDto.Ordre,
            };
        }
    }
}