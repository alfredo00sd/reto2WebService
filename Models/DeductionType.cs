using System.Xml.Serialization;

namespace reto2Propietaria.Models
{
    public class DeductionType
    {
        [XmlIgnore()]
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Value { get; set; }
        public string Description { get; set; }
        public bool SalaryDependent { get; set; }
        [XmlIgnore()]
        public string AddedDate { get; set; }
        public bool State { get; set; }
    }
}