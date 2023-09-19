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
    public partial class Lead2 : Form
    {
        private string connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";

        public Lead2()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Lead2_Load(object sender, EventArgs e)
        {
            LoadLead();
        }

        private void LoadLead()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "Select l.id as LeadId,per.username as Name, per.email as Email,per.mobileNumber as PhoneNumber " +
                ", s.name as SourceName,l.dateAdded,l.dateConnected from Lead as l " +
                "INNER JOIN(Select id, username, email, mobileNumber from Person) as per ON per.id = personid " +
                "INNER JOIN(Select id, name from Source) as s ON s.id = l.sourceid";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            LoadLead();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text) == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "Select l.id as LeadId,per.username as Name, per.email as Email,per.mobileNumber as PhoneNumber " +
                    ", s.name as SourceName,l.dateAdded,l.dateConnected from Lead as l " +
                    "INNER JOIN(Select id, username, email, mobileNumber from Person WITH(INDEX(idx_Person_Username))) as per ON per.id = personid " +
                    "INNER JOIN(Select id, name from Source) as s ON s.id = l.sourceid " +
                    "where per.username LIKE @name + '%'";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", txtSearch.Text);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                    MessageBox.Show("Searched Successfully!!");
                }
            }

            else
            {
                MessageBox.Show("Enter Lead Name to Search.");
            }
        }
    }
}
