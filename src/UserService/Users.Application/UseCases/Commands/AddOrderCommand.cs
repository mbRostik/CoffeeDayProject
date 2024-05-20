using MediatR;
using MessageBus.Messages.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contracts.SendDTOs;

namespace Users.Application.UseCases.Commands
{
    public record AddOrderCommand(OrderPayedEvent model) : IRequest;

}
