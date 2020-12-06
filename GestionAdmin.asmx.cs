using reto2Propietaria.DAO;
using reto2Propietaria.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
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
        public string Procesar_Nomina(string cedula, string entries, string deductions, string concept, decimal amount) {

            //El calculo se hace en el front, me enviara solo que debo guardar.

            //entries/deductions todas separadas por | puede ser... o ,
            return processDAO.ProcessPago(cedula, entries, deductions, concept, amount);
        }

        //----------------------------------------------Consultas
        //Consultar(Ver las transacciones candidatas a ser enviadas)
        //Consulta (transacciones x tipo y empleado en un rango de fechas)
        [WebMethod]
        //Recibe TranscType, idEmpleado, fecha_desde, fecha_hasta, enviados/porEnviar
        //estatus en 0 que significa sin enviar asiento contable.
        public List<TransaccionLog> Consultar_transacciones(string transType, string cedula, string fecha_desde, string fecha_hasta, int enviadas)
        {
            //if (fecha_desde.Length == 10 && fecha_hasta.Length == 10) {

            //return transType + " " + idEmp + " " + fecha_desde + " " + fecha_hasta + " " + enviadas;
            return processDAO.GetTransactions(transType, cedula, fecha_desde, fecha_hasta, enviadas);
            //Type transc emp = CR
            //}

            //return "Transacciones que cumplan los parametros de busqueda";
        }

        //----------------------------------------------Accounting_seat -Need validations
        //Procesar/Enviar asiento contable a WS-externo
        [WebMethod]
        //Recibe N/A
        //Date format : 2020-12-01
        //Busca los estatus en 0 que significa sin enviar asiento contable.
        //Notifica envio.
        public string Enviar_asiento_contable(string desde, string hasta)
        {
            //Cuentas contables : 70 (salarios y Sueldos Empleados) 71 (Gastos de Nomina Empresa)
            //Cada grupo debe enviar al menos dos filas en cada peticion (1 Debito y 1 credito)
            //idCuentaAuxiliar = 2 allways nomina
            
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 

            var url = $"https://plutus.azure-api.net/api/AccountingSeat/InsertAccountingSeats";
            var request = (HttpWebRequest)WebRequest.Create(url);

            //Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

            if (desde.Length == 10 && hasta.Length == 10)
            {
                decimal monto_a_enviar = processDAO.GetAmount(desde, hasta);

                if (monto_a_enviar > 0)
                {
                    //Hoja envio de asiento.
                    Root obj = new Root
                    {
                        descripcion = "Asiento de Nomina de empleados " + DateTime.Now.ToString("Y"),
                        idCuentaAuxiliar = 2,
                        inicioPeriodo = desde,
                        finPeriodo = hasta,
                        moneda = "DOP",
                        asientos = new List<Asiento> { new Asiento { idCuenta = 18, monto = Convert.ToInt32(monto_a_enviar) }, new Asiento { idCuenta = 2, monto = Convert.ToInt32(monto_a_enviar) } }
                    };

                    var data = new JavaScriptSerializer().Serialize(obj);

                    string json = data;
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    request.Accept = "application/json";

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }

                    try
                    {
                        using (WebResponse response = request.GetResponse())
                        {
                            using (Stream strReader = response.GetResponseStream())
                            {
                                if (strReader == null) return "";
                                using (StreamReader objReader = new StreamReader(strReader))
                                {
                                    string responseBody = objReader.ReadToEnd();
                                    if (responseBody.Contains("Su número de asiento es #"))
                                    {
                                        if (desde.Length == 10 && hasta.Length == 10)
                                        {
                                            processDAO.LogOnDB(desde, hasta);
                                        }
                                        else 
                                        {
                                            processDAO.LogOnDB("2017-01-01", DateTime.Now.ToString("yyyy-M-d"));
                                        }
                                    }
                                    return responseBody;
                                }
                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        return "ERR: " + ex.Status + " desc : " + ex.Message;
                    }
                }
                else
                {
                    return "No se han encontrado registros para el periodo evaluado.";

                }
            }
            else 
            {
                return "Formato de fecha invalido, favor enviar con formato 2020-12-31";
            }
        }

        [WebMethod]
        //Obtener los asientos enviados al web-service externo API-contabilidad.
        public string Get_asientos_from_API()
        {
            var url = $"https://plutus.azure-api.net/api/AccountingSeat/GetSeatByAuxiliar/2";
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/json";
            request.Accept = "application/json";

            try
            {
                
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream strReader = response.GetResponseStream())
                    {
                        if (strReader == null) return "NULL";
                        using (StreamReader objReader = new StreamReader(strReader))
                        {
                            string responseBody = objReader.ReadToEnd();
                            // Do something with responseBody
                            return responseBody;
                            //var model = JsonConvert.DeserializeObject<List<Root>>(request.GetResponse());

                        }
                    }
                }
                
            }
            catch (WebException ex)
            {
                return "ERR: " + ex.Status + " desc: "+ex.Message;
                // Handle error
            }
            /*
            List<AsientoFromAPI> list = new List<AsientoFromAPI>();

            //Contabilidad rest API
            string url = "https://plutus.azure-api.net/api/AccountingSeat/GetSeatByAuxiliar/2";

            // GET data from API & map to POCO
            list.Add(url.GetJsonFromUrl().FromJson<AsientoFromAPI>());
            
            return list;
            */
        }

        [WebMethod]
        //Busca los asientos no enviados en el perido indicado o periodo general.
        //Consultar data a ser enviada para asiento contable.
        public List<TransaccionLog> Data_para_asiento(string desde, string hasta)
        {
            if (desde.Length > 5 && hasta.Length > 5)
            {
                return Consultar_transacciones("N", "NULL", desde, hasta, 0);
            }
            else 
            {
                return Consultar_transacciones("N", "NULL", "2017-01-01", DateTime.Now.ToString("yyyy-M-d"), 0);
            }
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

public class Asiento
{
    public int idCuenta { get; set; }
    public int monto { get; set; }
}

public class Root
{
    public string descripcion { get; set; }
    public int idCuentaAuxiliar { get; set; }
    public string inicioPeriodo { get; set; }
    public string finPeriodo { get; set; }
    public string moneda { get; set; }
    public List<Asiento> asientos { get; set; }
}
