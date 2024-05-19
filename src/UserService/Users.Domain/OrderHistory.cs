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

        public string OrderId { get; set; }

        public User User { get; set; }
    }
}
