using MassTransit;
using MediatR;
using MessageBus.Messages.Menu;
using MessageBus.Messages.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.UseCases.Commands;

namespace Users.Application.UseCases.Consumers
{
    public class OrderPayedConsumer : IConsumer<OrderPayedEvent>
    {
        private readonly IMediator mediator;
        public OrderPayedConsumer(IMediator _mediator)
        {
            mediator = _mediator;
        }
        public async Task Consume(ConsumeContext<OrderPayedEvent> context)
        {
            Console.WriteLine($"Successfully consumed OrderPayedEvent");

            await mediator.Send(new AddOrderCommand(context.Message));
        }
    }
}
