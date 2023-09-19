using CRM_For_Real_Estate_Company.DL;
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
    public partial class ViewLeadSales : Form
    {
        int personid;
        public ViewLeadSales(int personid)
        {
            InitializeComponent();
            this.personid = personid;
        }

        private void ViewLeadSales_Load(object sender, EventArgs e)
        {
            buyerDL buyer = new buyerDL();
            List<int> buyerIds = buyer.GetBuyerIdsByPersonId(personid);

            if (buyerIds.Count == 0)
            {
                MessageBox.Show("You are not a buyer yet.");
                this.Close();
            }
            else
            {
                // Create a SQL connection and command
                string connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
                // Create a new SqlConnection object to connect to the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Create a new SqlCommand object to execute the query
                    using (SqlCommand command = new SqlCommand("SELECT Id, SellerId, PropertyId, Price, SaleDate FROM Sales WITH (INDEX(idx_Sales_BuyerId)) WHERE buyerId IN (" + string.Join(",", buyerIds) + ")", connection))
                    {
                        // Create a data adapter and fill a DataTable with the query results
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        // Bind the DataTable to the DataGridView
                        dataGridView1.DataSource = dt;
                    }
                }


            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
