using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRM_For_Real_Estate_Company
{
    public partial class ViewOwnerSales : Form
    {
        private int pid;
        public ViewOwnerSales(int pid)
        {
            this.pid = pid;
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ViewOwnerSales_Load(object sender, EventArgs e)
        {
            var con = Configuration.getInstance().getConnection();

            SqlCommand cmd1 = new SqlCommand("select \r\n(select a.streetName+' '+a.state+' '+a.city+' '+a.country from Address as a join \r\nProperty as p on p.addressid=a.id where p.id=o.propertyid) as [Address],(select price from Property where id=o.propertyid) as price,\r\n(select username from person where id=b.personid) as Buyer\r\nfrom Owner as o join Sales as s on s.propertyid=o.propertyid\r\njoin Buyer as b on b.propertyid=o.propertyid\r\nwhere o.personid="+pid+"", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
        }
    }
}
