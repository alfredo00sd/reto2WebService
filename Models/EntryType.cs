
using System.Xml.Serialization;

namespace reto2Propietaria.Models
{
    public class EntryType
    {
        [XmlIgnore()]
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public bool SalaryDependent { get; set; }
        [XmlIgnore()]
        public string AddedDate { get; set; }
        [XmlIgnore()]
        public bool State { get; set; }
    }
}