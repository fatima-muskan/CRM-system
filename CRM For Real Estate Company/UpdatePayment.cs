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
    public partial class UpdatePayment : Form
    {
        int paymentid;
        int personid;
        public UpdatePayment(int paymentid,int personid)
        {
            InitializeComponent();
            this.paymentid = paymentid;
            this.personid = personid;
        }
        private void PopulateForm()
        {
            PaymentsMethodDL payment = new PaymentsMethodDL();
            PaymentsMethodBL payments = payment.GetPaymentsMethodById(paymentid);

            if (payments != null)
            {
                // Set the textboxes with the property details
                txtBank.Text = payments.BankName;
                string account = new string(payments.AccountNo);
                txtAccountNo.Text = account;
                txtMethod.Text = payments.PaymentMethod;
            }

        }

        private void UpdatePayment_Load(object sender, EventArgs e)
        {
            PopulateForm();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PaymentsMethodDL payment = new PaymentsMethodDL();
            bool result = payment.UpdatePayment(paymentid, txtBank.Text, txtAccountNo.Text, txtMethod.Text);
            if(result==true)
            {
                MessageBox.Show("Updated");
                OwnerMenu newfrm = new OwnerMenu(personid, paymentid);
                this.Hide();
                newfrm.Show();
            }
            else
            {
                MessageBox.Show("Invalid");
            }
        }
    }
}
