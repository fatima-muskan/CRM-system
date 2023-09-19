using CRM_For_Real_Estate_Company.BL;
using CRM_For_Real_Estate_Company.DL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_For_Real_Estate_Company
{
    public partial class BuyProperty : Form
    {
        int personid;
        int propertyid;
        public BuyProperty(int personid, int propertyid)
        {
            InitializeComponent();
            this.personid = personid;
            this.propertyid = propertyid;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    char[] accountNo = txtAccountNo.Text.PadRight(16).Substring(0, 16).ToCharArray();
                    PaymentsMethodBL payments = new PaymentsMethodBL(txtBank.Text, accountNo, txtMethod.Text);
                    PaymentsMethodDL paymentInsert = new PaymentsMethodDL();

                    // Insert payment method
                    Tuple<int, string> result = paymentInsert.InsertPaymentMethod(payments);
                    int PaymentId = result.Item1;
                    string paymentError = result.Item2;

                    if (PaymentId != 0)
                    {
                        buyerDL buyerInsert = new buyerDL();

                        // Insert buyer
                        int buyerID = buyerInsert.InsertBuyer(personid, propertyid);

                        if (buyerID != 0)
                        {
                            PaymentsDL payment = new PaymentsDL();

                            // Insert payment
                            bool ans = payment.InsertPayment(buyerID, PaymentId);

                            if (ans == true)
                            {
                                PropertyDL property = new PropertyDL();
                                PropertyBL propertyData = property.GetPropertyById(propertyid);
                                if (propertyData != null)
                                {
                                    int sellerid = propertyData.EmployeeId;
                                    decimal price = propertyData.Price;
                                    DateTime currentDate = DateTime.Today.Date;
                                    SalesDL sales = new SalesDL();

                                    // Insert sale
                                    bool saleAdded = sales.InsertSale(sellerid, propertyid, buyerID, price, currentDate);

                                    if (saleAdded)
                                    {
                                        // Update property status
                                       // bool propertyUpdated = property.UpdatePropertyStatus(propertyid, 3);

                                        //if (propertyUpdated == true)
                                        //{
                                            LeadDL lead = new LeadDL();

                                            // Update lead connected date
                                            lead.UpdateLeadConnectedDate(personid, currentDate);

                                            MessageBox.Show("Sale added successfully! Property sold for $" + price.ToString() + ".");
                                            transaction.Commit();
                                            this.DialogResult = DialogResult.OK;
                                            this.Close();
                                        //}
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error inserting sale into the database.");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Property has been Sold");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Error inserting payment into the database.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Error inserting buyer into the database.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while inserting data into the database: " + ex.Message);
                    transaction.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Clicking Cancel would not insert new property ");
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
