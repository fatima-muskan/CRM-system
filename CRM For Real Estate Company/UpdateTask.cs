using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CRM_For_Real_Estate_Company.DL;
using System.Data.SqlClient;

namespace CRM_For_Real_Estate_Company
{
    public partial class UpdateTask : Form
    {
        public static List<string> name = new List<string>();
        int TaskId;
        int personId;
        public static string Connection;
        public UpdateTask(int TaskId,int personId)
        {
            InitializeComponent();
            this.TaskId = TaskId;
            this.personId = personId;
        }

        public static void GetPriorityLevelNameData()
        {

            Connection = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";

            using(SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT name from PriorityLevel", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    name.Add((string)reader["name"]);
                }
                reader.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Connection = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";

            if (string.IsNullOrEmpty(txtDescription.Text) == false)
            {
                TaskDL task = new TaskDL(Connection);
                int employeeid = task.GettaskEmployeeId(TaskId);
                string title = task.GettaskTitle(TaskId);
                int assigneeid = task.GettaskAssignedId(TaskId);
                int Prioritylevel = -1;
                using (SqlConnection connection = new SqlConnection(Connection))
                {
                    connection.Open();
                    string query = "Select id from PriorityLevel where name = @name";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", cboPriorityLevel.Text);
                    object result1 = command.ExecuteScalar();
                    if (result1 != null)
                    {
                        Prioritylevel = (int)result1;
                    }

                    connection.Close();
                }


                if (assigneeid != -1 && employeeid != -1)
                {
                    bool result = task.UpdateTask(TaskId, title, txtDescription.Text, dateTimePicker1.Value, employeeid, Prioritylevel, assigneeid);
                    if (result == true)
                    {
                        MessageBox.Show("Updated Sucessfully!!");
                        this.Close();
                    }                   
                }
            }
            else
            {
                MessageBox.Show("issue with task!!");
            }
        }

        private void UpdateTask_Load(object sender, EventArgs e)
        {
            GetPriorityLevelNameData();

            cboPriorityLevel.Text = "Select Priority Level";
            cboPriorityLevel.DataSource = name;

            List<object> uniqueItems = new List<object>();
            foreach (object item in cboPriorityLevel.Items)
            {
                if (!uniqueItems.Contains(item))
                {
                    uniqueItems.Add(item);
                }
            }

            cboPriorityLevel.DataSource = new BindingSource(uniqueItems, null);
        }
    }
}
