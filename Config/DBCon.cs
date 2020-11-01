using System.Data;
using System.Data.SqlClient;

namespace reto2Propietaria
{
    public class DBCon
    {
        protected SqlConnection Conexion = new SqlConnection("Server=DESKTOP-EOOHF5T;DataBase=Reto2Prop;Integrated Security=true");

        public SqlConnection Open()
        {
            if (Conexion.State == ConnectionState.Closed)
                Conexion.Open();

            return Conexion;
        }
        public SqlConnection Close()
        {
            if (Conexion.State == ConnectionState.Open)
                Conexion.Close();

            return Conexion;
        }
    }
}