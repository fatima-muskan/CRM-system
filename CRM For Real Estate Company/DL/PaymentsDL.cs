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
    class PaymentsDL
    {
        private string connectionString;
        private List<PaymentsMethodBL> paymentsList = new List<PaymentsMethodBL>();

        // Constructor
        public PaymentsDL()
        {
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
        }

        // Method to insert a payment
        public bool InsertPayment(int buyerId, int paymentId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("INSERT INTO Payment (BuyerId, PaymentMethodId) VALUES (@buyerId, @paymentMethodId)", connection))
                    {
                        command.Parameters.AddWithValue("@buyerId", buyerId);
                        command.Parameters.AddWithValue("@paymentMethodId", paymentId);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    connection.Close();
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

