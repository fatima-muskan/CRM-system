using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.DL
{
    class SalesDL
    {
        private string connectionString;

        public SalesDL()
        {
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
        }

        public bool InsertSale(int sellerId, int propertyId, int buyerId, decimal price, DateTime saleDate)
        {
            bool success = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "INSERT INTO Sales (sellerid, propertyid, buyerid, price, saleDate) " +
                                   "VALUES (@sellerId, @propertyId, @buyerId, @price, @saleDate); ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@sellerId", sellerId);
                        command.Parameters.AddWithValue("@propertyId", propertyId);
                        command.Parameters.AddWithValue("@buyerId", buyerId);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@saleDate", saleDate);

                        return true;
                    }
                    connection.Close();
                }
                
            }
            catch (Exception ex)
            {
                // log the error
                return false;
                Console.WriteLine("Error inserting sale: " + ex.Message);
            }

            return success;
        }
    }
}
