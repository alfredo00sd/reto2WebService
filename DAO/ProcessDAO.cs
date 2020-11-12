using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace reto2Propietaria.DAO
{
    public class ProcessDAO
    {
        SqlDataReader reader;
        readonly SqlCommand Cmd = new SqlCommand();
        readonly DBCon Connection = new DBCon();

        public string ProcessPago(int empId) {

            Cmd.Connection = Connection.Open();
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.Parameters.Add(empId);

            reader =  Cmd.ExecuteReader();

            if (reader.Read())
            {
                return reader.GetString(0);
            }
            else {

                return "WakaWaka";
            }

            /*
            int result = Cmd.ExecuteNonQuery();

            CloseConnections(Connection, Cmd, null);

            if (result > 0)
            {
                return "Nomina procesada";
            }
            else
            {
                return "Error tratando de insertar...";
            }*/
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