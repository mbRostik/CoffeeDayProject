using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Domain
{
    public class Bag
    {
        public string UserId { get; set; }

        public int ProductId {  get; set; }

        public int Count { get; set; }

        public User User { get; set; }

        public Product Product { get; set; }
    }
}
