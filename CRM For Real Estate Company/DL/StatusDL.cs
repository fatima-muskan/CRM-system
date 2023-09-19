using CRM_For_Real_Estate_Company.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.DL
{
    class StatusDL
    {
        private string connectionString; // Connection string for your database
        List<StatusBL> statusList = new List<StatusBL>();

        public StatusDL()
        {
            // Set your database connection string here
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
        }
        public StatusBL GetStatusById(int id)
        {
            StatusBL status = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT [id], [category], [name] FROM [FinalProject].[dbo].[Status] WHERE id = @id";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        status = new StatusBL();
                        status.Id = (int)reader["id"];
                        status.Category = reader["category"].ToString();
                        status.Name = reader["name"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return status;
        }

        // Get all sales types by category
        public List<StatusBL> GetAllStatusByCategory(string category)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Status WHERE category = @Category", connection);
                command.Parameters.AddWithValue("@Category", category);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    StatusBL status = new StatusBL();
                    status.Id = Convert.ToInt32(reader["Id"]);
                    status.Category = Convert.ToString(reader["Category"]);
                    status.Name = Convert.ToString(reader["Name"]);
                    statusList.Add(status);
                }
                reader.Close();
            }

            return statusList;
        }
    }
}
