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
    [System.Web.Script.Services.ScriptService]
    public class GestionIngDeb : WebService
    {

        private readonly EntryDao entryDAO = new EntryDao();
        private readonly EmployeeDepartmentDao departmentDAO = new EmployeeDepartmentDao();
        private readonly ProcessDAO processDAO = new ProcessDAO();


        //----------------------------------------------Procesos -Need validations
        [WebMethod]
        //Recibe empleado, lista de ingresos y deducciones. concepto "pago nomina" total a ingresar
        //estatus en 0 que significa sin enviar asiento contable.
        public string Procesar_Nomina(int idEmpleado, string entries, string deductions, string concept, decimal amount) {

            //El calculo se hace en el front, me enviara solo que debo guardar.

            //entries/deductions todas separadas por | puede ser... o ,
            return processDAO.ProcessPago(idEmpleado, entries, deductions, concept, amount);
        }

        //----------------------------------------------Accounting_seat -Need validations
        //Procesar/Enviar asiento contable a WS-externo
        [WebMethod]
        //Recibe N/A
        //Busca los estatus en 0 que significa sin enviar asiento contable.
        //Notifica envio.
        public string Enviar_asiento_contable()
        {
            return "tantos procesados...";
        }

        //----------------------------------------------Consultas
        //Consultar(Ver las transacciones candidatas a ser enviadas)
        //Consulta (transacciones x tipo y empleado en un rango de fechas)
        [WebMethod]
        //Recibe TranscType, idEmpleado, fecha_desde, fecha_hasta, enviados/porEnviar
        //estatus en 0 que significa sin enviar asiento contable.
        public string Consultar_transacciones(int transType, int idEmp, string fecha_desde, string fecha_hasta, bool enviadas)
        {
            return "Transacciones que cumplan los parametros de busqueda";
        }
        //----------------------------------------------Procesos -End

        //----------------------------------------------Departments -Validaded
        //Add
        [WebMethod]
        public string Agregar_Departamento(EmployeeDepartment department) 
        {
            if (department.Code.Length >= 3 && department.Code.Length <= 8)
            {
                return departmentDAO.Add(department);
            }
            else 
            {
                return "Codigo de departamento debe contener entre 3 y 8 caracteres maximo.";
            }
        }

        //Edit
        [WebMethod]
        public string Editar_Departamento(int id, EmployeeDepartment department)
        {
            if (department.Code.Length >= 3 && department.Code.Length <= 8)
            {
                return departmentDAO.Edit(department, id);
            }
            else
            {
                return "Codigo de departamento debe contener entre 3 y 8 caracteres maximo.";
            }
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

        //----------------------------------------------Departments -End

        //----------------------------------------------Entry_type/Deduction_type -Validaded
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
        public string Actualizar_Ing_Ded(EntryType entry, int id, string tipoIngresoDeduccion)
        {
            string result;

            switch (tipoIngresoDeduccion)
            {
                case "Ingreso":
                case "INGRESO":
                case "ingreso":
                    result = entryDAO.Edit(entry, "entry_type", id); break;
                case "Deduccion":
                case "deduccion":
                case "DEDUCCION":
                    result = entryDAO.Edit(entry, "deduction_type", id);
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
        //----------------------------------------------Entry_type/Deduction_type -End

        //----------------------------------------------user_roles
        //Add
        //Edit
        //Remove
        //See all
    }
}
