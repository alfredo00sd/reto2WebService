using reto2Propietaria.DTOs;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace reto2Propietaria
{
    public class EmployeeDao
    {
        SqlDataReader reader;
        readonly SqlCommand Cmd = new SqlCommand();
        readonly DBCon Connection = new DBCon();


        //Employee CRUD

        //Create Employees
        public string Add(EmployeeDTO employee) {
            
            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "insert into employee values(@NomId, @Cedula, @Name, @Department, @WorkPosition, @Salary, getDate(), null, 1)";
            Cmd.CommandType = CommandType.Text;
            FillEmployeeParams(Cmd, employee);

            int count = Cmd.ExecuteNonQuery();


            if (count > 0)
            {

                return "Empleado, " + employee.Name + " agregado!";
            }
            else {

                return "Error tratando de insertar...";
            }


        }

        //Read all employees
        public List<Employee> GetEmployees()
        {

            List<Employee> dtoList;

            Cmd.Connection = Connection.Open();
            Cmd.CommandText = "select * from employees";
            Cmd.CommandType = CommandType.Text;
            reader = Cmd.ExecuteReader();

            dtoList = FillEmployeeList(reader);

            Connection.Close();
            reader.Close();

            return dtoList;
        }

        public void Edit() { 
        
        }
        public void Delete() { }

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
                    Salary = reader.GetDouble(6),
                    FirstDay = reader.GetDateTime(7),
                    LastDay = reader.GetDateTime(8),
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