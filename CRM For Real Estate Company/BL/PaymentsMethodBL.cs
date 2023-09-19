using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class PaymentsMethodBL
    {
        private int ID;
        private string bankName;
        private char[] accountNo;
        private string Method;

        public int PaymentID
        {
            get { return ID; }
            set { ID = value; }
        }

        public string BankName
        {
            get { return bankName; }
            set { bankName = value; }
        }

        public char[] AccountNo
        {
            get { return accountNo; }
            set { accountNo = value; }
        }

        public string PaymentMethod
        {
            get { return Method; }
            set { Method = value; }
        }

        public PaymentsMethodBL()
        {
            // Default constructor
        }

        public PaymentsMethodBL(string bankName, char[] accountNo, string method)
        {
            BankName = bankName;
            AccountNo = accountNo;
            Method = method;
        }

        public PaymentsMethodBL(int id, string bankName, char[] accountNo, string method)
            : this(bankName, accountNo, method)
        {
            ID = id;
        }
    }
}
