using CRM_For_Real_Estate_Company.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.DL
{
    class InterestedInDL
    {
        private string connectionString;

        public InterestedInDL()
        {
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
        }
        public bool AddInterestedProperty(int leadId, int propertyId)
        {
            bool success = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if the property is already in the Interested table
                    bool isInterested = new InterestedInDL().CheckIfPropertyIsInterestedByLeadIdAndPropertyId(leadId, propertyId);

                    if (!isInterested)
                    {
                        // Add the property to the Interested table
                        string query = "INSERT INTO LeadInterestedIn (leadid, propertyid) VALUES (@leadid, @propertyid)";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@leadid", leadId);
                        command.Parameters.AddWithValue("@propertyid", propertyId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            success = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return success;
        }
        public bool RemoveInterestedProperty(int leadId, int propertyId)
        {
            bool success = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "DELETE FROM LeadInterestedIn WHERE leadid = @leadid AND propertyid = @propertyid";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@leadid", leadId);
                    command.Parameters.AddWithValue("@propertyid", propertyId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return success;
        }

        public bool CheckIfPropertyIsInterestedByLeadIdAndPropertyId(int leadId, int propertyId)
        {
            bool isInterested = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT COUNT(*) FROM LeadInterestedIn WHERE leadid = @leadid AND propertyid = @propertyid";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@leadid", leadId);
                    command.Parameters.AddWithValue("@propertyid", propertyId);

                    int count = (int)command.ExecuteScalar();
                    isInterested = count > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return isInterested;
        }

    }
}
