using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.UseCases.Commands
{
  
    public record PayOrderCommand(string userId) : IRequest<bool>;

}
