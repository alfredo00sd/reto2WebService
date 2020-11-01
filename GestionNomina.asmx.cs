using reto2Propietaria.DTOs;
using System;
using System.Web.Services;

namespace reto2Propietaria
{
    [WebService(Namespace = "https://github.com/alfredo00sd/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GestionNomina : System.Web.Services.WebService
    {

        [WebMethod]
        public string Greetings()
        {
            return "Saludos, que tal la life.";
        }

        

        [WebMethod]
        public string AddEmployee(EmployeeDTO employee)
        {
            EmployeeDao dao = new EmployeeDao();
            

            return dao.Add(employee);
        }
    }
}
