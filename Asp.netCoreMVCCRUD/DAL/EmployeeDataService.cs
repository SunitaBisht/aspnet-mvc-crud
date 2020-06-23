using Asp.netCoreMVCCRUD.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netCoreMVCCRUD.DAL
{
    public interface IEmployeeDataService
    {
        /// <summary>
        /// Creats a Employee into Database.
        /// </summary>
        /// <param name="xEmployee">Object of Employee containing all info.</param>
        /// <returns>No of row affected.</returns>
        Task<int> Save(Employee xEmployee);
        Task<IEnumerable<Employee>> Display();
        Task<Employee> DisplayById(int? EmpId);
        Task<int> Update(Employee xEmployee);
        Task<int> Remove(int? EmpId);
    }

    public class EmployeeDataService : IEmployeeDataService
    {
        private SqlConnection xSqlConnection;
        private SqlCommand xSqlCommand;
        private readonly string connectionString = "Server=(LocalDb)\\MSSQLLocalDB;Database=EmployeeDb;Trusted_Connection=True;MultipleActiveResultSets=True";

        /// <summary>
        /// Creats a Employee into Database.
        /// </summary>
        /// <param name="xEmployee">Object of Employee containing all info.</param>
        /// <returns>No of row affected.</returns>
        public async Task<int> Save(Employee xEmployee)
        {
            if (xEmployee == null)
            {
                throw new ArgumentNullException("Category can not be null");
            }

            using (xSqlConnection = new SqlConnection(connectionString))
            {
                string cmdText = "spInsertEmployee";
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

                xSqlCommand.CommandType = CommandType.StoredProcedure;

                await xSqlConnection.OpenAsync();
                int response = await xSqlCommand.ExecuteNonQueryAsync();
                await xSqlConnection.CloseAsync();
                return response;
            }
        }

        /// <summary>
        /// Display a Employee in index page.
        /// </summary>
        /// <returns>List of the employees</returns>
        public async Task<IEnumerable<Employee>> Display()
        {
            List<Employee> xEmployeeList = new List<Employee>();
            using (xSqlConnection = new SqlConnection(connectionString))
            {
                string cmdText = "spGetEmployees";
                xSqlCommand = new SqlCommand(cmdText, xSqlConnection);
                xSqlCommand.CommandType = CommandType.StoredProcedure;

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

        /// <summary>
        /// Get Employee Through Employee Id
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns>Employee Details</returns>
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
                    string cmdText = "spGetEmployeeByEmpId";
                    xSqlCommand = new SqlCommand(cmdText, xSqlConnection);
                    xSqlCommand.Parameters.AddWithValue("@EmpId", EmpId);
                    xSqlCommand.CommandType = CommandType.StoredProcedure;

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
        /// <summary>
        /// Update Employee details into database
        /// </summary>
        /// <param name="xEmployee"></param>
        /// <returns>no. of affected rows updated</returns>

        public async Task<int> Update(Employee xEmployee)
        {
            if (xEmployee == null)
            {
                throw new ArgumentNullException("No Employee Available");
            }
            using (xSqlConnection = new SqlConnection(connectionString))
            {
                string cmdText = "spUpdateEmployeeById";
                xSqlCommand = new SqlCommand(cmdText, xSqlConnection);

                xSqlCommand.Parameters.AddWithValue("@EmpId", xEmployee.EmpId);
                xSqlCommand.Parameters.AddWithValue("@firstname", xEmployee.FirstName);
                xSqlCommand.Parameters.AddWithValue("@lastname", xEmployee.LastName);
                xSqlCommand.Parameters.AddWithValue("@gender", xEmployee.Gender);
                xSqlCommand.Parameters.AddWithValue("@email", xEmployee.Email);
                xSqlCommand.Parameters.AddWithValue("@phone", xEmployee.Phone);

                xSqlCommand.CommandType = CommandType.StoredProcedure;

                await xSqlConnection.OpenAsync();
                int isUpdated = await xSqlCommand.ExecuteNonQueryAsync();
                await xSqlConnection.CloseAsync();
                return isUpdated;
            }
        }

        /// <summary>
        /// Remove Employee Details selected through EmpId
        /// </summary>
        /// <param name="EmpId"></param>
        /// <returns>No. of affected row deleted</returns>
        public async Task<int> Remove(int? EmpId)
        {
            if (EmpId == 0)
            {
                throw new ArgumentNullException("No Employee Available");
            }
            using (xSqlConnection = new SqlConnection(connectionString))
            {
                string cmdText = "spDeleteEmployeeByEmpID";
                xSqlCommand = new SqlCommand(cmdText, xSqlConnection);
                xSqlCommand.Parameters.AddWithValue("@EmpId", EmpId);
                xSqlCommand.CommandType = CommandType.StoredProcedure;


                await xSqlConnection.OpenAsync();
                var isDeleted = await xSqlCommand.ExecuteNonQueryAsync();
                await xSqlConnection.CloseAsync();
                return isDeleted;

            }
        }
    }
}



