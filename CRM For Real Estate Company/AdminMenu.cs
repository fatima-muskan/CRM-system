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
    public partial class AdminMenu : Form
    {
        int personId;
        public AdminMenu(int personId)
        {
            InitializeComponent();
            this.personId = personId;
        }

        private Form activeForm = null;

        private void OpenChildForm(Form childfrom)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childfrom;
            childfrom.TopLevel = false;
            childfrom.FormBorderStyle = FormBorderStyle.None;
            childfrom.Dock = DockStyle.Fill;
            panelChildForm.Controls.Add(childfrom);
            panelChildForm.Tag = childfrom;
            childfrom.BringToFront();
            childfrom.Show();
        }
        private void AdminMenu_Load(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form moreform = new Home();
            moreform.Show();
            this.Hide();
        }

        private void btnTask_Click(object sender, EventArgs e)
        {
            OpenChildForm(new TaskAdmin(personId)); 
        }

        private void btnProperty_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ViewProperty());           
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ViewSaleAdmin());
        }

        private void btnLead_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Lead2());
        }

        private void btnResponse_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ViewResponseAdmin());
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ViewPaymentAdmin());
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            OpenChildForm(new AdminReport());
        }
    }
}
