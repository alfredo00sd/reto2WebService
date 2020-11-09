using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace reto2Propietaria
{
    public class Employee
    {
        //Fields
        private int _Id;
        private int _nominaId;
        private string _dominicanId;
        private int _departmentId;
        private string _name;
        private string _lastName;
        private string _workPosition;
        private string _firstDay;
        private string _lastDay;
        private decimal _salary;
        private bool _state;

        //Mehtods
        [XmlIgnore()] 
        public int Id { get => _Id; set => _Id = value; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere completar campo {0}")]
        [Display(Name = "No. nomina")]
        public int Nomina { get => _nominaId; set => _nominaId = value; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere completar campo {0}")]
        [Display(Name = "Cedula")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Los caracteres en el campo {0} deben ser 11")]
        public string Cedula { get => _dominicanId; set => _dominicanId = value; }

        public int DepartamentId { get => _departmentId; set => _departmentId = value; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere completar campo {0}")]
        [Display(Name = "Nombre(s)")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Los caracteres en el campo {0} deben estar entre 30 y 3")]
        public string Name { get => _name; set => _name = value; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Se requiere completar campo {0}")]
        [Display(Name = "Apellido(s)")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Los caracteres en el campo {0} deben estar entre 60 y 3")]
        public string LastName { get => _lastName; set => _lastName = value; }

        public string WorkPosition { get => _workPosition; set => _workPosition = value; }
        
        [XmlIgnore()]
        public string FirstDay { get => _firstDay; set => _firstDay = value; }
        
        [XmlIgnore()]
        public string LastDay { get => _lastDay; set => _lastDay = value; }

        public decimal Salary { get => _salary; set => _salary = value; }
        
        [XmlIgnore()]
        public bool Status { get => _state; set => _state = value; }
    }
}