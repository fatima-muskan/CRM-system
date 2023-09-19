using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class RoleBL
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Constructor
        public RoleBL(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
