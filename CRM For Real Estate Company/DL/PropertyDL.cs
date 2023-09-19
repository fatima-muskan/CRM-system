using CRM_For_Real_Estate_Company.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;

namespace CRM_For_Real_Estate_Company.DL
{
    class PropertyDL
    {
        private string connectionString;
        private List<PropertyBL> propertylist = new List<PropertyBL>();

        public List<PropertyBL> Propertylist { get => propertylist; set => propertylist = value; }

        public PropertyDL()
        {
            connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
            LoadAllProperty();
        }
        public List<PropertyBL> GetPropertiesForLeadId(int leadId)
        {
            List<PropertyBL> properties = new List<PropertyBL>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Property AS p where p.id not in (select propertyid from Sales)";

                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        PropertyBL property = new PropertyBL();
                        property.Id = (int)reader["id"];
                        property.Price = (decimal)reader["price"];
                        property.TypeId = (int)reader["typeid"];
                        property.AddressId = (int)reader["addressid"];
                        property.StatusId = (int)reader["statusid"];
                        property.Image = (byte[])reader["image"];

                        properties.Add(property);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return properties;
        }

        public PropertyBL GetPropertyById(int Id)
        {
            PropertyBL property = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Property AS p where p.id not in (select propertyid from Sales) and Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", Id);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    property = new PropertyBL();
                    if (reader.Read())
                    {
                        property.Id = (int)reader["id"];
                        property.TypeId = (int)reader["typeid"];
                        property.StatusId = (int)reader["statusid"];
                        property.Price = (decimal)reader["price"];
                        property.AddressId = (int)reader["addressid"];
                        property.EmployeeId = (int)reader["employeeid"];
                    }
                }
                conn.Close();
            }

            return property;
        }
        public bool UpdatePropertyStatus(int propertyId, int statusId)
        {
            bool success = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "UPDATE Property SET StatusId = @StatusId WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", propertyId);
                cmd.Parameters.AddWithValue("@StatusId", statusId);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    success = true;
                }
                conn.Close();
            }

            return success;
        }

        public int GetTheAddressId(int propertyid)
        {
            int addressId = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "select addressid from Property where Property.id=@id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", propertyid);

                object result = command.ExecuteScalar();
                if (result != null)
                {
                    addressId = (int)result;
                }

                connection.Close();
            }
            return addressId;
        }
        public bool UpdateProperty(int id, int typeId, int statusId, decimal price, int addressId)
        {
            bool flag = false;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();
                try
                {
                    string query = "UPDATE Property " +
                                   "SET typeId = @TypeId, statusId = @StatusId, price = @Price, addressId = @AddressId " +
                                   "WHERE id = @Id";

                    SqlCommand command = new SqlCommand(query, connection, transaction);
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@TypeId", typeId);
                    command.Parameters.AddWithValue("@StatusId", statusId);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@AddressId", addressId);
                    command.ExecuteNonQuery();

                    // Perform another update here

                    transaction.Commit();
                    flag = true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    LoadAllProperty();
                }
            }
            return flag;
        }

        public List<PropertyBL> GetAllPropertiesWithImages(int personId)
        {
            List<PropertyBL> properties = new List<PropertyBL>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT p.id, p.price, p.typeId, p.addressId, p.employeeId, p.statusId, p.image, o.id AS ownerId " +
                               "FROM Property p " +
                               "INNER JOIN Owner o ON p.id = o.propertyId " +
                               "INNER JOIN Person pe ON pe.id = o.personId " +
                               "WHERE pe.id = @PersonId and p.id not in (Select propertyid from Sales)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonId", personId);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PropertyBL property = new PropertyBL();
                    property.Id = reader.GetInt32(0);
                    property.Price = reader.GetDecimal(1);
                    property.TypeId = reader.GetInt32(2);
                    property.AddressId = reader.GetInt32(3);
                    property.EmployeeId = reader.GetInt32(4);
                    property.StatusId = reader.GetInt32(5);

                    if (!reader.IsDBNull(6))
                    {
                        property.Image = (byte[])reader["image"];
                    }

                    properties.Add(property);
                }

                reader.Close();
            }

            return properties;
        }

        public int InsertProperty(int price, int addressId, int statusId, int typeId, int employeeid, byte[] image)
        {
            int propertyId = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_InsertProperty", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    // Assuming 'price' is an int value
                    decimal priceDecimal =(price)/100.0m; // Convert int to decimal
                    // Pass the 'priceDecimal' value as parameter to the stored procedure
                    command.Parameters.AddWithValue("@Price", priceDecimal);
                    command.Parameters.AddWithValue("@AddressId", addressId);
                    command.Parameters.AddWithValue("@StatusId", statusId);
                    command.Parameters.AddWithValue("@TypeId", typeId);
                    command.Parameters.AddWithValue("@EmployeeId", employeeid);

                    // Set the Size property for the @Image parameter
                    command.Parameters.Add("@Image", SqlDbType.VarBinary, -1).Value = image;

                    SqlParameter propertyIdParam = new SqlParameter("@PropertyId", SqlDbType.Int);
                    propertyIdParam.Direction = ParameterDirection.Output;
                    propertyIdParam.Size = sizeof(int);
                    command.Parameters.Add(propertyIdParam); // Add the output parameter to the SqlCommand object

                command.ExecuteNonQuery();
                    LoadAllProperty();
                    propertyId = (int)command.Parameters["@PropertyId"].Value; // Get the value of the output parameter
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error inserting Property: " + ex.Message);
                    throw;
                }
                connection.Close();
            }
            return propertyId; // Return the ID of the inserted property
        }
        public void UpdateProperty(int id, int price, int addressId, int statusId, int typeId, int employeeid, byte[] image)
        {
            PropertyBL PropertyToUpdate = propertylist.FirstOrDefault(e => e.Id == id);
            if (PropertyToUpdate != null)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_UpdateProperty", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@AddressId", addressId);
                        command.Parameters.AddWithValue("@StatusId", statusId);
                        command.Parameters.AddWithValue("@TypeId", typeId);
                        command.Parameters.AddWithValue("@employeeid", employeeid);
                        command.Parameters.AddWithValue("@image", image);
                        command.ExecuteNonQuery();
                        LoadAllProperty();
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine("Error While Updating Property: " + ex.Message);
                        throw;
                    }

                }
            }
        }

        private void LoadAllProperty()
        {
            propertylist.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_GetAllProperties", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PropertyBL property = new PropertyBL();
                            property.Id = Convert.ToInt32(reader["Id"]);
                            property.Price = Convert.ToDecimal(reader["Price"]);
                            property.AddressId = Convert.ToInt32(reader["AddressId"]);
                            property.StatusId = Convert.ToInt32(reader["StatusId"]);
                            property.EmployeeId = Convert.ToInt32(reader["employeeid"]);
                            property.TypeId = Convert.ToInt32(reader["TypeId"]);
                            // Convert image to base64 string
                            byte[] imageBytes = (byte[])reader["image"];
                            property.Image = imageBytes;
                            propertylist.Add(property);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception as per your application's requirement
                    Console.WriteLine("Error loading properties: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void DeleteProperty(int Id)
        {
            PropertyBL PropertyToDelete = propertylist.FirstOrDefault(e => e.Id == Id);
            if (PropertyToDelete != null)
            {
                propertylist.Remove(PropertyToDelete);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_DeleteProperty", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", Id);
                        command.ExecuteNonQuery();
                        LoadAllProperty();
                    }
                    catch (SqlException ex)
                    {

                        Console.WriteLine("Error While Deleting Property: " + ex.Message);
                        throw;
                    }
                }
            }
        }
    }
}