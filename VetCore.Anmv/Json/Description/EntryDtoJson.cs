using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Json.Description
{
    public abstract class EntryBase
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "La valeur doit être >= 0")] // minInclusive=0
        [JsonPropertyName("sourceCode")]
        public int SourceCode { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "La longueur doit être comprise entre 1 et 255")] // minLength=1, maxLength=255
        [JsonPropertyName("sourceDesc")]
        public string SourceDesc { get; set; }
    }

    /// <summary>
    /// Do a distinction between each type in json to allow easier filtering and manipulation
    /// </summary>
    public sealed class TermNatDtoJson : EntryBase
    {

    }

    public sealed class TermTitDtoJson : EntryBase
    {
    }

    public sealed class TermTypProcDtoJson : EntryBase
    {
    }

    public sealed class TermStatAutoDtoJson : EntryBase
    {
    }

    public sealed class TermFpDtoJson : EntryBase
    {
    }

    public sealed class TermEspDtoJson : EntryBase
    {
    }

    public sealed class TermSaDtoJson : EntryBase
    {
    }

    public sealed class TermVaDtoJson : EntryBase
    {
    }

    public sealed class TermCdDtoJson : EntryBase
    {
    }

    public sealed class TermDenrDtoJson : EntryBase
    {
    }

    public sealed class TermPresDtoJson : EntryBase
    {
    }

    public sealed class TermUniteDtoJson : EntryBase
    {
    }

    public sealed class TermTitreDtoJson : EntryBase
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "La valeur doit être >= 0")] // minInclusive=0
        [JsonPropertyName("ordre")]
        public int Ordre { get; set; }
    }

    public static class JsonDtoExtensions
    {
        /// <summary>
        /// Converts a JSON EntryOrdreDtoJson object to an XML EntryOrdreDto object.
        /// </summary>
        /// <param name="jsonDto">The JSON EntryOrdreDtoJson object.</param>
        /// <returns>A converted EntryOrdreDto object.</returns>
        public static EntryOrdreDto ToXmlDto(this TermTitreDtoJson jsonDto)
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

        /// <summary>
        /// Converts a JSON EntryDtoJson object to an XML EntryDto object.
        /// </summary>
        /// <param name="jsonDto">The JSON EntryDtoJson object.</param>
        /// <returns>A converted EntryDto object.</returns>
        public static EntryDto ToXmlDto(this EntryBase jsonDto)
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