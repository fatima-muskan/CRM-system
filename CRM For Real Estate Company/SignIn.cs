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
    public partial class SignIn : Form
    {
        public SignIn()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form moreForm = new ResetPassword();
            moreForm.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form moreForm = new SignUp();
            moreForm.Show();
        }

        private void SignIn_Load(object sender, EventArgs e)
        {

        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            Form moreForm = new Home();
            moreForm.Show();
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            PersonDL person = new PersonDL();
            string role = person.SignIn(username, password);
            if (role != null)
            {
                int personId = person.SearchPersonId(username, password);
                // Handle successful sign-in, e.g., show a message box with the role
                MessageBox.Show("Role: " + role, "Sign In Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //if role is manager open manager menu by passing person as a parameter
                
                if (personId != -1)
                {
                    if (role == "Owner")
                    {
                        OwnerDL owner = new OwnerDL();
                        int paymentMethodId = owner.SearchPaymentMethodIdByPersonId(personId);
                        // Handle successful sign-in for owner role
                        MessageBox.Show("Role: " + role + ", Payment Method ID: " + paymentMethodId, "Sign In Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        OwnerMenu newfrm = new OwnerMenu(personId, paymentMethodId);
                        newfrm.Show();
                        this.Hide();
                    }
                    if(role == "Lead")
                    {
                        LeadDL lead = new LeadDL();
                        int LeadId = lead.SearchLeadIdByPersonId(personId);
                        if(LeadId == -1)
                        {
                            MessageBox.Show("Lead not found for this person.");
                        }
                        else
                        {
                            MessageBox.Show("Role: " + role + ", Lead ID: " + LeadId, "Sign In Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            BuyerMenu newfrm = new BuyerMenu(LeadId,personId);
                            newfrm.Show();
                            this.Hide();
                        }
                    }
                    if(role=="Employee")
                    {
                        int pid = person.SerachPersonId(username, password);
                        Form moreForm = new EmployeeMenu(pid);
                        moreForm.Show();
                        this.Hide();
                    }
                    if (role == "Admin")
                    {
                        int pid = person.SerachPersonId(username, password);
                        Form moreForm = new AdminMenu(pid);
                        moreForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        // Handle successful sign-in for other roles
                        MessageBox.Show("Role: " + role, "Sign In Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("NO PERSON FOUND");
                }
            }
            else
            {
                // Handle failed sign-in, e.g., show an error message
                MessageBox.Show("Invalid username or password", "Sign In Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void txtUsername_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            txtPassword.Text = "";
        }
    }
}
