using CRM_For_Real_Estate_Company.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.DL
{
    class PersonDL
    {
        private string connectionString;
        private Dictionary<int, PersonBL> personsDict = new Dictionary<int, PersonBL>();

        public PersonDL()
        {
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            LoadPersons();
        }
        public int SearchPersonId(string username, string password)
        {
            int personId = -1; // initialize personId to -1, which indicates no match was found
            foreach (KeyValuePair<int, PersonBL> kvp in personsDict)
            {
                PersonBL person = kvp.Value;
                if (person.Username == username && person.Password == password)
                {
                    personId = person.ID; // match found, set personId to the ID of the matched person
                    break; // exit loop once a match is found
                }
            }
            return personId;
        }

        private void LoadPersons()
        {
            personsDict.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetAllPersons", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PersonBL person = new PersonBL();
                    person.ID = Convert.ToInt32(reader["ID"]);
                    person.Username = reader["Username"].ToString();
                    person.Email = reader["Email"].ToString();
                    person.Password = reader["Password"].ToString();
                    person.RoleId = Convert.ToInt32(reader["RoleId"]);
                    person.MobileNumber = reader["MobileNumber"].ToString();
                    person.AddressId = Convert.ToInt32(reader["AddressId"]);
                    personsDict.Add(person.ID, person);
                }
                reader.Close();
            }
        }
        
        public List<PersonBL> GetAllPersons()
        {
            return personsDict.Values.ToList();
        }

        public int InsertPerson(PersonBL person, out string error)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_InsertPerson", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add input parameters
                    command.Parameters.AddWithValue("@Username", person.Username);
                    command.Parameters.AddWithValue("@Password", person.Password);
                    command.Parameters.AddWithValue("@RoleId", person.RoleId);
                    command.Parameters.AddWithValue("@AddressId", person.AddressId);
                    command.Parameters.AddWithValue("@Email", person.Email);
                    command.Parameters.AddWithValue("@MobileNumber", person.MobileNumber);

                    // Add output parameter
                    SqlParameter outputParameter = new SqlParameter();
                    outputParameter.ParameterName = "@PersonId";
                    outputParameter.SqlDbType = SqlDbType.Int;
                    outputParameter.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outputParameter);

                    try
                    {
                        command.ExecuteNonQuery();
                        person.ID = Convert.ToInt32(outputParameter.Value);
                        personsDict.Add(person.ID,person); // Add the inserted person to the list
                        error = null;
                        return person.ID;
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                        return 0;
                    }
                }

            }
        }
        public string SignIn(string username, string password)
        {
            foreach (PersonBL person in personsDict.Values)
            {
                if (person.Username == username && person.Password == password)
                {
                    RoleDL role = new RoleDL();
                    string roleName=role.SearchRole(person.RoleId);// Return the role if username and password match
                    if(roleName!=null)
                    {
                        return roleName;
                    }
                }
            }
            return null; // Return null if username and password do not match
        }
        public bool UpdatePasswordInDatabase(string username, string newPassword)
        {
            // Implement the logic to update password in the database for the given username
            // You can use SqlCommand or any other data manipulation mechanism here
            // Example:
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_UpdatePassword", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@NewPassword", newPassword);
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    return true; // Return true if password updated successfully in the database
                }
            }
            return false; // Return false if password not updated in the database
        }
        public int SerachPersonId(string username, string password)
        {
            foreach (PersonBL person in personsDict.Values)
            {
                if (person.Username == username && person.Password == password)
                {
                    return person.ID;
                }
            }
            return -1;
        }
        public bool UpdatePassword(string username, string newPassword)
        {
            PersonBL person = personsDict.Values.FirstOrDefault(p => p.Username == username);
            if (person != null)
            {
                person.Password = newPassword; // Update the password
                bool result = UpdatePasswordInDatabase(username, newPassword);
                return result; // Return true if password updated successfully
            }
            return false; // Return false if username not found
        }
    }
}
/*public bool UpdatePerson(string username, string email, string password, string name, string contactNumber, out string errorMessage)
        {
            bool result = true;
            errorMessage = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_UpdatePerson", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@ContactNumber", contactNumber);
                    command.ExecuteNonQuery();

                    // Update the hash table with the updated person data
                    foreach (var person in personsDict.Values)
                    {
                        if (person.Username == username)
                        {
                            person.Email = email;
                            person.Password = password;
                            person.Name = name;
                            person.ContactNumber = contactNumber;
                            break;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                result = false;
                errorMessage = ex.Message;
            }

            return result;
        }

        public bool DeletePerson(string username, out string errorMessage)
        {
            bool result = true;
            errorMessage = "";

            int personId = -1; // initialize to an invalid ID
            foreach (var person in personsDict.Values)
            {
                if (person.Username == username)
                {
                    personId = person.ID;
                    break;
                }
            }

            if (personId != -1)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_DeletePerson", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ID", personId);
                        command.ExecuteNonQuery();

                        // Update the dictionary by removing the deleted person data
                        personsDict.Remove(personId);
                    }
                }
                catch (SqlException ex)
                {
                    result = false;
                    errorMessage = ex.Message;
                }
            }
            else
            {
                result = false;
                errorMessage = "Person not found.";
            }

            return result;
        }
        public List<PersonBL> SearchPersons(string searchQuery, out string errorMessage)
        {
            List<PersonBL> persons = new List<PersonBL>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_SearchPersons", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@SearchQuery", searchQuery);

                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PersonBL person = new PersonBL();
                            person.ID = Convert.ToInt32(reader["ID"]);
                            person.Username = reader["Username"].ToString();
                            person.Email = reader["Email"].ToString();
                            person.Password = reader["Password"].ToString();
                            person.Name = reader["Name"].ToString();
                            person.ContactNumber = reader["ContactNumber"].ToString();
                            persons.Add(person);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    // Log or handle the exception as needed
                   errorMessage=("Error executing stored procedure: " + ex.Message);
                }
            }
            errorMessage = null;
            return persons;
        }*/