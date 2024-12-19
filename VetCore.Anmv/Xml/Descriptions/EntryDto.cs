using System.Xml.Serialization;

namespace VetCore.Anmv.Xml.Descriptions
{
    public class EntryDto
    {
        [XmlElement("source-code")]
        public int SourceCode { get; set; }

        [XmlElement("source-desc")]
        public string SourceDesc { get; set; }
    }
}