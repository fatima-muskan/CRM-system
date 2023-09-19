using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_For_Real_Estate_Company
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Form moreform = new SignIn();
            moreform.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form moreform = new SignUp();
            moreform.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            DialogResult iExit;
            try
            {
                iExit = MessageBox.Show("Confirm if you want to exit!!", "CRM for Real Estate Company",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (iExit == DialogResult.Yes)
                {
                    Application.Exit();
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
