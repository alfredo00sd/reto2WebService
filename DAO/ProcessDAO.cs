using System.Data;
using System.Data.SqlClient;

namespace reto2Propietaria.DAO
{
    public class ProcessDAO
    {
        readonly SqlCommand Cmd = new SqlCommand();
        readonly DBCon Connection = new DBCon();

        public string ProcessPago(int empId, string entries, string deductions, string concept, decimal amount) {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "insert into transaction_log values(@EmpId, 'C', @Entries, @Deductions, @Concept, GETDATE(), @Amount, 0)";
            Cmd.CommandType = CommandType.Text;
            Cmd.Parameters.AddWithValue("@EmpId", empId);
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