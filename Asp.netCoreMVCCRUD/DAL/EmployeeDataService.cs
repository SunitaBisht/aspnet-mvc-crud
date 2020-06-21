using Asp.netCoreMVCCRUD.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;


namespace Asp.netCoreMVCCRUD.DAL
{
    public class EmployeeDataService
    {
        private SqlConnection xSqlConnection;
        private SqlCommand xSqlCommand;
        private readonly string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=EmployeeDb;Trusted_Connection=True;MultipleActiveResultSets=True";


        public async Task<int> Save(Employee xEmployee)
        {
            if (xEmployee == null)
            {
                throw new ArgumentNullException("Category can not be null");
            }
            using (xSqlConnection = new SqlConnection(connectionString))
            {
                string cmdText = "INSERT INTO Employees(FirstName,LastName,Password,ConfirmPassword,Gender,Email,Phone,SecurityQuestion,Answer) VALUES (@firstname,@lastname,@password,@confirmpassword,@gender,@email,@phone,@securityquestion,@answer)";
                xSqlCommand = new SqlCommand(cmdText, xSqlConnection);
                xSqlCommand.Parameters.AddWithValue("@firstname", xEmployee.FirstName);
                xSqlCommand.Parameters.AddWithValue("@lastname", xEmployee.LastName);
                xSqlCommand.Parameters.AddWithValue("@password", xEmployee.Password);
                xSqlCommand.Parameters.AddWithValue("@confirmpassword", xEmployee.ConfirmPassword);
                xSqlCommand.Parameters.AddWithValue("@gender", xEmployee.Gender);
                xSqlCommand.Parameters.AddWithValue("@email", xEmployee.Email);
                xSqlCommand.Parameters.AddWithValue("@phone", xEmployee.Phone);
                xSqlCommand.Parameters.AddWithValue("@securityquestion", xEmployee.SecurityQuestion);
                xSqlCommand.Parameters.AddWithValue("@answer", xEmployee.Answer);

                await xSqlConnection.OpenAsync();
                int response = await xSqlCommand.ExecuteNonQueryAsync();
                await xSqlConnection.CloseAsync();
                return response;
            }
        }

        public async Task<IEnumerable<Employee>> Display()
        {
            List<Employee> xEmployeeList = new List<Employee>();
            using (xSqlConnection = new SqlConnection(connectionString))
            {
                string cmdText = "SELECT * FROM Employees";
                xSqlCommand = new SqlCommand(cmdText, xSqlConnection);
                await xSqlConnection.OpenAsync();
                SqlDataReader xSqlDataReader = await xSqlCommand.ExecuteReaderAsync();

                while (xSqlDataReader.HasRows && await xSqlDataReader.ReadAsync())
                {
                    var xEmployee = new Employee
                    {
                        EmpId = Convert.ToInt32(xSqlDataReader["EmpId"]),
                        FirstName = xSqlDataReader["FirstName"].ToString(),
                        LastName = xSqlDataReader["LastName"].ToString(),
                        Gender = xSqlDataReader["Gender"].ToString(),
                        Email = xSqlDataReader["Email"].ToString(),
                        Phone = xSqlDataReader["Phone"].ToString(),
                    };
                    xEmployeeList.Add(xEmployee);
                }
                return xEmployeeList;
            }
        }

        public async Task<Employee> DisplayById(int? EmpId)
        {
            Employee xEmployee = null;
            if (EmpId == 0)
            {
                throw new ArgumentNullException($"{nameof(EmpId)} Can't be null.");
            }
            try
            {
                using (xSqlConnection = new SqlConnection(connectionString))
                {
                    string cmdText = "SELECT * FROM Employees WHERE EmpId=@EmpId";
                    xSqlCommand = new SqlCommand(cmdText, xSqlConnection);
                    xSqlCommand.Parameters.AddWithValue("@EmpId", EmpId);
                    await xSqlConnection.OpenAsync();
                    SqlDataReader xSqlDataReader = await xSqlCommand.ExecuteReaderAsync();
                    while (xSqlDataReader.HasRows && await xSqlDataReader.ReadAsync())
                    {
                        xEmployee = new Employee
                        {
                            EmpId = Convert.ToInt32(xSqlDataReader["EmpId"]),
                            FirstName = xSqlDataReader["FirstName"].ToString(),
                            LastName = xSqlDataReader["LastName"].ToString(),
                            Gender = xSqlDataReader["Gender"].ToString(),
                            Email = xSqlDataReader["Email"].ToString(),
                            Phone = xSqlDataReader["Phone"].ToString(),
                        };
                    }
                    return xEmployee;
                }
            }
            catch (Exception xException)
            {
                throw xException;
            }
        }

        public async Task<int> Update(Employee xEmployee)
        {
            if (xEmployee == null)
            {
                throw new ArgumentNullException("No Employee Available");
            }
            using (xSqlConnection = new SqlConnection(connectionString))
            {
                string cmdText = "UPDATE Employees SET FirstName= @firstname,LastName=@lastname,Gender=@gender,Email=@email,Phone=@phone WHERE EmpId =@EmpId";
                xSqlCommand = new SqlCommand(cmdText, xSqlConnection);

                xSqlCommand.Parameters.AddWithValue("@EmpId", xEmployee.EmpId);
                xSqlCommand.Parameters.AddWithValue("@firstname", xEmployee.FirstName);
                xSqlCommand.Parameters.AddWithValue("@lastname", xEmployee.LastName);
                xSqlCommand.Parameters.AddWithValue("@gender", xEmployee.Gender);
                xSqlCommand.Parameters.AddWithValue("@email", xEmployee.Email);
                xSqlCommand.Parameters.AddWithValue("@phone", xEmployee.Phone);

                await xSqlConnection.OpenAsync();
                int isUpdated = await xSqlCommand.ExecuteNonQueryAsync();
                await xSqlConnection.CloseAsync();
                return isUpdated;
            }
        }

        public async Task<int> Remove(int? EmpId)
        {
            if (EmpId == 0)
            {
                throw new ArgumentNullException("No Employee Available");
            }
            using (xSqlConnection = new SqlConnection(connectionString))
            {
                string cmdText = "DELETE FROM Employees WHERE EmpId=@EmpId";
                xSqlCommand = new SqlCommand(cmdText, xSqlConnection);
                xSqlCommand.Parameters.AddWithValue("@EmpId", EmpId);
                await xSqlConnection.OpenAsync();
                var isDeleted = await xSqlCommand.ExecuteNonQueryAsync();
                await xSqlConnection.CloseAsync();
                return isDeleted;

            }
        }
    }
}



