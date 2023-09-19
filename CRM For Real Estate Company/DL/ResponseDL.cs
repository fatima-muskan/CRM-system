using CRM_For_Real_Estate_Company.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.DL
{
    class ResponseDL
    {
        private string connectionString;
        private Queue<ResponseBL> ResponseQueue = new Queue<ResponseBL>();

        public ResponseDL()
        {
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            //LoadAllResponse();
        }

        private void LoadAllResponse()
        {
            ResponseQueue.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetAllResponse", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ResponseBL r = new ResponseBL();
                    r.Id = Convert.ToInt32(reader["Id"]);
                    r.Name = Convert.ToString(reader["Name"]);
                    r.Description = Convert.ToString(reader["Description"]);
                    r.LeadId = Convert.ToInt32(reader["LeadId"]);
                    r.TaskId = Convert.ToInt32(reader["TaskId"]);
                    ResponseQueue.Enqueue(r);
                }
                reader.Close();
            }
        }
        public int InsertResponse(string name, string comment)
        {
            // Check if the name and comment are not null or empty
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(comment))
            {
                return 0;
            }

            int responseId = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("INSERT INTO [Response] (name, comment) OUTPUT INSERTED.id VALUES (@name, @comment)", connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@comment", comment);

                        responseId = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return responseId;
        }


        public void InsertResponse(string Name, string description, int leadId, int TaskId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_InsertResponse", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name",Name);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@LeadId", leadId);
                    command.Parameters.AddWithValue("@TaskId", TaskId);
                    command.ExecuteNonQuery();
                    LoadAllResponse();
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error inserting Response: " + ex.Message);
                    throw;
                }
            }
        }
       /* public void UpdateResponse(int id, string Name, string description, int leadId, int TaskId)
        {
            ResponseBL ResponseToUpdate = ResponseQueue.FirstOrDefault(e => e.Id == id);
            if (ResponseToUpdate != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_UpdateResponse", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Name", Name);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@LeadId", leadId);
                        command.Parameters.AddWithValue("@TaskId", TaskId);
                        command.ExecuteNonQuery();
                        LoadAllResponse();
                    }

                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error While Updating Response: " + ex.Message);
                        throw;
                    }

                }
            }
        }*/
    }
}
