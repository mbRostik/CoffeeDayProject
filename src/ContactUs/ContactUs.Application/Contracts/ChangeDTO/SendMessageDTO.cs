using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactUs.Application.Contracts.ChangeDTO
{
    public class SendMessageDTO
    {
        public string Email { get; set; } = "";

        public string Name { get; set; } = "";

        public string User_Message { get; set; }

        public string UserId { get; set; } = "";
    }
}
