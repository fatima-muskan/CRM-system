using CRM_For_Real_Estate_Company.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CRM_For_Real_Estate_Company.DL
{
     class NotificationDL
    {
        private string connectionString;
        private List<NotificationBL> Notificationlist = new List<NotificationBL>();

        public NotificationDL(string connectionString,int pid)
        {
            this.connectionString = connectionString;
            LoadAllNotification(pid);
        }

        private void LoadAllNotification(int pid)
        {
            Notificationlist.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_LoadNotification", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@person_ID", pid);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    NotificationBL n = new NotificationBL();
                    n.Rid = Convert.ToInt32(reader["id"]);
                    n.Title = (reader["title"]).ToString();
                    n.Description =(reader["description"]).ToString();
                    n.Duedate = Convert.ToDateTime(reader["duedate"]);
                    Notificationlist.Add(n);
                }
                reader.Close();
                connection.Close();
            }
        }

        public List<string> messages()
        {
            List<string> msg = new List<string>();
            foreach(NotificationBL n in Notificationlist)
            {
                string s = n.Rid.ToString() + " The task named as " + n.Title + " Described as " + n.Description + " has Due date " + (n.Duedate).ToString();
                msg.Add(s);
            }
            return msg;
            
        }

        public bool DateUpdate(int rid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd3 = new SqlCommand("update Reminder set lastSentDate =GETDATE() where id=" + rid + "", connection);
                cmd3.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            
        }

        public bool StatusChanged(int rid)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd1 = new SqlCommand("update Reminder set statusid =4 where id=" + rid + "", connection);
                cmd1.ExecuteNonQuery();
                connection.Close();
                return true;
            }
        }
       
    }
}
