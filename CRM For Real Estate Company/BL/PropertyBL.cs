using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM_For_Real_Estate_Company.BL
{
    class PropertyBL
    {
        private int id;
        private decimal price;
        private int typeId;
        private int addressId;
        private int employeeId;
        private int statusId;
        private byte[] image;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        public int TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public int AddressId
        {
            get { return addressId; }
            set { addressId = value; }
        }

        public int EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; }
        }

        public int StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }

        public byte[] Image
        {
            get { return image; }
            set { image = value; }
        }

        public PropertyBL(decimal Price, int TypeId, int AddressId, int EmployeeId, int StatusId, byte[] Image)
        {
            this.Price = Price;
            this.TypeId = TypeId;
            this.AddressId = AddressId;
            this.EmployeeId = EmployeeId;
            this.StatusId = StatusId;
            this.Image = Image;
        }

        public PropertyBL(int Id, decimal Price, int TypeId, int AddressId, int EmployeeId, int StatusId, byte[] Image)
            : this(Price, TypeId, AddressId, EmployeeId, StatusId, Image)
        {
            this.Id = Id;
        }

        public PropertyBL()
        {

        }
    }
}
