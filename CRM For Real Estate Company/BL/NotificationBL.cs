using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class NotificationBL
    {
        private int rid;
        private string title;
        private string description;
        private DateTime duedate;

        public int Rid { get => rid; set => rid = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public DateTime Duedate { get => duedate; set => duedate = value; }

        public NotificationBL()
        {

        }
        public NotificationBL(int rid, string title, string description, DateTime duedate )
        {
            this.rid = rid;
            this.title = title;
            this.description = description;
            this.duedate = duedate;
        }
    }
}
