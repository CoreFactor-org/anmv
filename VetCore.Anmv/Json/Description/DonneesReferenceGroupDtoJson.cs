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
        [JsonPropertyName("termNat")] public EntryDtoJson[] TermNat { get; set; }

        [JsonPropertyName("termTit")] public EntryDtoJson[] TermTit { get; set; }

        [JsonPropertyName("termTypProc")] public EntryDtoJson[] TermTypProc { get; set; }

        [JsonPropertyName("termStatAuto")] public EntryDtoJson[] TermStatAuto { get; set; }

        [JsonPropertyName("termFp")] public EntryDtoJson[] TermFp { get; set; }

        [JsonPropertyName("termEsp")] public EntryDtoJson[] TermEsp { get; set; }

        [JsonPropertyName("termSa")] public EntryDtoJson[] TermSa { get; set; }

        [JsonPropertyName("termVa")] public EntryDtoJson[] TermVa { get; set; }

        [JsonPropertyName("termCd")] public EntryDtoJson[] TermCd { get; set; }

        [JsonPropertyName("termQsp")] public EntryDtoJson[] TermQsp { get; set; }

        [JsonPropertyName("termDenr")] public EntryDtoJson[] TermDenr { get; set; }

        [JsonPropertyName("termPres")] public EntryDtoJson[] TermPres { get; set; }

        [JsonPropertyName("termUnite")] public EntryDtoJson[] TermUnite { get; set; }

        [JsonPropertyName("termTitre")] public EntryOrdreDtoJson[] TermTitre { get; set; }
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