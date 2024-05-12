using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactUs.Domain
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public List<Message> Messages { get; set; }
    }
}
