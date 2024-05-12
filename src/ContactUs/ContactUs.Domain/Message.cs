using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactUs.Domain
{
    public class Message
    {
        public int Id { get; set; }
        public string Email {  get; set; }

        public string Name { get; set; }

        public string User_Message { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
