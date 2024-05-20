using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain
{
    public class OrderHistory
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public DateTime Date { get; set; }= DateTime.Now;
        public User User { get; set; }

        public List<Order_Products> Order_Products { get; set; }
    }
}
