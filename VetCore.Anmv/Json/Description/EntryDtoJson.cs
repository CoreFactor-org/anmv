using System.Text.Json.Serialization;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Json.Description
{
    public class EntryDtoJson
    {
        [JsonPropertyName("sourceCode")]
        public int SourceCode { get; set; }

        [JsonPropertyName("sourceDesc")]
        public string SourceDesc { get; set; }
    }

    public static class JsonDtoExtensions
    {
        /// <summary>
        /// Converts a JSON EntryDtoJson object to an XML EntryDto object.
        /// </summary>
        /// <param name="jsonDto">The JSON EntryDtoJson object.</param>
        /// <returns>A converted EntryDto object.</returns>
        public static EntryDto ToXmlDto(this EntryDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new EntryDto
            {
                SourceCode = jsonDto.SourceCode,
                SourceDesc = jsonDto.SourceDesc,
            };
        }
    }
}