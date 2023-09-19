using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class OwnerBL
    {
        private int propertyID;
        private int id;
        private int personId;
        private int paymentId;
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

        public int PersonId { get => personId; set => personId = value; }
        public int PaymentId { get => paymentId; set => paymentId = value; }

        // Constructor
        public OwnerBL(int propertyID,int PersonId,int PaymentId)
        {
            this.propertyID = propertyID;
            this.personId = PersonId;
        }
        public OwnerBL(int id,int propertyID, int PersonId, int PaymentId) : this(propertyID,PersonId, PaymentId)
        {
            this.id = id;
        }
        public OwnerBL()
        {

        }

    }
}
