using CRM_For_Real_Estate_Company.BL;
using CRM_For_Real_Estate_Company.DL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_For_Real_Estate_Company
{
    public partial class InsertProperty : Form
    {
        int personid;
        int paymentid;
        public InsertProperty(int personid, int paymentid)
        {
            InitializeComponent();
            this.personid = personid;
            this.paymentid = paymentid;
        }

        private void InsertProperty_Load(object sender, EventArgs e)
        {
            LoadPropertyTypes();
            LoadStatusTypes();
            LoadEmployees();

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Property not inserted successfully!");
            AddProperty newfrm = new AddProperty(personid, paymentid);
            newfrm.Show();
            this.Hide();
        }

        private void btnSelectImge_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pbPropertyImage.Image = Image.FromFile(openFileDialog.FileName);
            }
        }
        private void LoadPropertyTypes()
        {
            // Create an instance of TypeDL
            TypeDL typeDL = new TypeDL();

            // Get all property types
            List<TypeBL> propertyTypes = typeDL.GetAllTypesByCategory("PropertyType");

            // Set data source, display member, and value member for the combo box
            cboType.DataSource = propertyTypes;
            cboType.DisplayMember = "Name"; // Display the Name property of TypeBL objects
            cboType.ValueMember = "Id"; // Use the Id property of TypeBL objects as the value member

            // Set default selected item, if needed
            if (cboType.Items.Count > 0)
            {
                cboType.SelectedIndex = 0;
            }
        }
        private void LoadStatusTypes()
        {
            // Create an instance of StatusDL
            StatusDL statusDL = new StatusDL();

            // Get all status types for the desired category (e.g. "SalesType")
            List<StatusBL> statusTypes = statusDL.GetAllStatusByCategory("SalesType");

            // Set data source, display member, and value member for the combo box
            cboStatus.DataSource = statusTypes;
            cboStatus.DisplayMember = "Name"; // Display the Name property of StatusBL objects
            cboStatus.ValueMember = "Id"; // Use the Id property of StatusBL objects as the value member

            // Set default selected item, if needed
            if (cboStatus.Items.Count > 0)
            {
                cboStatus.SelectedIndex = 0;
            }
        }
        private void LoadEmployees()
        {
            // Create an instance of EmployeeDL
            EmployeeDL employeeDL = new EmployeeDL();

            // Get all employees from the database
            List<EmployeeBL> employees = employeeDL.GetAllEmployees();

            // Set data source, display member, and value member for the combo box
            cboEmployee.DataSource = employees;
            cboEmployee.DisplayMember = "FullName"; // Display the FullName property of EmployeeBL objects
            cboEmployee.ValueMember = "Id"; // Use the Id property of EmployeeBL objects as the value member

            // Set default selected item, if needed
            if (cboEmployee.Items.Count > 0)
            {
                cboEmployee.SelectedIndex = 0;
            }
        }
        private byte[] GetImageBytesFromPictureBox(Image image)
        {
            // Convert image from picture box to byte array
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddressBL address = new AddressBL(txtCountry.Text, txtState.Text, txtStreet.Text, txtCity.Text);
            AddressDL addressid = new AddressDL();
            int addressId;
            string addressError;
            // Call the InsertAddress function and pass the address object as a parameter
            bool isSuccess = addressid.InsertAddress(address, out addressId, out addressError);

            // Check the return value and output values
            if (isSuccess && addressId != 0)
            {
                PropertyDL property = new PropertyDL();
                int price = Convert.ToInt32(txtPrice.Text);
                int statusId = Convert.ToInt32(cboStatus.SelectedValue);
                int typeId = Convert.ToInt32(cboType.SelectedValue);
                int employeeId = Convert.ToInt32(cboEmployee.SelectedValue);
                byte[] image = GetImageBytesFromPictureBox(pbPropertyImage.Image);
                // Call InsertProperty method in the business layer
                int PropertyId = property.InsertProperty(price, addressId, statusId, typeId, employeeId, image);
                OwnerDL ownerDL = new OwnerDL();
                string error;
                bool ownerSuccess = ownerDL.InsertOwner(PropertyId, personid, paymentid, out error);
                if (ownerSuccess)
                {
                    MessageBox.Show("Property inserted successfully!");
                    AddProperty newfrm = new AddProperty(personid, paymentid);
                    newfrm.Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Failed to insert owner: " + error);
                }

            }
            else
            {
                MessageBox.Show("Error adding address: " + addressError);
            }
        }
    }
}
