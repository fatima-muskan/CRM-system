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
    public partial class AdminReport : Form
    {
        public AdminReport()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Salary Report")
            {
                comboBox2.Enabled = false;
                Report1 r = new Report1();
                v1.ReportSource = null;
                v1.ReportSource = r;
            }
            if (comboBox1.Text == "Remaining property Report")
            {
                comboBox2.Enabled = false;
                RemainProperty r = new RemainProperty();

                v1.ReportSource = null;
                v1.ReportSource = r;
            }
            if (comboBox1.Text == "All Property Report")
            {
                comboBox2.Enabled = false;
                AllPropertyReport r = new AllPropertyReport();

                v1.ReportSource = null;
                v1.ReportSource = r;
            }
            if (comboBox1.Text == "Commission Report")
            {
                comboBox2.Enabled = true;
                CommissionReport r = new CommissionReport();
                r.SetParameterValue("@m", comboBox2.Text);
                v1.ReportSource = null;
                v1.ReportSource = r;
            }
            if (comboBox1.Text == "Company revenue Report")
            {
                comboBox2.Enabled = true;
                revenue r = new revenue();
                r.SetParameterValue("@m", comboBox2.Text);
                v1.ReportSource = null;
                v1.ReportSource = r;
            }
            if (comboBox1.Text == "Sales Report")
            {

                comboBox2.Enabled = true;
                Sale r = new Sale();
                r.SetParameterValue("@m", comboBox2.Text);
                v1.ReportSource = null;
                v1.ReportSource = r;
            }
        }

        private void AdminReport_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
