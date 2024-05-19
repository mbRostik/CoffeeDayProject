using MediatR;
using Menu.Application.Contracts.ChangeDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.UseCases.Commands
{
    public record AddProductToTheBagCommand(AddProductToTheBagDTO model, string userId) : IRequest<bool>;
}
