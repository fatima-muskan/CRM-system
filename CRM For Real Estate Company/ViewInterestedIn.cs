using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_For_Real_Estate_Company
{
    public partial class ViewInterestedIn : Form
    {
        int leadid;
        public ViewInterestedIn(int leadid)
        {
            InitializeComponent();
            this.leadid = leadid;
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ViewInterestedIn_Load(object sender, EventArgs e)
        {
            // Create a SQL connection and command
            string connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT [id], [leadid], [propertyid] FROM [FinalProject].[dbo].[LeadInterestedIn] WITH (INDEX(idx_LeadInterestedIn_LeadId)) WHERE [leadid] = @LeadId", conn))
            {
                // Add the Lead ID parameter to the command
                cmd.Parameters.AddWithValue("@LeadId", leadid);

                // Create a data adapter and fill a DataTable with the query results
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Bind the DataTable to the DataGridView
                dataGridView1.DataSource = dt;
            }

        }
    }
}
