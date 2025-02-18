using System.ComponentModel.DataAnnotations;
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
        [JsonPropertyName("termNat")] public TermNatDtoJson[] TermNat { get; set; }
        [JsonPropertyName("termTit")] public TermTitDtoJson[] TermTit { get; set; }
        [JsonPropertyName("termTypProc")] public TermTypProcDtoJson[] TermTypProc { get; set; }
        [JsonPropertyName("termStatAuto")] public TermStatAutoDtoJson[] TermStatAuto { get; set; }
        [JsonPropertyName("termFp")] public TermFpDtoJson[] TermFp { get; set; }
        [JsonPropertyName("termEsp")] public TermEspDtoJson[] TermEsp { get; set; }
        [JsonPropertyName("termSa")] public TermSaDtoJson[] TermSa { get; set; }
        [JsonPropertyName("termVa")] public TermVaDtoJson[] TermVa { get; set; }
        [JsonPropertyName("termCd")] public TermCdDtoJson[] TermCd { get; set; }
        [JsonPropertyName("termQsp")] public TermQspDtoJson[] TermQsp { get; set; }
        [JsonPropertyName("termDenr")] public TermDenrDtoJson[] TermDenr { get; set; }
        [JsonPropertyName("termPres")] public TermPresDtoJson[] TermPres { get; set; }
        [JsonPropertyName("termUnite")] public TermUniteDtoJson[] TermUnite { get; set; }
        [JsonPropertyName("termTitre")] public TermTitreDtoJson[] TermTitre { get; set; }
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
                TermQsp = jsonDto.TermQsp?.Select(e => e.ToXmlDto()).ToList(),
                TermDenr = jsonDto.TermDenr?.Select(e => e.ToXmlDto()).ToList(),
                TermPres = jsonDto.TermPres?.Select(e => e.ToXmlDto()).ToList(),
                TermUnite = jsonDto.TermUnite?.Select(e => e.ToXmlDto()).ToList(),
                TermTitre = jsonDto.TermTitre?.Select(e => e.ToXmlDto()).ToList(),
            };
        }
    }
}