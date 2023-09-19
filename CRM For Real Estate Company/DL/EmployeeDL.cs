using CRM_For_Real_Estate_Company.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.DL
{
    class EmployeeDL
    {
        private string connectionString;
        private List<EmployeeBL> employeeslst = new List<EmployeeBL>();

        public EmployeeDL()
        {
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            LoadEmployeesData();
        }

        public List<EmployeeBL> GetAllEmployees()
        {
            return employeeslst;
        }
        public bool InsertEmployee(int personID, out string errorMessage)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_InsertEmployee", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", personID);
                    command.ExecuteNonQuery();

                    // Update employees list with the inserted employee
                    LoadEmployeesData();
                    errorMessage = null; // No error
                    return true; // Insertion successful
                }
                catch (SqlException ex)
                {
                    // Log or handle the exception as needed
                    errorMessage=("Error inserting employee: " + ex.Message);
                    return false;
                }
            }
        }

        public bool UpdateEmployee(int personID, out string errorMessage)
        {
            EmployeeBL employeeToUpdate = employeeslst.FirstOrDefault(e => e.PersonID == personID);
            if (employeeToUpdate != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_UpdateEmployee", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", personID);
                        command.ExecuteNonQuery();

                        // Update employees list with the updated employee
                        LoadEmployeesData();
                        errorMessage = null;
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        // Log or handle the exception as needed
                        errorMessage=("Error updating employee: " + ex.Message);
                        return false; 
                    }
                }
            }
            errorMessage = "Data not found";
            return false;
        }

        public bool DeleteEmployee(int id, out string errorMessage)
        {
            EmployeeBL employeeToDelete = employeeslst.FirstOrDefault(e => e.ID == id);
            if (employeeToDelete != null)
            {
                employeeslst.Remove(employeeToDelete);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_DeleteEmployee", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", id);
                        command.ExecuteNonQuery();

                        // Update employees list with the deleted employee
                        LoadEmployeesData();
                        errorMessage = null; // No error
                        return true; // Insertion successful
                    }
                    catch (SqlException ex)
                    {
                        // Log or handle the exception as needed
                        errorMessage=("Error deleting employee: " + ex.Message);
                        return false;
                    }
                }
            }
            errorMessage = "Data not found";
            return false;
        }
        private void LoadEmployeesData()
        {
            employeeslst.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetAllEmployees", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    EmployeeBL employee = new EmployeeBL();
                    employee.ID = Convert.ToInt32(reader["ID"]);
                    employee.PersonID = Convert.ToInt32(reader["PersonID"]);
                    employee.Salary = Convert.ToInt32(reader["salary"]);
                    employeeslst.Add(employee);
                }
                reader.Close();
            }
        }
        
    }
}
