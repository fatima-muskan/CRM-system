using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CRM_For_Real_Estate_Company
{
    public partial class ViewProperty : Form
    {
        private string connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
        public ViewProperty()
        {
            InitializeComponent();
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

        private void ViewProperty_Load(object sender, EventArgs e)
        { 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT t.name as PropertyType, (a.streetName+','+a.city+','+a.country) as Propertyaddress,p.price as Price, " +
                "per.username as EmployeeName, s.name as PropertyStatus, p.image,pe.username AS OwnerName " +
                "FROM Property as p " +
                "INNER JOIN(Select name, id from Type) as t ON t.id = p.typeid " +
                "INNER JOIN(Select streetName, city, country, id from Address) as a ON a.id = p.addressid " +
                "INNER JOIN(Select id, personid from Employee) as e ON e.id = p.employeeid " +
                "INNER JOIN(Select id, username from Person) as per ON per.id = e.personid " +
                "INNER JOIN(Select id, name from Status) as s ON s.id = p.statusid " +
                "INNER JOIN(Select id, personid, propertyid from Owner) as o ON propertyid = p.id " +
                "INNER JOIN(Select id, username from Person) as pe ON pe.id = o.personid";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvViewProperty.DataSource = dt;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Control c in tableLayoutPanel1.Controls)
                {
                    if (c is TextBox)
                        ((TextBox)c).Clear();
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT t.name as PropertyType, (a.streetName+','+a.city+','+a.country) as Propertyaddress,p.price as Price, " +
                "per.username as EmployeeName, s.name as PropertyStatus, p.image,pe.username AS OwnerName " +
                "FROM Property as p " +
                "INNER JOIN(Select name, id from Type) as t ON t.id = p.typeid " +
                "INNER JOIN(Select streetName, city, country, id from Address) as a ON a.id = p.addressid " +
                "INNER JOIN(Select id, personid from Employee) as e ON e.id = p.employeeid " +
                "INNER JOIN(Select id, username from Person) as per ON per.id = e.personid " +
                "INNER JOIN(Select id, name from Status) as s ON s.id = p.statusid " +
                "INNER JOIN(Select id, personid, propertyid from Owner) as o ON propertyid = p.id " +
                "INNER JOIN(Select id, username from Person) as pe ON pe.id = o.personid";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvViewProperty.DataSource = dt;
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text) == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT t.name as PropertyType, (a.streetName+','+a.city+','+a.country) as Propertyaddress,p.price as Price, " +
                    "per.username as EmployeeName, s.name as PropertyStatus, p.image,pe.username AS OwnerName " +
                    "FROM Property as p " +
                    "INNER JOIN(Select name, id from Type WITH(INDEX(idx_Property_typeName))) as t ON t.id = p.typeid " +
                    "INNER JOIN(Select streetName, city, country, id from Address) as a ON a.id = p.addressid " +
                    "INNER JOIN(Select id, personid from Employee) as e ON e.id = p.employeeid " +
                    "INNER JOIN(Select id, username from Person) as per ON per.id = e.personid " +
                    "INNER JOIN(Select id, name from Status) as s ON s.id = p.statusid " +
                    "INNER JOIN(Select id, personid, propertyid from Owner) as o ON propertyid = p.id " +
                    "INNER JOIN(Select id, username from Person) as pe ON pe.id = o.personid " +
                    "where t.name LIKE @name + '%'";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", txtSearch.Text);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvViewProperty.DataSource = dt;
                    MessageBox.Show("Searched Successfully!!");
                }
            }

            else
            {
                MessageBox.Show("Enter Property Type to Search.");
            }
        }
    }
}
