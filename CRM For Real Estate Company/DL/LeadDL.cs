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
    class LeadDL
    {
        private string connectionString;
        private List<LeadBL> Leadlist = new List<LeadBL>();

        public LeadDL()
        {
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            LoadAllLead();
        }
        // LeadDL class
        public int GetLeadIdByPersonId(int personId)
        {
            int leadId = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id FROM Lead WHERE PersonId = @PersonId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PersonId", personId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        leadId = (int)reader["id"];
                    }
                }
            }

            return leadId;
        }

        public bool UpdateLeadConnectedDate(int personId, DateTime date)
        {
            int leadId = GetLeadIdByPersonId(personId);

            if (leadId == 0)
            {
                return false;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Lead SET dateConnected = @Date WHERE Id = @LeadId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Date", date);
                cmd.Parameters.AddWithValue("@LeadId", leadId);
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public int SearchLeadIdByPersonId(int personId)
        {
            int leadId = -1;
            foreach (LeadBL lead in Leadlist)
            {
                if (lead.PersonId == personId)
                {
                    leadId = lead.Id;
                    break;
                }
            }
            return leadId;
        }

        private void LoadAllLead()
        {
            Leadlist.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetAllLead", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LeadBL n = new LeadBL();
                    n.Id = Convert.ToInt32(reader["Id"]);
                    n.Dateadded = Convert.ToDateTime(reader["Dateadded"]);
                    n.DateConnected = Convert.ToDateTime(reader["DateConnected"]);
                    n.SourceId = Convert.ToInt32(reader["SourceId"]);
                    n.PersonId = Convert.ToInt32(reader["PersonId"]);
                    Leadlist.Add(n);
                }
                reader.Close();
            }
        }

        public bool InsertLead(int PersonId, int SourceId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_InsertLead", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@Dateadded", Dateadded);
                    //command.Parameters.AddWithValue("@DateConnected", DateConnected);
                    command.Parameters.AddWithValue("@SourceId", SourceId);
                    command.Parameters.AddWithValue("@PersonId", PersonId);
                    command.ExecuteNonQuery();
                    LoadAllLead();
                    return true;
                }
                catch (SqlException ex)
                {
                    return false;
                }
            }
        }

        public void UpdateLead(int Id, DateTime Dateadded, DateTime DateConnected, int SourceId, int AddressId, int PersonId)
        {
            LeadBL LeadToUpdate = Leadlist.FirstOrDefault(e => e.Id == Id);
            if (LeadToUpdate != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_UpdateLead", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", Id);
                        command.Parameters.AddWithValue("@Dateadded", Dateadded);
                        command.Parameters.AddWithValue("@DateConnected", DateConnected);
                        command.Parameters.AddWithValue("@SourceId", SourceId);
                        command.Parameters.AddWithValue("@PersonId", PersonId);
                        command.Parameters.AddWithValue("@AddressId", AddressId);
                        command.ExecuteNonQuery();
                        LoadAllLead();
                    }

                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error While Updating Lead: " + ex.Message);
                        throw;
                    }

                }
            }
        }

        public void DeleteLead(int id)
        {
            LeadBL LeadToDelete = Leadlist.FirstOrDefault(e => e.Id == id);
            if (LeadToDelete != null)
            {
                Leadlist.Remove(LeadToDelete);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_DeleteLead", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                        LoadAllLead();
                    }

                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error While Deleting Lead: " + ex.Message);
                        throw;
                    }
                }
            }

        }
    }
}
