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
    public partial class BuyerMenu : Form
    {
        int leadID;
        int personid;
        public BuyerMenu(int leadID,int personid)
        {
            InitializeComponent();
            this.leadID = leadID;
            this.personid = personid;
        }

        private void btnProperty_Click(object sender, EventArgs e)
        {
            ViewLeadPropety newfrm = new ViewLeadPropety(leadID,personid);
            newfrm.Show();
            this.Close();
        }

        private void btnInterest_Click(object sender, EventArgs e)
        {
            ViewInterestedIn newfrm = new ViewInterestedIn(leadID);
            newfrm.ShowDialog();
        }

        private void btnResponse_Click(object sender, EventArgs e)
        {
            Response newfrm = new Response(leadID,personid);
            newfrm.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form moreform = new Home();
            moreform.Show();
            this.Hide();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {

            ViewLeadSales newfrm = new ViewLeadSales(personid);
            newfrm.ShowDialog();
        }
    }
}
