using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_For_Real_Estate_Company
{
    public partial class ViewEmployeeTask : Form
    {
        private int pid;
        public ViewEmployeeTask(int pid)
        {
            this.pid = pid;
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ViewEmployeeTask_Load(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd1 = new SqlCommand("select\r\nt.title,t.description,t.duedate\r\nfrom Task as t join Employee as e on t.assigneeId=e.id\r\nwhere e.personid="+pid+"", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }
    }
}
