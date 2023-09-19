using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class LeadBL
    {
        private int id;
        private DateTime dateadded;
        private DateTime dateConnected;
        private int sourceId;
        private int personId;

        public int Id { get => id; set => id = value; }
        public DateTime Dateadded { get => dateadded; set => dateadded = value; }
        public DateTime DateConnected { get => dateConnected; set => dateConnected = value; }
        public int SourceId { get => sourceId; set => sourceId = value; }
        public int PersonId { get => personId; set => personId = value; }

        public LeadBL()
        {

        }

        public LeadBL(DateTime Dateadded,DateTime DateConnected,int SourceId,int PersonId)
        {
            this.Dateadded = Dateadded;
            this.DateConnected= DateConnected;
            this.SourceId = SourceId;
            this.PersonId = PersonId;

        }

        public LeadBL(int Id,DateTime Dateadded, DateTime DateConnected, int SourceId,  int PersonId):this(Dateadded,DateConnected,SourceId,PersonId)
        {
            this.Id = Id;
        }

    }
}
