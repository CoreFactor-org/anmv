using System.Collections.Generic;
using System.Xml.Serialization;

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
        /// Excipients QSP
        /// </summary>
        [XmlArray("term-qsp")]
        [XmlArrayItem("entry")]
        public List<EntryDto> TermQsp { get; set; }

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
}