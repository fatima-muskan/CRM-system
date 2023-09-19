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
    public partial class ViewEmployeeProfile : Form
    {
        private int pid;
        public ViewEmployeeProfile(int pid)
        {
            InitializeComponent();
            this.pid = pid;
        }

        private void ViewEmployeeProfile_Load(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd1 = new SqlCommand("select p.username,p.email,p.password,(select a.streetName+' '+a.state+' '+a.city+' '+a.country from Address as a where a.id=p.addressid) as ad,e.salary\r\nfrom Person as p join Employee as  e\r\non p.id=e.personid\r\nwhere e.personid="+pid+"", con);
            SqlDataReader reader = cmd1.ExecuteReader();
            while (reader.Read())
            {
               textBox1.Text= reader["username"].ToString();
                textBox2.Text = reader["email"].ToString();
                textBox3.Text = reader["password"].ToString();
                richTextBox1.Text = reader["ad"].ToString();
                textBox4.Text = reader["salary"].ToString();
            }
            reader.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
