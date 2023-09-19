using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class ResponseBL
    {
        private int id;
        private string name;
        private string description;
        private int leadId;
        private int taskId;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Description { get => description; set => description = value; }
        public int LeadId { get => leadId; set => leadId = value; }
        public int TaskId { get => taskId; set => taskId = value; }

        public ResponseBL()
        {

        }

        public ResponseBL(string Name,string Description,int LeadId,int TaskId)
        {
            this.Name = Name;
            this.Description = Description;
            this.LeadId = LeadId;
            this.TaskId = TaskId;

        }

        public ResponseBL(int id,string Name, string Description, int LeadId, int TaskId):this(Name,Description,LeadId,TaskId)
        {
            this.Id=id;
        }

    }
}
