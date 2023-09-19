using CRM_For_Real_Estate_Company.BL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

class AddressDL
{
    private List<AddressBL> addressList;
    private string connectionString;

    // Constructor
    public AddressDL()
    {
        connectionString = @"Data Source=(local);Initial Catalog=FinalProject;Integrated Security=True";
        addressList = new List<AddressBL>();
        LoadAddresses(); // Load addresses from database in constructor
    }
    public AddressBL GetAddressByIdfromList(int id)
    {
        return addressList.FirstOrDefault(s => s.ID == id);
    }
    // Load addresses from database
    public AddressBL GetAddressById(int id)
    {
        AddressBL address = null;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string query = "SELECT * FROM Address WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    address = new AddressBL
                    {
                        ID = (int)reader["id"],
                        StreetName = (string)reader["streetName"],
                        City = (string)reader["city"],
                        State = (string)reader["state"],
                        Country = (string)reader["country"]
                    };
                }
            }
        }

        return address;
    }

    private void LoadAddresses()
    {
        addressList.Clear();
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_LoadAddresses", connection);
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AddressBL address = new AddressBL();
                        address.ID = Convert.ToInt32(reader["ID"]);
                        address.Country = reader["Country"].ToString();
                        address.State = reader["State"].ToString();
                        address.StreetName = reader["StreetName"].ToString();
                        address.City = reader["City"].ToString();
                        // Set other properties of the AddressBL object as needed
                        addressList.Add(address);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception as needed, e.g. log or throw
            Console.WriteLine("Error loading addresses from database: " + ex.Message);
        }
    }
    public bool UpdateAddressForProperty(int propertyId, string newCountry, string newState, string newStreetName, string newCity)
    {
        bool result = false;
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Begin a new transaction
            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                // Get the current address ID for the property
                string getAddressIdQuery = "SELECT addressId FROM Property WHERE id = @PropertyId";
                SqlCommand getAddressIdCommand = new SqlCommand(getAddressIdQuery, connection, transaction);
                getAddressIdCommand.Parameters.AddWithValue("@PropertyId", propertyId);
                int addressId = (int)getAddressIdCommand.ExecuteScalar();

                // Update the address
                string updateAddressQuery = "UPDATE Address SET country = @NewCountry, state = @NewState, streetName = @NewStreetName, city = @NewCity WHERE id = @AddressId";
                SqlCommand updateAddressCommand = new SqlCommand(updateAddressQuery, connection, transaction);
                updateAddressCommand.Parameters.AddWithValue("@NewCountry", newCountry);
                updateAddressCommand.Parameters.AddWithValue("@NewState", newState);
                updateAddressCommand.Parameters.AddWithValue("@NewStreetName", newStreetName);
                updateAddressCommand.Parameters.AddWithValue("@NewCity", newCity);
                updateAddressCommand.Parameters.AddWithValue("@AddressId", addressId);
                updateAddressCommand.ExecuteNonQuery();

                // Commit the transaction if everything succeeded
                transaction.Commit();
                connection.Close();
                LoadAddresses();
                result = true;
            }
            catch (Exception ex)
            {
                // If anything went wrong, roll back the transaction and log the error
                transaction.Rollback();
                connection.Close();
                Console.WriteLine("Transaction failed: " + ex.Message);
                result = false;
            }
        }
        return result;
    }

    // Insert a new address
    public bool InsertAddress(AddressBL address, out int addressId, out string error)
    {
        addressId = 0;
        error = "";
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_InsertAddress", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Country", address.Country);
                command.Parameters.AddWithValue("@State", address.State);
                command.Parameters.AddWithValue("@StreetName", address.StreetName);
                command.Parameters.AddWithValue("@City", address.City);

                // Add output parameter for addressId
                SqlParameter addressIdParam = new SqlParameter("@AddressId", SqlDbType.Int);
                addressIdParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(addressIdParam);

                // Execute the stored procedure
                command.ExecuteNonQuery();

                // Retrieve the newly created address ID from the output parameter
                addressId = (int)command.Parameters["@AddressId"].Value;
                return true;
            }
        }
        catch (Exception ex)
        {
            // Handle exception and set error message
            error = "Error inserting address into database: " + ex.Message;
            return false;
        }
        error = "Error inserting address into database";
        return false;
    }
    // Update an existing address
    public void UpdateAddress(AddressBL previousUpdate, AddressBL newUpdate, out string error)
    {
        error = "";
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_UpdateAddress", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", previousUpdate.ID);
                command.Parameters.AddWithValue("@NewCountry", newUpdate.Country);
                command.Parameters.AddWithValue("@NewState", newUpdate.State);
                command.Parameters.AddWithValue("@NewStreetName", newUpdate.StreetName);
                command.Parameters.AddWithValue("@NewCity", newUpdate.City);

                // Execute the stored procedure
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            // Handle exception and set error message
            error = "Error updating address in database: " + ex.Message;
        }
    }

    // Delete an address
    public void DeleteAddress(AddressBL address, out string error)
    {
        error = "";
        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_DeleteAddress", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ID", address.ID);

                // Execute the stored procedure
                command.ExecuteNonQuery();
                addressList.Remove(address); // Remove address from local list
            }
        }
        catch (Exception ex)
        {
            // Handle exception and set error message
            error = "Error deleting address from database: " + ex;

        }
    }
}