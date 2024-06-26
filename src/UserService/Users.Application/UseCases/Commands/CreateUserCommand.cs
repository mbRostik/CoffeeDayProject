﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain;

namespace Users.Application.UseCases.Commands
{
    public record CreateUserCommand(User model) : IRequest<User>;

}
