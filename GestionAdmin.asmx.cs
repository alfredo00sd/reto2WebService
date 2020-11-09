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
    public class GestionIngDeb : System.Web.Services.WebService
    {

        private readonly EntryDao DAO = new EntryDao();

        //----------------------------------------------Departments
        //Add
        //Edit
        //Remove
        //See all

        //----------------------------------------------Entry_type/Deduction_type
        //Add
        [WebMethod]
        public string Add(EntryType entry, string tipoIngresoDeduccion) 
        {

            string result;

            switch (tipoIngresoDeduccion) 
            {
                case "Ingreso" :
                case "INGRESO" :
                case "ingreso" :
                    result = DAO.Add(entry, "entry_type"); break;
                case "Deduccion" :
                case "deduccion" :
                case "DEDUCCION" :
                    result = DAO.Add(entry, "deduction_type");
                    break;

                default : result = "Argumuento tipo incorrecto, favor coloar tipo : Ingreso o Deduccion"; break; 
            }

            return result;
        
        }
        
        //Edit
        [WebMethod]
        public string Edit(EntryType entry, string tipoIngresoDeduccion)
        {

            string result;

            switch (tipoIngresoDeduccion)
            {
                case "Ingreso":
                case "INGRESO":
                case "ingreso":
                    result = DAO.Edit(entry, "entry_type"); break;
                case "Deduccion":
                case "deduccion":
                case "DEDUCCION":
                    result = DAO.Edit(entry, "deduction_type");
                    break;

                default: result = "Argumuento tipo incorrecto, favor coloar tipo : Ingreso o Deduccion"; break;
            }

            return result;

        }
        
        //Remove
        [WebMethod]
        public string Delete(int id, string tipoIngresoDeduccion)
        {

            string result;

            switch (tipoIngresoDeduccion)
            {
                case "Ingreso":
                case "INGRESO":
                case "ingreso":
                    result = DAO.Delete(id, "entry_type"); break;
                case "Deduccion":
                case "deduccion":
                case "DEDUCCION":
                    result = DAO.Delete(id, "deduction_type");
                    break;

                default: result = "Argumuento tipo incorrecto, favor coloar tipo : Ingreso o Deduccion"; break;
            }

            return result;

        }
        
        //See all
        [WebMethod]
        public List<EntryType> GetAll(string tipoIngresoDeduccion)
        {

            List<EntryType> result;

            switch (tipoIngresoDeduccion)
            {
                case "Ingreso":
                case "INGRESO":
                case "ingreso":
                    result = DAO.GetAll("entry_type"); break;
                case "Deduccion":
                case "deduccion":
                case "DEDUCCION":
                    result = DAO.GetAll("deduction_type");
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
