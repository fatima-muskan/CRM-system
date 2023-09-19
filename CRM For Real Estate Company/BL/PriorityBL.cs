using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class PriorityBL
    {
        private int id;
        private string name;
        private int range;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Range { get => range; set => range = value; }

        public PriorityBL()
        {

        }

        public PriorityBL(int Id,string Name,int Range)
        {
            this.Id = Id;
            this.Name = Name;
            this.Range = Range;
        }

        
    }
}
