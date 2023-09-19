using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CRM_For_Real_Estate_Company.BL;
using CRM_For_Real_Estate_Company.DL;
using System.Data.SqlClient;

namespace CRM_For_Real_Estate_Company
{
    public partial class TaskAdmin : Form
    {
        int personId;
        public static List<int> AssigneeId = new List<int>();
        public static string Connection;

        public TaskAdmin(int personId)
        {
            InitializeComponent();
            this.personId = personId;
        }

        public static void GetAssigneeIdData()
        {
            Connection = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT id from Employee", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    AssigneeId.Add((int)reader["id"]);
                }
                reader.Close();
            }
        }
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TaskAdmin_Load(object sender, EventArgs e)
        {
            Connection = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string query = "Select * from Task where personid = @personid";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@personid", personId);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }

            UpdateTask.GetPriorityLevelNameData();
            cboPriorityLevel.Text = "Select Priority Level";
            cboPriorityLevel.DataSource = UpdateTask.name;

            List<object> uniqueItems = new List<object>();
            foreach (object item in cboPriorityLevel.Items)
            {
                if (!uniqueItems.Contains(item))
                {
                    uniqueItems.Add(item);
                }
            }

            cboPriorityLevel.DataSource = new BindingSource(uniqueItems, null);

            GetAssigneeIdData();
            cboAssigneeId.Text = "Select Assignee Id";
            cboAssigneeId.DataSource = AssigneeId;
            List<object> uniqueItems2 = new List<object>();
            foreach (object item in cboAssigneeId.Items)
            {
                if (!uniqueItems2.Contains(item))
                {
                    uniqueItems2.Add(item);
                }
            }

            cboAssigneeId.DataSource = new BindingSource(uniqueItems2, null);
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string Connection = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";

            string Title = txtTitle.Text;
            string Description = txtDescription.Text;
            DateTime DueDate = dateTimePicker1.Value;
            //int EmployeeId = int.Parse(cboEmployeeId.Text);
            int EmployeeId = personId;
            int Prioritylevel = -1;

            Connection = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            
            if(string.IsNullOrEmpty(txtTitle.Text) == false && string.IsNullOrEmpty(txtDescription.Text)== false)
            {
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

                //Prioritylevel = int.Parse(cboPriorityLevel.Text);
                int AssignedId = int.Parse(cboAssigneeId.Text);
                TaskBL task = new TaskBL(Title, Description, DueDate, personId, Prioritylevel, AssignedId);
                TaskDL taskList = new TaskDL(Connection);
                bool result = taskList.InsertTask(Title, Description, DueDate, personId, Prioritylevel, AssignedId);
                if (result == true)
                {
                    MessageBox.Show("Task Inserted Sucessfully!!");
                    this.Close();
                }               
            }
            else
            {
                MessageBox.Show("Task Not Inserted Successfully!!");
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                {
                    // Skip the new row
                    continue;
                }

                bool rowVisible = false;

                // Clear any selected cells to avoid InvalidOperationException
                dataGridView1.CurrentCell = null;

                // Concatenate all the cell values in the row into a single string for searching
                string rowValue = string.Join(" ", row.Cells.Cast<DataGridViewCell>().Select(cell => cell.Value?.ToString()));

                if (rowValue.ToLower().Contains(txtSearch.Text))
                {
                    rowVisible = true;
                }

                row.Visible = rowVisible;
            }

        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Update"].Index && e.RowIndex >= 0)
            {
                MessageBox.Show(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString());
                int TaskId = (int)dataGridView1.Rows[e.RowIndex].Cells["ID"].Value;
                UpdateTask updatetask = new UpdateTask(TaskId,personId);
                updatetask.ShowDialog();

            }
            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                string Connection = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
                MessageBox.Show(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString());
                int TaskId = (int)dataGridView1.Rows[e.RowIndex].Cells["ID"].Value;
                TaskDL task = new TaskDL(Connection);
                bool result = task.DeleteTask(TaskId);
                if (result == true)
                {
                    MessageBox.Show("Task Deleted Sucessfully!!");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Task Not Deleted Successfully!!");
                }             
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Connection = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(Connection))
            {
                connection.Open();
                string query = "Select * from Task where personid = @personid";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@personid", personId);
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
    }
}
