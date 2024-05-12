using ContactUs.Domain;
using MassTransit;
using MediatR;
using MessageBus.Messages.User;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.UseCases.Commands;

namespace Users.Application.UseCases.Consumers
{
    public class UserCreation_Consumer : IConsumer<UserCreationEvent>
    {
        private readonly IMediator mediator;
        public UserCreation_Consumer(IMediator _mediator)
        {
            mediator = _mediator;
        }
        public async Task Consume(ConsumeContext<UserCreationEvent> context)
        {
            Console.WriteLine($"Successfully consumed UserCreationEvent");

            User temp = new User
            { 
                Id = context.Message.UserId, 
                Email=context.Message.Email
            };
            await mediator.Send(new CreateUserCommand(temp));
        }
    }
}
