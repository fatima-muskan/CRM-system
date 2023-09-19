using CRM_For_Real_Estate_Company.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;

namespace CRM_For_Real_Estate_Company.DL
{
    class TaskDL
    {
        private string connectionString;
        private List<TaskBL> Tasklist = new List<TaskBL>();

        public TaskDL(string connectionString)
        {
            this.connectionString = connectionString;
            LoadAllTask();
        }
        private void LoadAllTask()
        {
            Tasklist.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetAllTask", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TaskBL t = new TaskBL(); 
                    t.Id = Convert.ToInt32(reader["id"]);
                    //t.EmployeeId = Convert.ToInt32(reader["employeeid"]);
                    t.Title = Convert.ToString(reader["title"]);
                    t.Description = Convert.ToString(reader["Description"]);
                    t.DueDate = Convert.ToDateTime(reader["duedate"]);
                    t.Prioritylevel = Convert.ToInt32(reader["priorityLevelid"]);                
                    t.AssignedId = Convert.ToInt32(reader["assigneeid"]);
                    t.EmployeeId = Convert.ToInt32(reader["personid"]);
                    Tasklist.Add(t);
                }
                reader.Close();
                
            }
        }
        public bool InsertTask(string title,string description,DateTime dusdate,int employeeId,int prioritylevel,int assignedId)
        {
            bool flag = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_InsertTask", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@DueDate", dusdate);
                    command.Parameters.AddWithValue("@PersonId", employeeId);
                    command.Parameters.AddWithValue("@Prioritylevel", prioritylevel);
                    command.Parameters.AddWithValue("@AssignedId", assignedId);
                    // Add the @TaskId parameter and specify its direction as output
                    SqlParameter outputParam = new SqlParameter("@TaskId", SqlDbType.Int);
                    outputParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParam);

                    // Execute the stored procedure
                    command.ExecuteNonQuery();

                    // Retrieve the value of the @TaskId output parameter
                    int taskId = (int)outputParam.Value;
                    command.ExecuteNonQuery();
                    LoadAllTask();
                    flag = true;
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error inserting Task: " + ex.Message);
                    throw;
                }
            }
            return flag;
        }

        public bool UpdateTask(int id,string title, string description, DateTime dusdate, int employeeId, int prioritylevel, int AssignedId)
        {
            bool flag = false;
            TaskBL TaskToUpdate = Tasklist.FirstOrDefault(e => e.Id == id);
            if (TaskToUpdate != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_UpdateTask", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TaskId", id);
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@DueDate", dusdate);
                        command.Parameters.AddWithValue("@PersonId", employeeId);
                        command.Parameters.AddWithValue("@Prioritylevel", prioritylevel);
                        command.Parameters.AddWithValue("@AssignedId", AssignedId);
                        
                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        
                        LoadAllTask();
                        flag = true;
                    }

                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error While Updating Task: " + ex.Message);
                        throw;
                        //flag = false;
                    }

                }
            }
            return flag;
        }

        public string GettaskTitle(int taskId)
        {
            //int addressId = -1;
            string title = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "select title from Task where Task.id=@id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", taskId);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    title = Convert.ToString(result);
                }

                connection.Close();
            }
            return title;
        }

        public int GettaskEmployeeId(int taskId)
        {
            //int addressId = -1;
            int employeeid = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "select personid from Task where Task.id=@id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", taskId);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    employeeid = (int)result;
                }

                connection.Close();
            }
            return employeeid;
        }

        public int GettaskAssignedId(int taskId)
        {
            //int addressId = -1;
            int assigneeid = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "select assigneeid from Task where Task.id=@id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", taskId);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    assigneeid = (int)result;
                }

                connection.Close();
            }
            return assigneeid;
        }

        public bool DeleteTask(int id)
        {
            bool flag = false;
            TaskBL TaskToDelete = Tasklist.FirstOrDefault(e => e.Id == id);
            if (TaskToDelete != null)
            {
               Tasklist.Remove(TaskToDelete);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_DeleteTask", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TaskId", id);
                        command.ExecuteNonQuery();
                        LoadAllTask();
                        flag = true;
                    }

                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error While Deleting Task: " + ex.Message);
                        throw;
                    }
                }
            }
            return flag;
        }

    }

}
