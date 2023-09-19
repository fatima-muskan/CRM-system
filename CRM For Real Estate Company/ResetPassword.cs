using CRM_For_Real_Estate_Company.BL;
using CRM_For_Real_Estate_Company.DL;
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
    public partial class ResetPassword : Form
    {
        public ResetPassword()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form moreForm = new SignIn();
            moreForm.Show();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string newPassword = txtPassword.Text;
            if (txtConfirmPassword.Text == txtPassword.Text)
            {
                // Call the UpdatePassword method in the business logic layer (BL)
                PersonDL person = new PersonDL();
                bool result = person.UpdatePassword(username, newPassword);

                if (result)
                {
                    MessageBox.Show("Password updated successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to update password. Invalid username.");
                }
            }
            else
            {
                MessageBox.Show("Failed to update.Password doesnt match");
            }
        }
    }
}
