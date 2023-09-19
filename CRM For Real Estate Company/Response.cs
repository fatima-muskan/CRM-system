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
    public partial class Response : Form
    {
        int leadId;
        int personid;
        public Response(int leadId, int personid)
        {
            InitializeComponent();
            this.leadId = leadId;
            this.personid = personid;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            BuyerMenu newfrm = new BuyerMenu(leadId,personid);
            newfrm.Show();
            this.Close();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            ResponseDL response = new ResponseDL();
            int responseid = response.InsertResponse(txtTitle.Text, txtComment.Text);
            if(responseid != 0 )
            {
                SqlConnection connection = new SqlConnection(@"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True");
                SqlCommand command = new SqlCommand("INSERT INTO LeadResponse (leadid, responseid) VALUES (@leadid, @responseid)", connection);
                command.Parameters.AddWithValue("@leadid", leadId);
                command.Parameters.AddWithValue("@responseid", responseid);
                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Lead response added successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Error inserting lead response into the database.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error inserting lead response into the database: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

            }
            else
            {
                MessageBox.Show("check if the textboxes are null or any wrong data type ");
            }
        }
    }
}
