using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CRM_For_Real_Estate_Company.DL;


namespace CRM_For_Real_Estate_Company
{
    public partial class EmployeeMenu : Form
    {

        private int pid;

        public EmployeeMenu(int pid)
        {
            InitializeComponent();
            this.pid = pid;

        }
        
        private void EmployeeMenu_Load(object sender, EventArgs e)
        {
            string Connection = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            NotificationDL li = new NotificationDL(Connection,pid);
           

            /*//while (true)
             //{
                 var con = Configuration.getInstance().getConnection();
                 SqlCommand cmd1 = new SqlCommand("Select Reminder.id,Task.title,Task.description,Task.duedate,Reminder.typeid from Reminder join Task on Task.id = Reminder.taskid join Employee on Employee.id=Task.assigneeId where Reminder.statusid=4 and Employee.personid=" + pid + "", con);
                 SqlDataReader reader = cmd1.ExecuteReader();
                 if (reader.Read())
                 {
                    int rid=Convert.ToInt32(reader["id"]);
                     if (Convert.ToInt32(reader["typeid"]) == 5)
                     {
                         notifyIcon1.Icon = new System.Drawing.Icon(Path.GetFullPath("order.ico"));
                         notifyIcon1.Visible = true;
                         notifyIcon1.BalloonTipTitle = "Reminder for Task";
                         notifyIcon1.BalloonTipText = "reader[\"id\"]}\\nTask Title: {reader[\"title\"]}\\nTask Description: {reader[\"description\"]}\\nDue Date: {reader[\"duedate\"]}";
                         notifyIcon1.ShowBalloonTip(100);
                         notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                         SqlCommand cmd2 = new SqlCommand("update Reminder set lastSentDate =GETDATE() where id=" + rid + "", con);
                         cmd2.ExecuteNonQuery();
                         NotifyIcon1_BalloonTipClicked(sender, e, rid);

                        // await System.Threading.Tasks.Task.Delay(3600000);
                     }
                     else if (Convert.ToInt32(reader["typeid"]) == 6)
                     {
                         notifyIcon1.Icon = new System.Drawing.Icon(Path.GetFullPath("order.ico"));
                         notifyIcon1.Visible = true;
                         notifyIcon1.BalloonTipTitle = "Reminder for Task";
                         notifyIcon1.BalloonTipText = "reader[\"id\"]}\\nTask Title: {reader[\"title\"]}\\nTask Description: {reader[\"description\"]}\\nDue Date: {reader[\"duedate\"]}";
                         notifyIcon1.ShowBalloonTip(100);
                         notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                         SqlCommand cmd3 = new SqlCommand("update Reminder set lastSentDate =GETDATE() where id=" + rid + "", con);
                         cmd3.ExecuteNonQuery();
                         NotifyIcon1_BalloonTipClicked(sender, e, rid);
                       //  await System.Threading.Tasks.Task.Delay(86400000);
                     }
                     else if (Convert.ToInt32(reader["typeid"]) == 7)
                     {
                         notifyIcon1.Icon = new System.Drawing.Icon(Path.GetFullPath("order.ico"));
                         notifyIcon1.Visible = true;
                         notifyIcon1.BalloonTipTitle = "Reminder for Task";
                         notifyIcon1.BalloonTipText = "reader[\"id\"]}\\nTask Title: {reader[\"title\"]}\\nTask Description: {reader[\"description\"]}\\nDue Date: {reader[\"duedate\"]}";
                         notifyIcon1.ShowBalloonTip(100);
                         notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                         SqlCommand cmd4 = new SqlCommand("update Reminder set lastSentDate =GETDATE() where id=" + rid + "", con);
                         cmd4.ExecuteNonQuery();
                         NotifyIcon1_BalloonTipClicked(sender, e, rid);
                        // await System.Threading.Tasks.Task.Delay(86400000 * 7);
                     }
                     reader.Close();

                     con.Close();
                 }
              //   await System.Threading.Tasks.Task.Delay(2600000);

             //}*/

            List<string> msg = li.messages();
            int rid;
            foreach (string m in msg)
            {
                
               
                notifyIcon1.Icon = new System.Drawing.Icon(Path.GetFullPath("order.ico"));
                notifyIcon1.Visible = true;
                notifyIcon1.BalloonTipTitle = "Reminder for Task";
                notifyIcon1.BalloonTipText = m;
                notifyIcon1.ShowBalloonTip(10000);
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                if (int.TryParse(m.Substring(0, 1), out rid))
                {
                    li.DateUpdate(rid) ;
                }
                NotifyIcon1_BalloonTipClicked(sender,e,m);
            }
            
        }
       
        private void NotifyIcon1_BalloonTipClicked(object sender, EventArgs e,string m)
        {
            int rid;
            string Connection = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            NotificationDL lis = new NotificationDL(Connection, pid);

            DialogResult result = MessageBox.Show("Have you done your task that is "+m, "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (int.TryParse(m.Substring(0, 1), out rid))
                {
                    lis.StatusChanged(rid);
                }
            }
            else if (result == DialogResult.No)
            {
               
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            Form morefrom = new EReport(pid);
            morefrom.Show();
            

        }

        private void btnAssignedProperty_Click(object sender, EventArgs e)
        {
            Form moreform = new ViewAssignedProperty(pid);
            moreform.Show();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Form moreform = new Home();
            moreform.Show();
            this.Hide();
        }

        private void btnViewTask_Click(object sender, EventArgs e)
        {
            Form moreform=new ViewEmployeeTask(pid);
            moreform.Show();
        }

        private void btnLead_Click(object sender, EventArgs e)
        {
            Form moreform = new ViewEmployeeProfile(pid);
            moreform.Show();
        }

        /*
          private void NotifyIcon1_BalloonTipClosed(object sender, EventArgs e)
          {
              // Handle the balloon tip close event here
              MessageBox.Show("Balloon tip closed!");

              // Add code to mark the notification as read or perform any other actions as needed
          }*/

      /*  private void reminders(string messages)
        {
         //   notifyIcon1.Icon = new System.Drawing.Icon(Path.GetFullPath("order.ico"));
            notifyIcon1.Visible = true;
            notifyIcon1.BalloonTipTitle = "Reminder for Task";
            notifyIcon1.BalloonTipText = messages;
            notifyIcon1.ShowBalloonTip(1000);
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipClicked += NotifyIcon1_BalloonTipClicked;
            notifyIcon1.Click += (s, e) =>
            {
                // Show the message in a MessageBox when the user clicks on the notification
                DialogResult result = MessageBox.Show(messages,"Do you want to fulfill the task?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    MessageBox.Show("Task fulfilled", "Fulfill Task");
                }
                else if (result == DialogResult.No)
                {
                    MessageBox.Show("Task remaining", "Remaining");
                }
               
            };
        }*/
    }
}
