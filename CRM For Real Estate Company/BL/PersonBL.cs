using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class PersonBL
    {
        private int id;
        private string username;
        private string email;
        private string password;
        private int roleId;
        private int addressId;
        private string mobileNumber;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        public int AddressId
        {
            get { return addressId; }
            set { addressId = value; }
        }

        public string MobileNumber
        {
            get { return mobileNumber; }
            set { mobileNumber = value; }
        }

        // Constructor with parameters
        public PersonBL(string username, string email, string password, int roleId, int addressId, string mobileNumber)
        {
            this.username = username;
            this.email = email;
            this.password = password;
            this.roleId = roleId;
            this.addressId = addressId;
            this.mobileNumber = mobileNumber;
        }

        // Constructor with ID parameter
        public PersonBL(int id, string username, string email, string password, int roleId, int addressId, string mobileNumber)
            : this(username, email, password, roleId, addressId, mobileNumber)
        {
            this.ID = id;
        }

        // Default constructor
        public PersonBL()
        {
        }
    }
}
