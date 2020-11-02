using reto2Propietaria.DTOs;
using System.Collections.Generic;
using System.Web.Services;

namespace reto2Propietaria
{
    [WebService(Namespace = "https://github.com/alfredo00sd/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GestionNomina : WebService
    {

        private readonly EmployeeDao DAO = new EmployeeDao();

        [WebMethod]
        public string Greetings()
        {
            return "Saludos, que tal la life.";
        }

        [WebMethod]
        public string AddEmployee(EmployeeDTO employee)
        {            
            return DAO.Add(employee);
        }

        [WebMethod]
        public List<Employee> Listar_Empleados()
        {
            return DAO.GetEmployees();
        }

        [WebMethod]
        public string Remover_Empleado(int id)
        {
            return DAO.Delete(id);
        }

        [WebMethod]
        public Employee Buscar_Empleado(string argumento)
        {
            return DAO.GetEmployeeBy(argumento);
        }
    }
}
