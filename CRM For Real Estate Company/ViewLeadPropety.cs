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
    public partial class ViewLeadPropety : Form
    {
        int leadID;
        int personid;
        public ViewLeadPropety(int leadID,int personid)
        {
            InitializeComponent();
            this.leadID = leadID;
            this.personid = personid;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            BuyerMenu newfrm = new BuyerMenu(leadID,personid);
            newfrm.Show();
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                {
                    // Skip the new row
                    continue;
                }

                bool rowVisible = false;

                // Clear any selected cells to avoid InvalidOperationException
                dataGridView1.CurrentCell = null;

                // Concatenate all the cell values in the row into a single string for searching
                string rowValue = string.Join(" ", row.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value?.ToString()));

                if (rowValue.ToLower().Contains(txtSearch.Text))
                {
                    rowVisible = true;
                }

                row.Visible = rowVisible;
            }



        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }
        private void PopulateData()
        {
            // Get the list of interested properties for the given lead ID
            List<PropertyBL> interestedProperties = new PropertyDL().GetPropertiesForLeadId(leadID);

            // Create a new DataTable to hold the data
            DataTable dt = new DataTable();

            // Add columns to the DataTable
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Image", typeof(Image));  // Add a column for the image
            dt.Columns.Add("Interested", typeof(string)); // Add a column for interested status

            // Loop through each interested property and add a row to the DataTable
            foreach (PropertyBL interestedProperty in interestedProperties)
            {
                // Get the property details
                PropertyBL property = new PropertyDL().GetPropertyById(interestedProperty.Id);
                    TypeBL type = new TypeDL().GetTypeById(property.TypeId);
                    StatusBL status = new StatusDL().GetStatusById(property.StatusId);
                    AddressBL address = new AddressDL().GetAddressById(property.AddressId);
                    string fullAddress = address.StreetName + "," + address.State + "," + address.City + "," + address.Country;

                    // Check if the property is already in the Interested table
                    bool isInterested = new InterestedInDL().CheckIfPropertyIsInterestedByLeadIdAndPropertyId(leadID, property.Id);

                    // Add a new row to the DataTable
                    DataRow dr = dt.NewRow();
                    dr["ID"] = property.Id;
                    dr["Price"] = property.Price;
                    dr["Type"] = type.Name;
                    dr["Address"] = fullAddress;
                    dr["Status"] = status.Name;
                    dr["Image"] = property.Image;  // Add the image to the row
                    dr["Interested"] = isInterested ? "Uninterested" : "Interested";
                    dt.Rows.Add(dr);
                
            }

            // Set the DataGridView's DataSource to the DataTable
            dataGridView1.DataSource = dt;

            // Hide the ID column
            dataGridView1.Columns["ID"].Visible = false;

            // Set the image column's properties
            DataGridViewImageColumn imgCol = (DataGridViewImageColumn)dataGridView1.Columns["Image"];
            imgCol.HeaderText = "Image";
            imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;

            // Create a new DataGridViewButtonColumn
            DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
            btnCol.Name = "Buy";
            btnCol.HeaderText = "Buy";
            btnCol.Text = "Buy";
            btnCol.UseColumnTextForButtonValue = true;

            // Add the column to the DataGridView
            dataGridView1.Columns.Add(btnCol);
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        private void ViewLeadPropety_Load(object sender, EventArgs e)
        {
            PopulateData();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                Console.WriteLine(column.Name);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Interested"].Index)
            {
                // Check if the cell is not null before accessing its value
                if (dataGridView1.Rows[e.RowIndex].Cells["ID"].Value != null)
                {
                    int propertyId = (int)dataGridView1.Rows[e.RowIndex].Cells["ID"].Value;
                    bool isInterested = dataGridView1.Rows[e.RowIndex].Cells["Interested"].Value.ToString() == "Interested";

                    if (isInterested)
                    {
                        // Add the property to the Interested table
                        bool result = new InterestedInDL().AddInterestedProperty(leadID, propertyId);
                        if (!result) { MessageBox.Show("not valid command"); }
                        else { dataGridView1.Rows[e.RowIndex].Cells["Interested"].Value = "Uninterested"; }
                    }
                    else
                    {
                        // Remove the property from the Interested table
                        bool result = new InterestedInDL().RemoveInterestedProperty(leadID, propertyId);
                        if (!result)
                        { MessageBox.Show("not valid command"); }
                        else { dataGridView1.Rows[e.RowIndex].Cells["Interested"].Value = "Interested"; }
                    }
                }
            }

            // Add an event handler for the "Buy" button column
            else if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView1.Columns["Buy"].Index)
            {
                int propertyId = (int)dataGridView1.Rows[e.RowIndex].Cells["ID"].Value;
                BuyProperty newfrm = new BuyProperty(personid, propertyId);
                newfrm.ShowDialog();
            }
        }
    }
}
