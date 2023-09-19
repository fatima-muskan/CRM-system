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
    public partial class ViewSaleAdmin : Form
    {
        private string connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";

        public ViewSaleAdmin()
        {
            InitializeComponent();
        }

        private void ViewSaleAdmin_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "Select s.id as SalesId,p.username as SellerName,t.name as PropertyType,(a.streetName+','+a.city+','+a.country) "+
                "as Propertyaddress, " +
                "per.username as BuyerName,price,saleDate from Sales as s " +
                "INNER JOIN(Select id, personid from Owner) as o ON o.id = s.sellerid " +
                "INNER JOIN(Select id, username from Person) as p ON p.id = o.personid " +
                "INNER JOIN(Select id, typeid, addressid from Property) as pro ON pro.id = s.propertyid " +
                "INNER JOIN(Select name, id from Type) as t ON t.id = pro.typeid " +
                "INNER JOIN(Select streetName, city, country, id from Address) as a ON a.id = pro.addressid " +
                "INNER JOIN(Select id, personid from Buyer) as b ON b.id = s.buyerid " +
                "INNER JOIN(Select id, username from Person) as per ON per.id = b.personid";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvSale.DataSource = dt;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text) == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "Select s.id as SalesId,p.username as SellerName,t.name as PropertyType,(a.streetName+','+a.city+','+a.country) " +
                    "as Propertyaddress, " +
                    "per.username as BuyerName,price,saleDate from Sales as s WITH(INDEX(idx_Sales_Date)) " +
                    "INNER JOIN(Select id, personid from Owner) as o ON o.id = s.sellerid " +
                    "INNER JOIN(Select id, username from Person) as p ON p.id = o.personid " +
                    "INNER JOIN(Select id, typeid, addressid from Property) as pro ON pro.id = s.propertyid " +
                    "INNER JOIN(Select name, id from Type) as t ON t.id = pro.typeid " +
                    "INNER JOIN(Select streetName, city, country, id from Address) as a ON a.id = pro.addressid " +
                    "INNER JOIN(Select id, personid from Buyer) as b ON b.id = s.buyerid " +
                    "INNER JOIN(Select id, username from Person) as per ON per.id = b.personid " +
                    "where s.saleDate LIKE @date + '%'";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@date", txtSearch.Text);
                    SqlDataAdapter da = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvSale.DataSource = dt;
                    MessageBox.Show("Searched Successfully!!");
                }
            }

            else
            {
                MessageBox.Show("Enter Sales Date to Search.");
            }
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
                string query = "Select s.id as SalesId,p.username as SellerName,t.name as PropertyType,(a.streetName+','+a.city+','+a.country) " +
                "as Propertyaddress, " +
                "per.username as BuyerName,price,saleDate from Sales as s " +
                "INNER JOIN(Select id, personid from Owner) as o ON o.id = s.sellerid " +
                "INNER JOIN(Select id, username from Person) as p ON p.id = o.personid " +
                "INNER JOIN(Select id, typeid, addressid from Property) as pro ON pro.id = s.propertyid " +
                "INNER JOIN(Select name, id from Type) as t ON t.id = pro.typeid " +
                "INNER JOIN(Select streetName, city, country, id from Address) as a ON a.id = pro.addressid " +
                "INNER JOIN(Select id, personid from Buyer) as b ON b.id = s.buyerid " +
                "INNER JOIN(Select id, username from Person) as per ON per.id = b.personid";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvSale.DataSource = dt;
            }
        }
    }
}
