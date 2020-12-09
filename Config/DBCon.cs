using System.Data;
using System.Data.SqlClient;

namespace reto2Propietaria
{
    public class DBCon
    {
        protected SqlConnection Conexion = new SqlConnection("Server=tcp:nomina-server.database.windows.net,1433;Initial Catalog=nomina_system;Persist Security Info=False;User ID=adminNomina;Password=Propietaria2020;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //protected SqlConnection Conexion = new SqlConnection("Server=DESKTOP-EOOHF5T;DataBase=nomina_system;Integrated Security=true");

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