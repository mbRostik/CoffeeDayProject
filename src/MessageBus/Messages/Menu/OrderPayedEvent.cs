using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.Messages.Menu
{
    public class OrderPayedEvent : IntegrationBaseEvent
    {
       public string UserId { get; set; }
       public List<Order_Product> Order_Products { get; set; } = new List<Order_Product>();
    }

    public class Order_Product
    {
        public string ProductName { get; set;}
        public string ProductDescription { get; set; }
        public byte[] ProductPhoto { get; set;}
        public float ProductPrice { get; set;} 
        public int ProductCount { get; set;}
    }
}
