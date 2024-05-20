using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Domain
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Preferences { get; set; }

        public byte[] Photo { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int SexId { get; set; }

        public bool IsBanned { get; set; } = false;

        public List<OrderHistory> OrderHistory { get; set; }
    }
}
