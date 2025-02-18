using System;
using System.Linq;
using System.Text.Json.Serialization;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Json.Description
{
    /// <summary>
    /// Root for ANMV descriptions (JSON version)
    /// </summary>
    public sealed class DonneesReferenceGroupDtoJson
    {
        [JsonPropertyName("termNat")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermNatDtoJson[] TermNat { get; set; } = Array.Empty<TermNatDtoJson>();

        [JsonPropertyName("termTit")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermTitDtoJson[] TermTit { get; set; } = Array.Empty<TermTitDtoJson>();

        [JsonPropertyName("termTypProc")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermTypProcDtoJson[] TermTypProc { get; set; } = Array.Empty<TermTypProcDtoJson>();

        [JsonPropertyName("termStatAuto")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermStatAutoDtoJson[] TermStatAuto { get; set; } = Array.Empty<TermStatAutoDtoJson>();

        [JsonPropertyName("termFp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermFpDtoJson[] TermFp { get; set; } = Array.Empty<TermFpDtoJson>();

        [JsonPropertyName("termEsp")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermEspDtoJson[] TermEsp { get; set; } = Array.Empty<TermEspDtoJson>();

        [JsonPropertyName("termSa")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermSaDtoJson[] TermSa { get; set; } = Array.Empty<TermSaDtoJson>();

        [JsonPropertyName("termVa")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermVaDtoJson[] TermVa { get; set; } = Array.Empty<TermVaDtoJson>();

        [JsonPropertyName("termCd")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermCdDtoJson[] TermCd { get; set; } = Array.Empty<TermCdDtoJson>();

        [JsonPropertyName("termDenr")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermDenrDtoJson[] TermDenr { get; set; } = Array.Empty<TermDenrDtoJson>();

        [JsonPropertyName("termPres")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermPresDtoJson[] TermPres { get; set; } = Array.Empty<TermPresDtoJson>();

        [JsonPropertyName("termUnite")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermUniteDtoJson[] TermUnite { get; set; } = Array.Empty<TermUniteDtoJson>();

        [JsonPropertyName("termTitre")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TermTitreDtoJson[] TermTitre { get; set; } = Array.Empty<TermTitreDtoJson>();
    }


    public static class DonneesReferenceGroupDtoExtensions
    {
        /// <summary>
        /// Converts a JSON DonneesReferenceGroupDtoJson object to an XML DonneesReferenceGroupDto object.
        /// </summary>
        /// <param name="jsonDto">The JSON DonneesReferenceGroupDtoJson object.</param>
        /// <returns>A converted DonneesReferenceGroupDto object.</returns>
        public static DonneesReferenceGroupDto ToXmlDto(this DonneesReferenceGroupDtoJson jsonDto)
        {
            if (jsonDto == null)
                return null;

            return new DonneesReferenceGroupDto
            {
                TermNat = jsonDto.TermNat?.Select(e => e.ToXmlDto()).ToList(),
                TermTit = jsonDto.TermTit?.Select(e => e.ToXmlDto()).ToList(),
                TermTypProc = jsonDto.TermTypProc?.Select(e => e.ToXmlDto()).ToList(),
                TermStatAuto = jsonDto.TermStatAuto?.Select(e => e.ToXmlDto()).ToList(),
                TermFp = jsonDto.TermFp?.Select(e => e.ToXmlDto()).ToList(),
                TermEsp = jsonDto.TermEsp?.Select(e => e.ToXmlDto()).ToList(),
                TermSa = jsonDto.TermSa?.Select(e => e.ToXmlDto()).ToList(),
                TermVa = jsonDto.TermVa?.Select(e => e.ToXmlDto()).ToList(),
                TermCd = jsonDto.TermCd?.Select(e => e.ToXmlDto()).ToList(),
                TermDenr = jsonDto.TermDenr?.Select(e => e.ToXmlDto()).ToList(),
                TermPres = jsonDto.TermPres?.Select(e => e.ToXmlDto()).ToList(),
                TermUnite = jsonDto.TermUnite?.Select(e => e.ToXmlDto()).ToList(),
                TermTitre = jsonDto.TermTitre?.Select(e => e.ToXmlDto()).ToList(),
            };
        }
    }
}