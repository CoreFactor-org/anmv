using System.Xml.Serialization;

namespace VetCore.Anmv.Xml.Descriptions
{
    public sealed class EntryOrdreDto : EntryDto
    {
        [XmlElement("ordre")]
        public int Ordre { get; set; }
    }
}