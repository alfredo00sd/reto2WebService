using reto2Propietaria.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace reto2Propietaria.DAO
{
    public class EmployeeDepartmentDao
    {
        private readonly DBCon Connection = new DBCon();
        readonly SqlCommand Cmd = new SqlCommand();
        SqlDataReader Reader;

        //Queries
        private const string INSERT = "insert into departments values(@Code, @Description, 1)";
        private const string DELETE = "update departments set state = 0 where id = @id";
        private const string GET_BY = "select * from departments where id = @Id and state = 1";
        private const string GET_ALL = "select * from departments where state = 1";
        private const string UPDATE = "update departments set code = @Code, description = @Description where id = @Id";

        //Create
        public string Add(EmployeeDepartment department)
        {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = INSERT;
            Cmd.CommandType = CommandType.Text;

            FillDepartmentParams(Cmd, department);

            int result = Cmd.ExecuteNonQuery();

            CloseConnections(Connection, Cmd, null);

            if (result > 0)
            {
                return "Departamento agregado!";
            }
            else
            {
                return "Error tratando de insertar...";
            }
        }

        //Update
        public string Edit(EmployeeDepartment department)
        {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = UPDATE;
            Cmd.CommandType = CommandType.Text;

            FillDepartmentParams(Cmd, department);

            int result = Cmd.ExecuteNonQuery();

            CloseConnections(Connection, Cmd, null);

            if (result > 0)
            {
                return "Departamento actualizado!";
            }
            else
            {
                return "Error tratando de editar...";
            }
        }

        //GetAll
        public List<EmployeeDepartment> GetAll()
        {

            List<EmployeeDepartment> dtoList;

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = GET_ALL;
            Cmd.CommandType = CommandType.Text;
            Reader = Cmd.ExecuteReader();

            dtoList = FillDepartmentList(Reader);

            CloseConnections(Connection, null, Reader);

            return dtoList;
        }

        //Get by Id
        public EmployeeDepartment GetById(int Id)
        {
            Cmd.Connection = Connection.Open();
            Cmd.CommandText = GET_BY;
            Cmd.CommandType = CommandType.Text;
            Cmd.Parameters.AddWithValue("@Id", Id);

            Reader = Cmd.ExecuteReader();

            if (Reader.Read())
            {
                EmployeeDepartment department = new EmployeeDepartment
                {
                    Id = Reader.GetInt32(0),
                    Code = Reader.GetString(1),
                    Description = Reader.GetString(2),
                    Status = Reader.GetBoolean(3)
                };
                return department;
            }
            else
            {
                return null;
            }
        }

        //Delete
        public string Delete(int id)
        {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = DELETE;
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
        public List<EmployeeDepartment> FillDepartmentList(SqlDataReader reader)
        {
            List<EmployeeDepartment> entryList = new List<EmployeeDepartment>();

            while (reader.Read())
            {
                entryList.Add(new EmployeeDepartment
                {
                    Id = reader.GetInt32(0),
                    Code = reader.GetString(1),
                    Description = reader.GetString(2),
                    Status = reader.GetBoolean(3)
                });
            }

            return entryList;
        }

        private void FillDepartmentParams(SqlCommand cmd, EmployeeDepartment e)
        {
            if (e.Id > 0)
            {
                cmd.Parameters.AddWithValue("@Id", e.Id);
            }
            cmd.Parameters.AddWithValue("@Title", e.Code);
            cmd.Parameters.AddWithValue("@Description", e.Description);
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