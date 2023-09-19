using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class TaskBL
    {
        private int id;
        private string title;
        private string description;
        private DateTime dueDate;
        private int employeeId;
        private int prioritylevel;
        private int assignedId;

        public int Id { get => id; set => id = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public int EmployeeId { get => employeeId; set => employeeId = value; }
        public int Prioritylevel { get => prioritylevel; set => prioritylevel = value; }
        public int AssignedId { get => assignedId; set => assignedId = value; }

        public TaskBL()
        {

        }

        public TaskBL(string Title,string Description,DateTime DueDate,int EmployeeId,int Prioritylevel,int AssignedId)
        {
            this.Title = Title;
            this.Description = Description;
            this.DueDate = DueDate;
            this.EmployeeId = EmployeeId;
            this.Prioritylevel = Prioritylevel;
            this.AssignedId = AssignedId;
        }

        public TaskBL(int Id,string Title, string Description, DateTime DueDate, int EmployeeId, int PriorityLevel,int AssignedId):this(Title,Description,DueDate,EmployeeId,PriorityLevel,AssignedId)
        {
            this.Id=Id;
        }
    }
}
