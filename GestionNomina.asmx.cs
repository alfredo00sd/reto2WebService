using reto2Propietaria.DAO;
using System.Collections.Generic;
using System.Web.Services;

namespace reto2Propietaria
{
    [WebService(Namespace = "https://github.com/alfredo00sd/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class GestionNomina : WebService
    {

        private readonly EmployeeDao DAO = new EmployeeDao();
        private readonly EmployeeDepartmentDao departmentDao = new EmployeeDepartmentDao();

        [WebMethod]
        public string Crear_Empleado(Employee employee)
        {
            return CheckEmployeData(employee, true);
        }

        private string CheckEmployeData(Employee employee, bool INSERT) 
        {
            string MSGResponse;
            int result;

            //Valid cedula number
            if (employee.Cedula.Length == 11 && long.TryParse(employee.Cedula, out _))
            {
                //Has valid department ?
                if (departmentDao.GetById(employee.DepartamentId) != null)
                {
                    if (INSERT)
                    {
                        //Cedula and nomina are unique.
                        if (DAO.GetEmployeeBy(employee.Cedula).Count == 0 && DAO.GetEmployeeBy(employee.Nomina.ToString()).Count == 0)
                        {
                            result = DAO.Add(employee);
                         
                            if (result > 0)
                            {
                                MSGResponse = "Emplead@, " + employee.Name + " agregado!";
                            }
                            else
                            {
                                MSGResponse = "Error tratando de insertar...";
                            }
                        }
                        else
                        {
                            MSGResponse = "Esta cedula o nomina ya existe en el sistema.";
                        }
                    }
                    else 
                    {
                        DAO.GetEmployeeById(employee.Id, employee.Cedula);
                        result = DAO.Edit(employee);
                            
                        if (result > 0)
                        {
                            MSGResponse = "Emplead@ "+employee.Name+" actualizado!";
                        }
                        else
                        {
                            MSGResponse = "Error actualizando emplead@";
                        }
                    }
                }
                else
                {
                    MSGResponse = "Departamento no encontrado, favor revalidar";
                }   
            }
            else
            {
                MSGResponse = "Empleado invalido, favor revisar cedula.";
            }
            return MSGResponse;
        }

        [WebMethod]
        public string Editar_Empleado(Employee employee)
        {
            return CheckEmployeData(employee, false);
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
