using reto2Propietaria.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace reto2Propietaria.DAO
{
    public class EntryDao
    {
        private readonly DBCon Connection = new DBCon();
        readonly SqlCommand Cmd = new SqlCommand();
        SqlDataReader Reader;

        //Create
        public string Add(EntryType entry, string table)
        {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "insert into "+table+ " values(@Title, @Value, @Description, @SalaryDependent, FORMAT(getdate(), 'yyyy-M-dd'), 1)";
            Cmd.CommandType = CommandType.Text;

            FillEntryParams(Cmd, entry, 0);

            int result = Cmd.ExecuteNonQuery();

            CloseConnections(Connection, Cmd, null);

            if (result > 0)
            {
                return  table.Equals("entry_type") ? " Definicion de "+entry.Title+" ingreso agregado! " : " Definicion " + entry.Title + " para deduccion agregada!";
            }
            else
            {
                return "Error tratando de insertar...";
            }
        }

        //Update
        public string Edit(EntryType entry, string table, int id)
        {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "update "+table+ " set title = @Title, value = @Value, description = @Description, salary_dependent = @SalaryDependent where id = @Id";
            Cmd.CommandType = CommandType.Text;

            FillEntryParams(Cmd, entry, id);

            int result = Cmd.ExecuteNonQuery();

            CloseConnections(Connection, Cmd, null);

            if (result > 0)
            {
                return table.Equals("entry_type") ? " Definicion de " + entry.Title + " ingreso editado! " : " Definicion " + entry.Title + " para deduccion editada!";
            }
            else
            {
                return "Error tratando de editar...";
            }
        }
        
        //GetAll
        public List<EntryType> GetAll(string table)
        {

            List<EntryType> dtoList;

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "select * from "+table+" where state = 1";
            Cmd.CommandType = CommandType.Text;
            Reader = Cmd.ExecuteReader();

            dtoList = FillEntryList(Reader);

            CloseConnections(Connection, null, Reader);

            return dtoList;
        }

        //Get by Id
        public EntryType GetById(int Id, string table)
        {
            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "select * from "+table+" where id = @Id and state = 1";
            Cmd.CommandType = CommandType.Text;
            Cmd.Parameters.AddWithValue("@Id", Id);

            Reader = Cmd.ExecuteReader();

            if (Reader.Read())
            {
                EntryType entry = new EntryType
                {
                    Id = Reader.GetInt32(0),
                    Title = Reader.GetString(1),
                    Value = Reader.GetDecimal(2),
                    Description = Reader.GetString(3),
                    SalaryDependent = Reader.GetBoolean(4),
                    AddedDate = Reader.GetString(5),
                    State = Reader.GetBoolean(6)
                };
                return entry;
            }
            else
            {
                return null;
            }
        }

        //Delete
        public string Delete(int id, string table)
        {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "update "+table+" set state = 0 where id = @id ";
            Cmd.CommandType = CommandType.Text;
            Cmd.Parameters.AddWithValue("@id", id);

            int conunt = Cmd.ExecuteNonQuery();

            CloseConnections(Connection, Cmd, null);

            if (conunt > 0)
            {
                return "Eliminado";
            }
            else
            {
                return "Error al intentar eliminar";
            }
        }

        //Fills
        public List<EntryType> FillEntryList(SqlDataReader reader)
        {
            List<EntryType> entryList = new List<EntryType>();

            while (reader.Read())
            {
                entryList.Add(new EntryType
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Value =  reader.GetDecimal(2),
                    Description = reader.GetString(3),
                    SalaryDependent = reader.GetBoolean(4),
                    AddedDate = reader.GetString(5),
                    State = reader.GetBoolean(6)
                });
            }

            return entryList;
        }

        private void FillEntryParams(SqlCommand cmd, EntryType e, int id)
        {
            if (id > 0)
            {
                cmd.Parameters.AddWithValue("@Id", id);
            }
            cmd.Parameters.AddWithValue("@Title", e.Title);
            cmd.Parameters.AddWithValue("@Value", e.Value);
            cmd.Parameters.AddWithValue("@Description", e.Description);
            cmd.Parameters.AddWithValue("@SalaryDependent", e.SalaryDependent);
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