using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM_For_Real_Estate_Company.BL;
using System.Data.SqlClient;

namespace CRM_For_Real_Estate_Company.DL
{
    class PriorityDL
    {
        // Connection string
        private string connectionString;
        private List<PriorityBL> priorityList = new List<PriorityBL>();

        public PriorityDL()
        {
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            LoadPriority();
        }

        // Function to load data from the database and return a list of Priority objects
        public List<PriorityBL> LoadPriority()
        {
            priorityList.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id,name,range FROM PriorityLevel"; // Update with your actual table name and column names
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            int range = reader.GetInt32(2);
                            PriorityBL p = new PriorityBL(id, name,range);
                            priorityList.Add(p);
                        }
                    }
                }
            }

            return priorityList;
        }
    }
}
