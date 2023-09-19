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
    public partial class AddProperty : Form
    {
        int personid;
        int paymentid;
        public AddProperty(int personid, int paymentid)
        {
            InitializeComponent();
            this.personid = personid;
            this.paymentid = paymentid;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            InsertProperty newfrm = new InsertProperty(personid,paymentid);
            newfrm.Show();
            this.Hide();
        }
        private List<PropertyBL> GetAllPropertiesWithImages()
        {
            List<PropertyBL> properties = new List<PropertyBL>();
            PropertyDL propertyDL = new PropertyDL();
            properties = propertyDL.GetAllPropertiesWithImages(personid);
            return properties;
        }
        private void BindPropertiesData()
        {
            List<PropertyBL> properties = GetAllPropertiesWithImages();

            // Set up the DataGridView columns
            dataGridView1.AutoGenerateColumns = false;

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "Id";
            idColumn.HeaderText = "ID";
            idColumn.Name = "ID";

            DataGridViewTextBoxColumn priceColumn = new DataGridViewTextBoxColumn();
            priceColumn.DataPropertyName = "Price";
            priceColumn.HeaderText = "Price";
            priceColumn.Name = "Price";

            DataGridViewTextBoxColumn typeColumn = new DataGridViewTextBoxColumn();
            typeColumn.DataPropertyName = "TypeId";
            typeColumn.HeaderText = "Type";
            typeColumn.Name = "Type";

            DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();
            imageColumn.DataPropertyName = "Image";
            imageColumn.HeaderText = "Image";
            imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;
            imageColumn.Name = "Image";

            // Add the columns to the DataGridView
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { idColumn, priceColumn, typeColumn, imageColumn });

            // Bind the data to the DataGridView
            dataGridView1.DataSource = properties;

            // Add an "Update" button column
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.HeaderText = "Update";
            buttonColumn.Name = "Update";
            buttonColumn.Text = "Update";
            buttonColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(buttonColumn);
        }
        private void AddProperty_Load(object sender, EventArgs e)
        {
            BindPropertiesData();
                    
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Update"].Index && e.RowIndex >= 0)
            {
                MessageBox.Show(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString());
                int propertyId = (int)dataGridView1.Rows[e.RowIndex].Cells["ID"].Value;
                UpdateProperty updatePropertyForm = new UpdateProperty(propertyId, personid, paymentid);
                updatePropertyForm.ShowDialog();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                bool rowVisible = false;

                // Clear any selected cells to avoid InvalidOperationException
                dataGridView1.CurrentCell = null;

                // Concatenate all the cell values in the row into a single string for searching
                string rowValue = string.Join(" ", row.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value?.ToString()));

                if (rowValue.ToLower().Contains(searchText))
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

        private void btnBack_Click(object sender, EventArgs e)
        {
            Form moreform = new OwnerMenu(personid,paymentid);
            moreform.Show();
            this.Hide();

        }
    }
}

