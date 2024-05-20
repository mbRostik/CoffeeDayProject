using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain
{
    public class Order_Products
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        public int Count { get; set; }

        public OrderHistory Order { get; set; }

        public Product Product { get; set; }

    }
}
