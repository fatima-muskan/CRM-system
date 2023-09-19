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
    public partial class AddOwnerProperty : Form
    {
        public int PropertyId { get; set; }
        public int PaymentId { get; set; }
        public AddOwnerProperty()
        {
            InitializeComponent();
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
            cboStatusType.DataSource = statusTypes;
            cboStatusType.DisplayMember = "Name"; // Display the Name property of StatusBL objects
            cboStatusType.ValueMember = "Id"; // Use the Id property of StatusBL objects as the value member

            // Set default selected item, if needed
            if (cboStatusType.Items.Count > 0)
            {
                cboStatusType.SelectedIndex = 0;
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

        private void AddOwnerProperty_Load(object sender, EventArgs e)
        {
            LoadPropertyTypes();
            LoadStatusTypes();
            LoadEmployees();
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
                int statusId = Convert.ToInt32(cboStatusType.SelectedValue);
                int typeId = Convert.ToInt32(cboType.SelectedValue);
                int employeeId = Convert.ToInt32(cboEmployee.SelectedValue);
                byte[] image = GetImageBytesFromPictureBox(pbPropertyImage.Image);
                // Call InsertProperty method in the business layer
                PropertyId = property.InsertProperty(price, addressId, statusId, typeId, employeeId, image);
                // Convert the txtAccountNo.Text value to a char[] type
                char[] accountNo = txtAccountNo.Text.PadRight(16).Substring(0, 16).ToCharArray();
                // Create a new PaymentsMethodBL object with the converted accountNo value
                PaymentsMethodBL payments = new PaymentsMethodBL(txtBank.Text, accountNo, txtMethod.Text);
                PaymentsMethodDL paymentInsert = new PaymentsMethodDL();
                Tuple<int, string> result = paymentInsert.InsertPaymentMethod(payments);
                PaymentId = result.Item1;
                string paymentError = result.Item2;

                if (PaymentId != 0 && PropertyId != 0)
                {
                    MessageBox.Show("Property added successfully!");
                    this.DialogResult = DialogResult.OK;
                    // Close the form
                    this.Close();
                }
                else
                {
                    if (!string.IsNullOrEmpty(paymentError))
                    {
                        MessageBox.Show("Error adding payment method: " + paymentError);
                        MessageBox.Show(txtAccountNo.TextLength.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Error adding property.");
                    }
                    // delete the address if the property was not added successfully
                    //addressid.DeleteAddress(addressId);
                }
            }
            else
            {
                MessageBox.Show("Error adding address: " + addressError);
            }
        }


        private void btnBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
