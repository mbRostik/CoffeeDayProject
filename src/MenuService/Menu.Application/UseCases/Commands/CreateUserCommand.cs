using MediatR;
using Menu.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.UseCases.Commands
{
    public record CreateUserCommand(User model) : IRequest<User>;

}
