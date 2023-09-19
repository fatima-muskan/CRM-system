using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class AddressBL
    {
        private int id;
        private string country;
        private string state;
        private string streetName;
        private string city;

        // Constructor
        public AddressBL()
        {
            // Default constructor
        }

        public AddressBL(string country, string state, string streetName, string city)
        {
            this.country = country;
            this.state = state;
            this.streetName = streetName;
            this.city = city;
        }
        public AddressBL(int id, string country, string state, string streetName, string city):this(country, state, streetName, city)
        {
            this.id = id;
        }
        // Properties
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        public string StreetName
        {
            get { return streetName; }
            set { streetName = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }
    }
}
