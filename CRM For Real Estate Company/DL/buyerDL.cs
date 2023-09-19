using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.DL
{
    class buyerDL
    {
        private List<buyerBL> buyersList = new List<buyerBL>(); // List of buyers
        private string connectionString;
        public buyerDL()
        {
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
        }
        // Method to insert buyer into SQL database
        public bool InsertBuyer(buyerBL buyer, out string errorMessage)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_InsertBuyer", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@LeadID", buyer.LeadID);
                    command.Parameters.AddWithValue("@PropertyID", buyer.PropertyID);
                    command.ExecuteNonQuery();

                    // Add buyer to local buyers list
                    buyersList.Add(buyer);

                    errorMessage = null; // No error
                    return true; // Insertion successful
                }
                catch (Exception ex)
                {
                    // Handle the exception as needed, such as logging or displaying an error message
                    errorMessage = "Failed to insert buyer into the database: " + ex.Message;
                    return false; // Insertion failed
                }
            }
        }

        // Method to get list of all buyers from the database
        public List<buyerBL> GetBuyers()
        {
            // Logic to fetch buyers from SQL database and populate the local buyers list
            return buyersList;
        }
        public List<buyerBL> SearchBuyers(string searchCriteria)
        {
            List<buyerBL> searchResults = new List<buyerBL>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_SearchBuyers", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SearchCriteria", searchCriteria);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        buyerBL buyer = new buyerBL();
                        buyer.LeadID = Convert.ToInt32(reader["LeadID"]);
                        buyer.PropertyID = Convert.ToInt32(reader["PropertyID"]);
                        buyer.ID = Convert.ToInt32(reader["ID"]);
                        // Set other properties of the BuyerBL object as needed
                        searchResults.Add(buyer);
                    }
                    reader.Close();
                }
            }

            return searchResults;
        }
        public void LoadBuyers()
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_LoadBuyers", connection);
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        buyerBL buyer = new buyerBL();
                        buyer.LeadID = Convert.ToInt32(reader["LeadID"]);
                        buyer.PropertyID = Convert.ToInt32(reader["PropertyID"]);
                        buyer.ID = Convert.ToInt32(reader["ID"]);
                        // Set other properties of the BuyerBL object as needed
                        buyersList.Add(buyer);
                    }
                    reader.Close();
                }
            }

        }
        public int InsertBuyer(int personId, int propertyId)
        {
            int buyerId = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Set up the SQL query with parameters for person ID and property ID
                    string query = "INSERT INTO Buyer (personid, propertyid) VALUES (@PersonId, @PropertyId); SELECT CAST(scope_identity() AS int)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Set the values for the parameters
                        command.Parameters.AddWithValue("@PersonId", personId);
                        command.Parameters.AddWithValue("@PropertyId", propertyId);

                        // Open the database connection
                        connection.Open();

                        // Execute the query and get the ID of the inserted row
                        buyerId = (int)command.ExecuteScalar();
                    }
                }
                

                return buyerId;
            }
            catch
            {
                return 0;
            }
        }
        public List<int> GetBuyerIdsByPersonId(int personId)
        {
            List<int> buyerIds = new List<int>();

            // Query to retrieve the buyer IDs using the provided person ID
            string query = "SELECT [id] FROM [FinalProject].[dbo].[Buyer] WHERE [personid] = @PersonId";

            // Create a new SqlConnection object to connect to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create a new SqlCommand object to execute the query
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Add the Person ID parameter to the query
                    command.Parameters.AddWithValue("@PersonId", personId);

                    // Open the database connection
                    connection.Open();

                    // Execute the query and retrieve the buyer IDs
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        int buyerId = (int)reader["id"];
                        buyerIds.Add(buyerId);
                    }
                }
            }
            return buyerIds;
        }

    }
}
