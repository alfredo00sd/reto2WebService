using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace reto2Propietaria
{
    //Employee CRUD
    public class EmployeeDao
    {
        SqlDataReader reader;
        readonly SqlCommand Cmd = new SqlCommand();
        readonly DBCon Connection = new DBCon();

        //Queries
        private const string UPDATE = "update employee set nomina_id = @NomId, dominican_id = @Cedula, name = @Name, last_name = @LastName, department_id = @Department, work_position = @WorkPosition, salary = @Salary where Id = @id or dominican_id = @Cedula";
        private const string INSERT = "insert into employee values(@NomId, @Cedula, @Department, @Name, @LastName, @WorkPosition, @Salary, convert(date, getDate()), 'N/A', 1)";
        private const string GET_BY_ID = "select * from employee where id = @Id or dominican_id = @Cedula";
        private const string GET_ALL_ACTIVES = "select * from employee where state = 1";
        private const string DELETE = "update employee set state = 0, last_day = convert(date, getDate()) where id = @id ";

        //Get by Name, cedula and nomina
        //Retornar una lista con todos los matchs para la busqueda
        public List<Employee> GetEmployeeBy(string criteria)
        {
            Cmd.Connection = Connection.Open();
            List<Employee> dtoList;
        
            long Nom = 0L;
            
            if (long.TryParse(criteria, out _)) 
            {
                Nom = long.Parse(criteria);
            }

            Cmd.CommandText = "select * from employee where nomina_id = "+ Nom  +" or dominican_id like '%" + criteria + "%' or name like '%" + criteria + "%' or last_name like '%" + criteria + "%' and state = 1";
            Cmd.CommandType = CommandType.Text;

            reader = Cmd.ExecuteReader();
            
            dtoList = FillEmployeeList(reader);

            CloseConnections(Connection, Cmd, reader);

            return dtoList;
        }

        //Create Employees
        public int Add(Employee employee)
        {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = INSERT;
            Cmd.CommandType = CommandType.Text;

            FillEmployeeParams(Cmd, employee);

            int result = Cmd.ExecuteNonQuery();

            CloseConnections(Connection, Cmd, null);

            return result;
        }

        //Read all employees
        public List<Employee> GetEmployees()
        {
            List<Employee> dtoList;

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = GET_ALL_ACTIVES;
            Cmd.CommandType = CommandType.Text;
            reader = Cmd.ExecuteReader();

            dtoList = FillEmployeeList(reader);

            CloseConnections(Connection, null, reader);

            return dtoList;
        }


        //Get by Name, cedula, department
        //Retornar una lista con todos los matchs para la busqueda
        public Employee GetEmployeeById(int Id, string Cedula)
        {
            Cmd.Connection = Connection.Open();
            Cmd.CommandText = GET_BY_ID;
            Cmd.CommandType = CommandType.Text;
            Cmd.Parameters.AddWithValue("@Id", Id);
            Cmd.Parameters.AddWithValue("@Cedula", Cedula);

            reader = Cmd.ExecuteReader();

            if (reader.Read())
            {
                Employee employee = new Employee
                {
                    Id = reader.GetInt32(0),
                    Nomina = reader.GetInt32(1),
                    Cedula = reader.GetString(2),
                    DepartamentId = reader.GetInt32(3),
                    Name = reader.GetString(4),
                    LastName = reader.GetString(5),
                    WorkPosition = reader.GetString(6),
                    Salary = reader.GetDecimal(7),
                    FirstDay = reader.GetString(8),
                    LastDay = reader.GetString(9),
                    Status = reader.GetBoolean(10)
                };
                CloseConnections(Connection, Cmd, reader);
                return employee;
            }
            else
            {
                CloseConnections(Connection, Cmd, reader);
                return null;
            }
        }

        //Update employee
        //Consultar Id para retornar la info del empleado y poder editar... "Conversion failed when converting the nvarchar value 'Bianca' to data type int."
        public int Edit(Employee e)
        {
            Cmd.Connection = Connection.Open();
            Cmd.CommandText = UPDATE;
            Cmd.CommandType = CommandType.Text;

            FillEmployeeParams(Cmd, e);

            int result = Cmd.ExecuteNonQuery();

            CloseConnections(Connection, Cmd, null);

            return result;
        }

        //Delete employee
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
                return "Emplead@ eliminado";
            }
            else
            {
                return "Error al intentar eliminar";
            }
        }

        //Utils
        public List<Employee> FillEmployeeList(SqlDataReader reader)
        {
            List<Employee> empList = new List<Employee>();

            while (reader.Read())
            {
                empList.Add(new Employee
                {
                    Id = reader.GetInt32(0),
                    Nomina = reader.GetInt32(1),
                    Cedula = reader.GetString(2),
                    DepartamentId = reader.GetInt32(3),
                    Name = reader.GetString(4),
                    LastName = reader.GetString(5),
                    WorkPosition = reader.GetString(6),
                    Salary = reader.GetDecimal(7),
                    FirstDay = reader.GetString(8),
                    LastDay = reader.GetString(9),
                    Status = reader.GetBoolean(10)
                });
            }
            return empList;
        }

        private void FillEmployeeParams(SqlCommand cmd, Employee e)
        {
            cmd.Parameters.AddWithValue("@Id", e.Id);
            cmd.Parameters.AddWithValue("@NomId", e.Nomina);
            cmd.Parameters.AddWithValue("@Cedula", e.Cedula);
            cmd.Parameters.AddWithValue("@Name", e.Name);
            cmd.Parameters.AddWithValue("@LastName", e.LastName);
            cmd.Parameters.AddWithValue("@Department", e.DepartamentId);
            cmd.Parameters.AddWithValue("@WorkPosition", e.WorkPosition);
            cmd.Parameters.AddWithValue("@Salary", e.Salary);
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