using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class EmployeeBL
    {
        
        private int id;
        private int personID;
        private int salary;

        // Properties with private backing fields
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public int Salary
        {
            get { return salary; }
            set { salary = value; }
        }
        public int PersonID
        {
            get { return personID; }
            set { personID = value; }
        }

        // Default constructor
        public EmployeeBL()
        {
        }
        public EmployeeBL(int personID,int salary)
        {
            this.personID = personID;
            this.Salary = salary;
        }
        // Constructor with parameters
        public EmployeeBL(int id, int personID,int salary) : this(personID, salary)
        {
            this.id = id;
        }
        
    }
}
