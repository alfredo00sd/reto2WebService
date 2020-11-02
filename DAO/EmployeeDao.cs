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

        //Create Employees
        public string Add(EmployeeDTO employee)
        {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "insert into employee values(@NomId, @Name, @Cedula, @Department, @WorkPosition, @Salary, convert(date, getDate()), 'N/A', 1)";
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
            Cmd.CommandText = "select * from employee where EmpState = 1";
            Cmd.CommandType = CommandType.Text;
            reader = Cmd.ExecuteReader();

            dtoList = FillEmployeeList(reader);

            Connection.Close();
            reader.Close();

            return dtoList;
        }

        //Get by Name, cedula, department
        public Employee GetEmployeeBy(string argument)
        {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "select * from employee where name like @argument or cedula like @argument or department like @argument";
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
        public string Edit(EmployeeDTO dTO)
        {

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "update employee set ";
            Cmd.CommandType = CommandType.Text;

            FillEmployeeParams(Cmd, dTO);

            Cmd.Parameters.Clear();
            Connection.Close();

            if (Cmd.ExecuteNonQuery() > 0)
            {
                return "Empleado ingresado";
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
            Cmd.CommandText = "delete from employee where id = @id ";
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