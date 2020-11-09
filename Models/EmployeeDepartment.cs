using System.Xml.Serialization;

namespace reto2Propietaria.Models
{
    public class EmployeeDepartment
    {
        [XmlIgnore()]
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        [XmlIgnore()]
        public bool Status { get; set; }
    }
}