using System.Collections.Generic;
using System.Xml.Serialization;

namespace VetCore.Anmv.Xml.Descriptions
{
    [XmlRoot("donnees-reference-group")]
    public sealed class DonneesReferenceGroupDto
    {
        [XmlElement("term-nat")]
        public TermNatDto TermNat { get; set; }

        [XmlElement("term-tit")]
        public TermTitDto TermTit { get; set; }
    }

    /// <summary>
    /// Natures de médicaments
    /// </summary>
    public class TermNatDto
    {
        [XmlElement("entry")]
        public List<EntryDto> Entries { get; set; }
    }

    /// <summary>
    /// Titulaires d'AMM
    /// </summary>
    public class TermTitDto
    {
        [XmlElement("entry")]
        public List<EntryDto> Entries { get; set; }
    }
}