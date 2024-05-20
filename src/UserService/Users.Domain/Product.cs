using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public byte[] ProductPhoto { get; set; }
        public float ProductPrice { get; set; }
        public List<Order_Products> Order_Products { get; set; }

    }
}
