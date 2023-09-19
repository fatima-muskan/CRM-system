using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace CRM_For_Real_Estate_Company
{
    public partial class ViewPaymentAdmin : Form
    {
        private string connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";

        public ViewPaymentAdmin()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ViewPaymentAdmin_Load(object sender, EventArgs e)
        {
            LoadPayment();
        }

        private void LoadPayment()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "Select p.id as PaymentId,per.username as BuyerName, per.email as Email,per.mobileNumber as BuyerNumber, " +
                "pm.accountNo as AccountNo,pm.bank as BankName,pm.method as PaymentMthod from Payment as p " +
                "INNER JOIN(Select id, personid from Buyer) as b ON b.id = p.BuyerId " +
                "INNER JOIN(Select id, username, email, mobileNumber from Person) as per ON per.id = b.personid " +
                "INNER JOIN(Select id, bank, accountNo, method from PaymentMethod) as pm ON pm.id = p.PaymentMethodId";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvPayment.DataSource = dt;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control c in tableLayoutPanel1.Controls)
                {
                    if (c is TextBox)
                        ((TextBox)c).Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            LoadPayment();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text) == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "Select p.id as PaymentId,per.username as BuyerName, per.email as Email,per.mobileNumber as BuyerNumber, " +
                    "pm.accountNo as AccountNo,pm.bank as BankName,pm.method as PaymentMthod from Payment as p " +
                    "INNER JOIN(Select id, personid from Buyer) as b ON b.id = p.BuyerId " +
                    "INNER JOIN(Select id, username, email, mobileNumber from Person) as per ON per.id = b.personid " +
                    "INNER JOIN(Select id, bank, accountNo, method from PaymentMethod WITH(INDEX(idx_Payment_method))) as pm ON pm.id = p.PaymentMethodId " +
                    "where pm.method LIKE @name + '%'";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", txtSearch.Text);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvPayment.DataSource = dt;
                    MessageBox.Show("Searched Successfully!!");
                }
            }

            else
            {
                MessageBox.Show("Enter Payment Method to Search.");
            }
        }
    }
}
