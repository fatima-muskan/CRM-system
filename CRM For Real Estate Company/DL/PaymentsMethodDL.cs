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
    class PaymentsMethodDL
    {
        private string connectionString;
        private List<PaymentsMethodBL> paymentsList = new List<PaymentsMethodBL>();

        public PaymentsMethodDL()
        {
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            LoadPayments();
        }

        private void LoadPayments()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_LoadPayments", connection);
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PaymentsMethodBL payment = new PaymentsMethodBL();
                        payment.PaymentID = Convert.ToInt32(reader["ID"]);
                        payment.BankName = reader["bank"].ToString();

                        string accountNoStr = reader["accountNo"].ToString();
                        payment.AccountNo = accountNoStr.PadRight(16).Substring(0, 16).ToCharArray();

                        payment.PaymentMethod = reader["method"].ToString();
                        // Set other properties of the PaymentBL object as needed
                        paymentsList.Add(payment);
                    }
                    reader.Close();
                }
            }
        }
        public PaymentsMethodBL GetPaymentsMethodById(int id)
        {
            PaymentsMethodBL PaymentsMethod = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM PaymentMethod WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    PaymentsMethod = new PaymentsMethodBL();
                    if (reader.Read())
                    {
                        //{
                            PaymentsMethod.PaymentID = Convert.ToInt32(reader["ID"]);
                            PaymentsMethod.BankName = reader["bank"].ToString();

                            string accountNoStr = reader["accountNo"].ToString();
                            PaymentsMethod.AccountNo = accountNoStr.PadRight(16).Substring(0, 16).ToCharArray();

                            PaymentsMethod.PaymentMethod = reader["method"].ToString();
                            // Set other properties of the PaymentBL object as needed
                        //}
                        reader.Close();
                    }
                }
            }

            return PaymentsMethod;
        }
        public bool UpdatePayment(int id, string bank, string accountNoStr, string method)
        {
            bool falg = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "UPDATE PaymentMethod " +
                               "SET bank = @bank, accountNo = @accountNo, method = @method " +
                               "WHERE id = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@bank", bank);
                char[] accountNo=accountNoStr.PadRight(16).Substring(0, 16).ToCharArray();
                command.Parameters.AddWithValue("@accountNo", accountNo);
                command.Parameters.AddWithValue("@method", method);

                command.ExecuteNonQuery();
                connection.Close();
                LoadPayments();
                falg = true;
            }
            return falg;
        }
        public Tuple<int, string> InsertPaymentMethod(PaymentsMethodBL paymentMethod)
        {
            int paymentMethodId = 0;
            string errorMessage = "";

            //try
            //{
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("InsertPaymentMethod", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        
                        // Add the parameters
                        command.Parameters.AddWithValue("@Bank", paymentMethod.BankName);
                        command.Parameters.AddWithValue("@AccountNo", paymentMethod.AccountNo);
                        command.Parameters.AddWithValue("@Method", paymentMethod.PaymentMethod);

                        // Add the output parameter
                        SqlParameter outputParameter = new SqlParameter("@PaymentMethodId", System.Data.SqlDbType.Int);
                        outputParameter.Direction = System.Data.ParameterDirection.Output;
                        command.Parameters.Add(outputParameter);

                        // Execute the command
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        paymentMethodId = (int)outputParameter.Value;
                        LoadPayments();
                    }
                connection.Close();
                }
            //}
            /*catch (Exception ex)
            {
                errorMessage = ex.Message;
            }*/

            return new Tuple<int, string>(paymentMethodId, errorMessage);
        }

        public bool UpdatePayment(PaymentsMethodBL payment, out string errorMessage)
        {
            errorMessage = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_UpdatePayment", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", payment.PaymentID);
                    command.Parameters.AddWithValue("@BankName", payment.BankName);

                    string accountNoStr = payment.AccountNo.ToString();
                    char[] accountNo = accountNoStr.PadRight(16).Substring(0, 16).ToCharArray();
                    command.Parameters.AddWithValue("@AccountNo", accountNo);

                    command.Parameters.AddWithValue("@Method", payment.PaymentMethod);
                    command.ExecuteNonQuery();

                    // Update the paymentsList with the updated payment
                    LoadPayments();

                    return true;
                }
                catch (SqlException ex)
                {
                    // Log or handle the exception as needed
                    errorMessage = "Error updating payment: " + ex.Message;
                    return false;
                }
            }
        }

    }

}