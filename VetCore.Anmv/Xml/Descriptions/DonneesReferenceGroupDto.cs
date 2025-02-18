using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using VetCore.Anmv.Json.Description;

namespace VetCore.Anmv.Xml.Descriptions
{
    /// <summary>
    /// Racine des descriptions ANMV
    /// </summary>
    [XmlRoot("donnees-reference-group")]
    public sealed class DonneesReferenceGroupDto
    {
        /// <summary>
        /// Natures de médicaments
        /// </summary>
        [XmlArray("term-nat")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermNat { get; set; }

        /// <summary>
        /// Titulaires d'AMM
        /// </summary>
        [XmlArray("term-tit")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermTit { get; set; }

        /// <summary>
        /// Types de procédure
        /// </summary>
        [XmlArray("term-typ-proc")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermTypProc { get; set; }

        /// <summary>
        /// Statuts d'autorisation
        /// </summary>
        [XmlArray("term-stat-auto")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermStatAuto { get; set; }

        /// <summary>
        /// Formes pharmaceutiques
        /// </summary>
        [XmlArray("term-fp")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermFp { get; set; }

        /// <summary>
        /// Espèces de destination
        /// </summary>
        [XmlArray("term-esp")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermEsp { get; set; }

        /// <summary>
        /// Substances actives
        /// </summary>
        [XmlArray("term-sa")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermSa { get; set; }

        /// <summary>
        /// Voies d'administration
        /// </summary>
        [XmlArray("term-va")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermVa { get; set; }

        /// <summary>
        /// Conditions de délivrance
        /// </summary>
        [XmlArray("term-cd")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermCd { get; set; }

        /// <summary>
        /// Denrées
        /// </summary>
        [XmlArray("term-denr")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermDenr { get; set; }

        /// <summary>
        /// Présentations
        /// </summary>
        [XmlArray("term-pres")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermPres { get; set; }

        /// <summary>
        /// Unités
        /// </summary>
        [XmlArray("term-unite")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermUnite { get; set; }

        /// <summary>
        /// Titres paragraphes RCP
        /// </summary>
        [XmlArray("term-titre")]
        [XmlArrayItem("entry")]
        public List<EntryOrdreDto> TermTitre { get; set; }
    }

    public static class DonneesReferenceGroupDtoExtensions
    {
        /// <summary>
        /// Converts an XML DonneesReferenceGroupDto object to a JSON DonneesReferenceGroupDtoJson object.
        /// </summary>
        /// <param name="xmlDto">The XML DonneesReferenceGroupDto object.</param>
        /// <returns>A converted DonneesReferenceGroupDtoJson object.</returns>
        public static DonneesReferenceGroupDtoJson ToJsonDto(this DonneesReferenceGroupDto xmlDto)
        {
            if (xmlDto == null)
                return null;

            return new DonneesReferenceGroupDtoJson
            {
                TermNat = xmlDto.TermNat?.Select(e => e.ToJsonTermNatDto()).ToArray() ?? Array.Empty<TermNatDtoJson>(),
                TermTit = xmlDto.TermTit?.Select(e => e.ToJsonTermTitDto()).ToArray() ?? Array.Empty<TermTitDtoJson>(),
                TermTypProc = xmlDto.TermTypProc?.Select(e => e.ToJsonTermTypProcDto()).ToArray() ?? Array.Empty<TermTypProcDtoJson>(),
                TermStatAuto = xmlDto.TermStatAuto?.Select(e => e.ToJsonTermStatAutoDto()).ToArray() ?? Array.Empty<TermStatAutoDtoJson>(),
                TermFp = xmlDto.TermFp?.Select(e => e.ToJsonTermFpDto()).ToArray() ?? Array.Empty<TermFpDtoJson>(),
                TermEsp = xmlDto.TermEsp?.Select(e => e.ToJsonTermEspDto()).ToArray() ?? Array.Empty<TermEspDtoJson>(),
                TermSa = xmlDto.TermSa?.Select(e => e.ToJsonTermSaDto()).ToArray() ?? Array.Empty<TermSaDtoJson>(),
                TermVa = xmlDto.TermVa?.Select(e => e.ToJsonTermVaDto()).ToArray() ?? Array.Empty<TermVaDtoJson>(),
                TermCd = xmlDto.TermCd?.Select(e => e.ToJsonTermCdDto()).ToArray() ?? Array.Empty<TermCdDtoJson>(),
                TermDenr = xmlDto.TermDenr?.Select(e => e.ToJsonTermDenrDto()).ToArray() ?? Array.Empty<TermDenrDtoJson>(),
                TermPres = xmlDto.TermPres?.Select(e => e.ToJsonTermPresDto()).ToArray() ?? Array.Empty<TermPresDtoJson>(),
                TermUnite = xmlDto.TermUnite?.Select(e => e.ToJsonTermUniteDto()).ToArray() ?? Array.Empty<TermUniteDtoJson>(),
                TermTitre = xmlDto.TermTitre?.Select(e => e.ToJsonTermTitreDto()).ToArray() ?? Array.Empty<TermTitreDtoJson>(),
            };
        }
    }
}