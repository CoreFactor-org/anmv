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

    public sealed class EntryOrdreDto : EntryDto
    {
        [XmlElement("ordre")] public int Ordre { get; set; }
    }

    public static class XmlDtoExtensions
    {
        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermNatDtoJson ToJsonTermNatDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermNatDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermTitDtoJson ToJsonTermTitDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermTitDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermTypProcDtoJson ToJsonTermTypProcDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermTypProcDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermStatAutoDtoJson ToJsonTermStatAutoDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermStatAutoDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermFpDtoJson ToJsonTermFpDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermFpDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermEspDtoJson ToJsonTermEspDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermEspDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermSaDtoJson ToJsonTermSaDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermSaDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermVaDtoJson ToJsonTermVaDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermVaDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermCdDtoJson ToJsonTermCdDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermCdDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermQspDtoJson ToJsonTermQspDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermQspDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermDenrDtoJson ToJsonTermDenrDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermDenrDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermPresDtoJson ToJsonTermPresDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermPresDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermUniteDtoJson ToJsonTermUniteDto(this EntryDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermUniteDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
            };
        }

        /// <summary>
        /// Converts an XML Dto to a JSON Dto.
        /// </summary>
        public static TermTitreDtoJson ToJsonTermTitreDto(this EntryOrdreDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new TermTitreDtoJson
            {
                SourceCode = xmlDto.SourceCode,
                SourceDesc = xmlDto.SourceDesc,
                Ordre = xmlDto.Ordre,
            };
        }
    }
}