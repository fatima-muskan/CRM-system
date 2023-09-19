using CRM_For_Real_Estate_Company.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.DL
{
    class TypeDL
    {
        private string connectionString; // Connection string for your database
        private List<TypeBL> typeList = new List<TypeBL>();


        public TypeDL()
        {
            // Set your database connection string here
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
        }
        public TypeBL GetTypeById(int id)
        {
            TypeBL type = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT [id], [category], [name] FROM [FinalProject].[dbo].[Type] WHERE id = @id";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        type = new TypeBL();
                        type.Id = (int)reader["id"];
                        type.Category = reader["category"].ToString();
                        type.Name = reader["name"].ToString();
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return type;
        }

        // Get all types by category
        public List<TypeBL> GetAllTypesByCategory(string category)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Type WHERE category = @Category", connection);
                command.Parameters.AddWithValue("@Category", category);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TypeBL type = new TypeBL();
                    type.Id = Convert.ToInt32(reader["Id"]);
                    type.Category = Convert.ToString(reader["Category"]);
                    type.Name = Convert.ToString(reader["Name"]);
                    typeList.Add(type);
                }
                reader.Close();
            }

            return typeList;
        }
    }
}
