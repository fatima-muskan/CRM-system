using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class PaymentsBL
    {
        private int id;
        private int paymentMethodID;
        private int buyerID;

        // Properties
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int PaymentMethodID
        {
            get { return paymentMethodID; }
            set { paymentMethodID = value; }
        }

        public int BuyerID
        {
            get { return buyerID; }
            set { buyerID = value; }
        }

        // Constructors
        public PaymentsBL()
        {
            // Default constructor
        }

        public PaymentsBL(int paymentMethodID, int buyerID)
        {
            this.paymentMethodID = paymentMethodID;
            this.buyerID = buyerID;
        }
        public PaymentsBL(int id, int paymentMethodID, int buyerID):this(paymentMethodID, buyerID)
        {
            this.id = id;
        }
    }
}
