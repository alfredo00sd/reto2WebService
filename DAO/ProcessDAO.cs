using reto2Propietaria.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace reto2Propietaria.DAO
{
    public class ProcessDAO
    {
        readonly SqlCommand Cmd = new SqlCommand();
        readonly DBCon Connection = new DBCon();
        SqlDataReader reader;

        public decimal GetAmount(string desde, string hasta) 
        {
            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "select sum(amount) as total_credit from transaction_log where (date >= @Desde and date <= @Hasta) and type = 'CR' and status = 0";
            Cmd.CommandType = CommandType.Text;
            Cmd.Parameters.AddWithValue("@Desde", desde);
            Cmd.Parameters.AddWithValue("@Hasta", hasta);

            reader = Cmd.ExecuteReader();

            if (reader.Read() && !reader.IsDBNull(0))
            {
                return reader.GetDecimal(0);
            }
            else 
            {
                return 0;
            }
        }

        public string LogOnDB(string desde, string hasta) 
        {
            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "update transaction_log set status = 1 where (date >= @Desde and date <= @Hasta) and type = 'CR'";
            Cmd.CommandType = CommandType.Text;
            Cmd.Parameters.AddWithValue("@Desde", desde);
            Cmd.Parameters.AddWithValue("@Hasta", hasta);

            int result = Cmd.ExecuteNonQuery();

            CloseConnections(Connection, Cmd, null);

            if (result > 1)
            {
                return "OK";
            }
            else 
            {
                return "ERR saving data on db for transaction desde: " + desde + " hasta: "+ hasta;
            }
        }

        public string ProcessPago(int empId, string entries, string deductions, string concept, decimal amount) {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "insert into transaction_log values(@EmpId, @Type, @Entries, @Deductions, @Concept, ''+GETDATE(), @Amount, 0)";
            Cmd.CommandType = CommandType.Text;
            Cmd.Parameters.AddWithValue("@EmpId", empId);
            Cmd.Parameters.AddWithValue("@Type", "CR");
            Cmd.Parameters.AddWithValue("@Entries", entries);
            Cmd.Parameters.AddWithValue("@Deductions", deductions);
            Cmd.Parameters.AddWithValue("@Concept", concept);
            Cmd.Parameters.AddWithValue("@Amount", amount);


            int result = Cmd.ExecuteNonQuery();

            CloseConnections(Connection, Cmd, null);

            if (result > 0)
            {
                return "Transaccion procesada, estado 0 - por procesar asiento contable";
            }
            else
            {
                return "Error tratando de insertar...";
            }
        }

        public List<TransaccionLog> GetTransactions(string transType, int idEmp, string fecha_desde, string fecha_hasta, int enviadas)
        {
            List<TransaccionLog> dtoList = new List<TransaccionLog>();
            //enviadas (1 = procesadas, 0 = por procesar)
            string param1 = transType.Length > 1 ? " and type = @Type " : ""; 
            string param2 = idEmp > 0 ? " and employee_id = @EmpId " : "";
            string param3 = enviadas == 1 || enviadas == 0 ? " and status = @Status " : "";  
           

            //get by dates wich where sent or not. (date format 2020-11-28)
            string query = "select * from transaction_log where(date >= @FechaDesde and date <= @FechaHasta)" + param1 + param2 + param3;
            //get by employee_id + period
            //query = "select * from transaction_log where(date >= @FechaDesde and date <= @FechaHasta) and employee_id = @EmpId";
            //get by type + period
            //query = "select * from transaction_log where(date >= @FechaDesde and date <= @FechaHasta) and type = @Type";
            //get all in period
            //query = "select* from transaction_log where(date >= @FechaDesde and date <= @FechaHasta)";


            Cmd.Connection = Connection.Open();
            Cmd.CommandText = query;
            Cmd.CommandType = CommandType.Text;
            Cmd.Parameters.AddWithValue("@EmpId", idEmp);
            Cmd.Parameters.AddWithValue("@Type", transType);
            Cmd.Parameters.AddWithValue("@FechaDesde", fecha_desde);
            Cmd.Parameters.AddWithValue("@FechaHasta", fecha_hasta);
            Cmd.Parameters.AddWithValue("@Status", enviadas);
            
            reader = Cmd.ExecuteReader();

            while (reader.Read())
            {
                dtoList.Add(new TransaccionLog
                {
                    Id = reader.GetInt32(0),
                    EmpId = reader.GetInt32(1),
                    TranscType = reader.GetString(2),
                    EntryDetails = reader.GetString(3),
                    DeductionDetails = reader.GetString(4),
                    Concept = reader.GetString(5),
                    Date = reader.GetString(6),
                    Amount = reader.GetDecimal(7),
                    State = reader.GetBoolean(8)
                });
            }

            CloseConnections(Connection, Cmd, reader);

            return dtoList;
        }

        private void CloseConnections(DBCon connection, SqlCommand command, SqlDataReader reader)
        {
            if (command != null)
            {
                command.Parameters.Clear();
                connection.Close();
            }

            if (reader != null)
            {
                reader.Close();
            }
        }
    }
}