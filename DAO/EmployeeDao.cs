using reto2Propietaria.DTOs;
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
        private const string INSERT = "insert into employee values(@NomId, @Cedula, @Name, @Department, @WorkPosition, @Salary, convert(date, getDate()), 'N/A', 1)";
        private const string GET_ALL_ACTIVES = "select * from employee where EmpState = 1";
        private const string GET_BY = "select * from employee where name like @argument or cedula like @argument or department like @argument";
        private const string DELETE = "delete from employee where id = @id ";
        private const string UPDATE = "update employee set NomId = @NomId, Cedula = @Cedula, Name = @Name, Department = @Department, WorkPosition = @WorkPosition, Salary = @Salary where Id = @Id";

        //Create Employees
        public string Add(EmployeeDTO employee)
        {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = INSERT;
            Cmd.CommandType = CommandType.Text;

            FillEmployeeParams(Cmd, employee);

            int count = Cmd.ExecuteNonQuery();

            if (count > 0)
            {
                return "Empleado, " + employee.Name + " agregado!";
            }
            else
            {
                return "Error tratando de insertar...";
            }
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

            Connection.Close();
            reader.Close();

            return dtoList;
        }

        //Get by Name, cedula, department
        //Retornar una lista con todos los matchs para la busqueda
        public Employee GetEmployeeBy(string argument)
        {
            Cmd.Connection = Connection.Open();
            Cmd.CommandText = GET_BY;
            Cmd.CommandType = CommandType.Text;
            Cmd.Parameters.AddWithValue("@argument", argument);

            reader = Cmd.ExecuteReader();

            if (reader.Read())
            {
                Employee employee = new Employee
                {
                    Id = reader.GetInt32(0),
                    NomId = reader.GetInt32(1),
                    Cedula = reader.GetString(2),
                    Name = reader.GetString(3),
                    Department = reader.GetString(4),
                    WorkPosition = reader.GetString(5),
                    Salary = reader.GetDecimal(6),
                    FirstDay = reader.GetString(7),
                    LastDay = reader.GetString(8),
                    State = reader.GetInt32(9)
                };
                return employee;
            }
            else
            {
                return null;
            }
        }

        //Update employee
        //Consultar Id para retornar la info del empleado y poder editar...
        public string Edit(Employee e)
        {
            Cmd.Connection = Connection.Open();
            Cmd.CommandText = UPDATE;
            Cmd.CommandType = CommandType.Text;

            Cmd.Parameters.AddWithValue("@Id", e.Id);
            Cmd.Parameters.AddWithValue("@NomId", e.NomId);
            Cmd.Parameters.AddWithValue("@Cedula", e.Cedula);
            Cmd.Parameters.AddWithValue("@Name", e.Name);
            Cmd.Parameters.AddWithValue("@Department", e.Department);
            Cmd.Parameters.AddWithValue("@WorkPosition", e.WorkPosition);
            Cmd.Parameters.AddWithValue("@Salary", e.Salary);

            Cmd.Parameters.Clear();
            Connection.Close();

            if (Cmd.ExecuteNonQuery() > 0)
            {
                return "Empleado actualizado!";
            }
            else
            {
                return "Error actualizando el empleado";
            }
        }

        //Delete employee
        public string Delete(int id)
        {
            Cmd.Connection = Connection.Open();
            Cmd.CommandText = DELETE;
            Cmd.CommandType = CommandType.Text;
            Cmd.Parameters.AddWithValue("@id", id);

            int conunt = Cmd.ExecuteNonQuery();

            Cmd.Parameters.Clear();
            Connection.Close();

            if (conunt > 0)
            {
                return "Empleado eliminado";
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
                    NomId = reader.GetInt32(1),
                    Cedula = reader.GetString(2),
                    Name = reader.GetString(3),
                    Department = reader.GetString(4),
                    WorkPosition = reader.GetString(5),
                    Salary = reader.GetDecimal(6),
                    FirstDay = reader.GetString(7),
                    LastDay = reader.GetString(8),
                    State = reader.GetInt32(9)
                });
            }

            reader.Close();

            return empList;
        }

        private void FillEmployeeParams(SqlCommand cmd, EmployeeDTO emp)
        {
            cmd.Parameters.AddWithValue("@NomId", emp.NomId);
            cmd.Parameters.AddWithValue("@Cedula", emp.Cedula);
            cmd.Parameters.AddWithValue("@Name", emp.Name);
            cmd.Parameters.AddWithValue("@Department", emp.Department);
            cmd.Parameters.AddWithValue("@WorkPosition", emp.WorkPosition);
            cmd.Parameters.AddWithValue("@Salary", emp.Salary);
        }
    }
}