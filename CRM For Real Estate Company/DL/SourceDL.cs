using CRM_For_Real_Estate_Company.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.DL
{
    class SourceDL
    {
        private List<SourceBL> sources = new List<SourceBL>();
        public List<SourceBL> GetSources()
        {
            string connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM Source", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SourceBL source = new SourceBL();
                    source.Id = Convert.ToInt32(reader["Id"]);
                    source.Name = reader["Name"].ToString();
                    sources.Add(source);
                }
                reader.Close();
            }
            return sources;
        }
    }
}
