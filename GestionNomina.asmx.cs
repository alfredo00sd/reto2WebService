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
        public string Crear_Empleado(Employee employee)
        {
            //if (ValidEmployee(employee))
            //{
                return DAO.Add(employee);
            //}
            //else 
            //{
              //  return "Empleado invalido, favor revisar parametros.";
            //}
        }

        /*private bool ValidEmployee(Employee e) 
        {
            bool result;

            result = e.Cedula.Length != 11;
            result = long.TryParse(e.Cedula, out long number);
            
            return result;
        }*/

        [WebMethod]
        public string Editar_Empleado(Employee employee)
        {
            return DAO.Edit(employee);
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
        public List<Employee> Buscar_Empleado(string argumento)
        {
            return DAO.GetEmployeeBy(argumento);
        }
    }
}
