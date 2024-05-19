using MediatR;
using Menu.Application.Contracts.ChangeDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.UseCases.Commands
{
    public record RemoveProductFromTheBagCommand(RemoveProductFromTheBagDTO model, string userId) : IRequest<bool>;
}
