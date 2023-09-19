using CRM_For_Real_Estate_Company.Report;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_For_Real_Estate_Company
{
    public partial class EReport : Form
    {
        private int pid;
        public EReport(int pid)
        {
            this.pid = pid;
            InitializeComponent();
        }

        private void EReport_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (comboBox1.Text == "Salary Report")
            {
                EmployeeSalary r = new EmployeeSalary();
                r.SetParameterValue("@Person_ID", pid);
                v1.ReportSource = null;
                v1.ReportSource = r;
            }
            if (comboBox1.Text == "Sales Report")
            {
                EmployeeSales r = new EmployeeSales();
                r.SetParameterValue("@Person_ID", pid);
                v1.ReportSource = null;
                v1.ReportSource = r;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
