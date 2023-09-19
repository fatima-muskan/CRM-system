using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.DL
{
    class buyerBL
    {
        private int leadID;
        private int propertyID;
        private int id;

        // Properties for public access
        public int LeadID
        {
            get { return leadID; }
            set { leadID = value; }
        }

        public int PropertyID
        {
            get { return propertyID; }
            set { propertyID = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        // Constructor
        public buyerBL(int leadID, int propertyID)
        {
            this.leadID = leadID;
            this.propertyID = propertyID;
        }
        public buyerBL(int leadID, int propertyID, int id):this(leadID,propertyID)
        {
            this.id = id;
        }
        public buyerBL()
        {

        }
    }
}
