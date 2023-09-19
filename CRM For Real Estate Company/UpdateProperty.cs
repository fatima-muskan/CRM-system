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
    public partial class UpdateProperty : Form
    {
        int propertyId;
        int personid;
        int paymentid;
        public UpdateProperty(int propertyid,int personid,int paymentid)
        {
            InitializeComponent();
            this.propertyId = propertyid;
            this.personid = personid;
            this.paymentid = paymentid;

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
        private List<PropertyBL> GetAllPropertiesWithImages()
        {
            List<PropertyBL> properties = new List<PropertyBL>();
            PropertyDL propertyDL = new PropertyDL();
            properties = propertyDL.GetAllPropertiesWithImages(personid);
            return properties;
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            PropertyDL newproperty = new PropertyDL();
            int addressid=newproperty.GetTheAddressId(propertyId);
            if(addressid!=-1)
            {
                AddressDL addressaDL = new AddressDL();
                bool result=addressaDL.UpdateAddressForProperty(propertyId, txtCountry.Text, txtState.Text, txtStreet.Text, txtCity.Text);
                if (result)
                {
                    result = newproperty.UpdateProperty(propertyId, Convert.ToInt32(cboType.SelectedValue), Convert.ToInt32(cboStatus.SelectedValue), Convert.ToDecimal(txtPrice.Text), addressid);
                    if (result == true)
                    {
                        MessageBox.Show("Updated");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("issue with property");
                    }
                }
                else
                {
                    MessageBox.Show("issue with address");
                }
            }
            else
            {
                MessageBox.Show("Property not found with this address!");
            }
        }
        private void UpdateProperty_Load(object sender, EventArgs e)
        {
            LoadPropertyTypes();
            LoadStatusTypes();
            PopulateForm();
        }
        private void PopulateForm()
        {
            PropertyDL propertyDL = new PropertyDL();
            PropertyBL property = propertyDL.GetPropertyById(propertyId);

            if (property != null)
            {
                // Set the textboxes with the property details
                txtPrice.Text = property.Price.ToString();
                cboType.SelectedValue = property.TypeId;
                cboStatus.SelectedValue = property.StatusId;

            }
            AddressDL addressDL = new AddressDL();
            AddressBL address = addressDL.GetAddressById(property.AddressId);

            if (address != null)
            {
                txtStreet.Text = address.StreetName;
                txtCity.Text = address.City;
                txtState.Text = address.State;
                txtCountry.Text = address.Country;
            }
        }
    }
}
