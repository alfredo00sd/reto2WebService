using reto2Propietaria.DAO;
using reto2Propietaria.Models;
using System.Collections.Generic;
using System.Web.Services;

namespace reto2Propietaria
{
    [WebService(Namespace = "https://github.com/alfredo00sd/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GestionIngDeb : WebService
    {

        private readonly EntryDao entryDAO = new EntryDao();
        private readonly EmployeeDepartmentDao departmentDAO = new EmployeeDepartmentDao();
        private readonly ProcessDAO processDAO = new ProcessDAO();


        //----------------------------------------------Procesos
        [WebMethod]
        public string Procesar_Nomina(int idEmpleado) {

            return processDAO.ProcessPago(idEmpleado);
        }



        //----------------------------------------------Departments
        //Add
        [WebMethod]
        public string Agregar_Departamento(EmployeeDepartment department) 
        {
            return departmentDAO.Add(department);
        }

        //Edit
        [WebMethod]
        public string Editar_Departamento(EmployeeDepartment department)
        {
            return departmentDAO.Edit(department);
        }

        //Remove
        [WebMethod]
        public string Remover_Departamento(int id)
        {
            return departmentDAO.Delete(id);
        }

        //See all
        [WebMethod]
        public List<EmployeeDepartment> Listar_Departamentos()
        {
            return departmentDAO.GetAll();
        }


        //----------------------------------------------Entry_type/Deduction_type
        //Add
        [WebMethod]
        public string Agregar_Ing_Ded(EntryType entry, string tipoIngresoDeduccion) 
        {

            string result;

            switch (tipoIngresoDeduccion) 
            {
                case "Ingreso" :
                case "INGRESO" :
                case "ingreso" :
                    result = entryDAO.Add(entry, "entry_type"); break;
                case "Deduccion" :
                case "deduccion" :
                case "DEDUCCION" :
                    result = entryDAO.Add(entry, "deduction_type");
                    break;

                default : result = "Argumuento tipo incorrecto, favor coloar tipo : Ingreso o Deduccion"; break; 
            }

            return result;
        
        }
        
        //Edit
        [WebMethod]
        public string Actualizar_Ing_Ded(EntryType entry, string tipoIngresoDeduccion)
        {

            string result;

            switch (tipoIngresoDeduccion)
            {
                case "Ingreso":
                case "INGRESO":
                case "ingreso":
                    result = entryDAO.Edit(entry, "entry_type"); break;
                case "Deduccion":
                case "deduccion":
                case "DEDUCCION":
                    result = entryDAO.Edit(entry, "deduction_type");
                    break;

                default: result = "Argumuento tipo incorrecto, favor coloar tipo : Ingreso o Deduccion"; break;
            }

            return result;

        }
        
        //Remove
        [WebMethod]
        public string Remover_Ing_Ded(int id, string tipoIngresoDeduccion)
        {

            string result;

            switch (tipoIngresoDeduccion)
            {
                case "Ingreso":
                case "INGRESO":
                case "ingreso":
                    result = entryDAO.Delete(id, "entry_type"); break;
                case "Deduccion":
                case "deduccion":
                case "DEDUCCION":
                    result = entryDAO.Delete(id, "deduction_type");
                    break;

                default: result = "Argumuento tipo incorrecto, favor coloar tipo : Ingreso o Deduccion"; break;
            }

            return result;

        }
        
        //See all
        [WebMethod]
        public List<EntryType> Listar_Ing_Ded(string tipoIngresoDeduccion)
        {

            List<EntryType> result;

            switch (tipoIngresoDeduccion)
            {
                case "Ingreso":
                case "INGRESO":
                case "ingreso":
                    result = entryDAO.GetAll("entry_type"); break;
                case "Deduccion":
                case "deduccion":
                case "DEDUCCION":
                    result = entryDAO.GetAll("deduction_type");
                    break;

                default: 
                    result = new List<EntryType>();
                    result[0].Title = "Argumuento tipo incorrecto, favor coloar tipo : Ingreso o Deduccion";
                    break;
            }

            return result;

        }

        //Get by

        //----------------------------------------------Transaction_log
        //Add
        //Edit
        //Remove
        //See all

        //----------------------------------------------user_roles
        //Add
        //Edit
        //Remove
        //See all

        //----------------------------------------------Accounting_seat
        //Add
        //Edit
        //Remove
        //See all




        //----------------------------------------------Users/Employee CRUD
        //Add
        //Edit
        //Remove
        //See all
    }
}
