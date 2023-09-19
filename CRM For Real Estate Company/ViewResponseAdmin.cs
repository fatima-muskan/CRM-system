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
    public partial class ViewResponseAdmin : Form
    {
        private string connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";

        public ViewResponseAdmin()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ViewResponseAdmin_Load(object sender, EventArgs e)
        {
            LoadResponse();
        }

        private void LoadResponse()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "Select * from Response";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvResponse.DataSource = dt;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text) == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "Select * from Response where name LIKE @name + '%'";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", txtSearch.Text);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvResponse.DataSource = dt;
                    MessageBox.Show("Searched Successfully!!");
                }
            }

            else
            {
                MessageBox.Show("Enter Response Name to Search.");
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

            LoadResponse();
        }
    }
}
