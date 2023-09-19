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
    class OwnerDL
    {
        private string connectionString;
        private List<OwnerBL> ownerslst= new List<OwnerBL>();
        public OwnerDL()
        {
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            LoadOwners();
        }
        public List<OwnerBL> GetOwners()
        {
            return ownerslst;
        }
        public int SearchPaymentMethodIdByPersonId(int personId)
        {
            foreach (OwnerBL owner in ownerslst)
            {
                if (owner.PersonId == personId)
                {
                    return owner.PaymentId;
                }
            }
            // If personId not found in ownersList, return -1 to indicate failure
            return -1;
        }

        public bool InsertOwner(int PropertyID, int PersonId, int PaymentMethodID, out string errorMessage)
        {
            errorMessage = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_InsertOwner", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PropertyID", PropertyID);
                    command.Parameters.AddWithValue("@PersonID", PersonId);
                    command.Parameters.AddWithValue("@PaymentMethodID", PaymentMethodID);
                    command.ExecuteNonQuery();
                    LoadOwners();
                    Console.WriteLine("Owner inserted successfully");
                    return true;
                }
                catch (SqlException ex)
                {
                    // Log or handle the exception as needed
                    errorMessage = "Error inserting owner: " + ex.Message;
                    Console.WriteLine(errorMessage);
                    return false;
                }
            }
        }

        public List<OwnerBL> SearchOwners(string searchCriteria, out string errorMessage)
        {
            errorMessage = null;
            List<OwnerBL> searchResults = new List<OwnerBL>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_SearchOwners", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SearchCriteria", searchCriteria);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            OwnerBL owner = new OwnerBL();
                            owner.PropertyID = Convert.ToInt32(reader["PropertyID"]);
                            owner.ID = Convert.ToInt32(reader["ID"]);
                            owner.PaymentId = Convert.ToInt32(reader["PaymentMethodID"]);
                            // Set other properties of the OwnerBL object as needed
                            searchResults.Add(owner);
                        }
                        reader.Close();
                    }

                }
                catch (SqlException ex)
                {
                    // Log or handle the exception as needed
                    errorMessage = "Error searching owners: " + ex.Message;
                }
            }

            return searchResults;
        }

        public void LoadOwners()
        {
            ownerslst.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_LoadOwners", connection);
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        OwnerBL owner = new OwnerBL();
                        owner.ID = Convert.ToInt32(reader["OwnerID"]);
                        owner.PropertyID = Convert.ToInt32(reader["PropertyID"]);
                        owner.PersonId= Convert.ToInt32(reader["PersonID"]);
                        owner.PaymentId = Convert.ToInt32(reader["PaymentMethodID"]);
                        // Set other properties of the OwnerBL object as needed
                        ownerslst.Add(owner);
                    }
                    reader.Close();
                }
            }
        }


    }
}


