﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBus.Messages.User
{
    public class UserCreationEvent: IntegrationBaseEvent
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }
    }
}
