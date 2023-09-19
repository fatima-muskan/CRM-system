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
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            Form moreForm = new Home();
            moreForm.Show();
            this.Hide();
        }

        private void SignUp_Load(object sender, EventArgs e)
        {
            RoleDL roleDL = new RoleDL();
            List<RoleBL> roles = roleDL.LoadRoles();

            // Filter roles based on role name
            List<RoleBL> filteredRoles = roles.FindAll(r => r.Name == "Owner" || r.Name == "Lead");

            // Set data source, display member, and value member for the combo box
            RolecomboBox.DataSource = filteredRoles;
            RolecomboBox.DisplayMember = "Name"; // Display the Name property of RoleBL objects
            RolecomboBox.ValueMember = "Id"; // Use the Id property of RoleBL objects as the value member
            LoadSources();

        }
        private void LoadSources()
        {
            SourceDL sourceDL = new SourceDL();
            List<SourceBL> sources = sourceDL.GetSources();
            cbosource.DataSource = sources;
            cbosource.DisplayMember = "Name";
            cbosource.ValueMember = "Id";
        }
        private void txtUsername_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            txtPassword.Text = "";
        }
        private void RolecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RolecomboBox.SelectedIndex == 0)
            {
                cbosource.Enabled = true;
                cbosource.Visible = true;

            }
            else
            {
                // Disable and hide the combo box
                cbosource.Enabled = false;
                cbosource.Visible = false;
            }
        }
        private void btnSignUp_Click(object sender, EventArgs e)
        {
            PersonBL person = new PersonBL();
            person.Username = txtUsername.Text;
            person.Password = txtPassword.Text;
            person.RoleId = Convert.ToInt32(RolecomboBox.SelectedValue);
            AddressBL address = new AddressBL(txtCountry.Text, txtState.Text, txtStreet.Text, txtCity.Text);
            AddressDL addressid = new AddressDL();
            int addressId;
            string error;
            // Call the InsertAddress function and pass the address object as a parameter
            bool isSuccess = addressid.InsertAddress(address, out addressId, out error);

            // Check the return value and output values
            if (isSuccess)
            {
                Console.WriteLine("Address inserted successfully. Address ID: " + addressId);
                person.AddressId = addressId;
                person.Email = txtEmail.Text;
                person.MobileNumber = txtMobileNumber.Text;
                // Call the InsertPerson method to insert the person into the database
                PersonDL personDL = new PersonDL();
                string errorMessage;
                int PersonId = personDL.InsertPerson(person, out errorMessage);
                if (PersonId != 0)
                {
                    MessageBox.Show("Person inserted successfully!");
                    if (RolecomboBox.SelectedIndex == 1)
                    {
                        AddOwnerProperty ownerPropertyForm = new AddOwnerProperty();
                        DialogResult result = ownerPropertyForm.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            // Retrieve the property ID from the owner property form
                            int propertyId = ownerPropertyForm.PropertyId;
                            int paymentId = ownerPropertyForm.PaymentId;
                            // Use the property ID as needed, e.g. update the sign-up form with the property ID
                            MessageBox.Show("Property ID: " + propertyId);

                            // Call the InsertOwner method to insert the owner into the database
                            OwnerDL ownerDL = new OwnerDL();
                            bool ownerSuccess = ownerDL.InsertOwner(propertyId, PersonId, paymentId, out error);
                            if (ownerSuccess)
                            {
                                MessageBox.Show("Owner inserted successfully!");
                                SignIn newfrm = new SignIn();
                                newfrm.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Failed to insert owner: " + error);
                            }
                        }
                    }
                    else if (RolecomboBox.SelectedIndex == 0)
                    {
                        // Create a new lead record in the database
                        LeadDL leadDL = new LeadDL();
                        int sourceId = Convert.ToInt32(cbosource.SelectedValue);
                        bool isLeadAdded=leadDL.InsertLead(PersonId, sourceId);
                        if(isLeadAdded==true)
                        {
                            SignIn newfrm = new SignIn();
                            newfrm.Show();
                            this.Hide();

                        }
                        else
                        {
                            MessageBox.Show("Failed to insert owner: " + error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Failed to insert person: " + errorMessage);
                }
            }
            else
            {
                Console.WriteLine("Failed to insert address. Error: " + error);
            }
        }
    }
}
