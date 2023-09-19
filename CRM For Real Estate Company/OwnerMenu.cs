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
    public partial class OwnerMenu : Form
    {
        int personid;
        int paymentid;
        public OwnerMenu(int personid, int paymentid)
        {
            InitializeComponent();
            this.personid = personid;
            this.paymentid = paymentid;
        }

        private void btnProperty_Click(object sender, EventArgs e)
        {
            AddProperty newfrm = new AddProperty(personid, paymentid);
            newfrm.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdatePayment newfrm = new UpdatePayment(paymentid,personid);
            newfrm.Show();
            this.Hide();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            Form moreform = new ViewOwnerSales(personid);
            moreform.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form moreform = new Home();
            moreform.Show();
            this.Hide();
        }
    }
}
