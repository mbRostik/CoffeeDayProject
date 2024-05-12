using ContactUs.Application.UseCases.Commands;
using ContactUs.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.UseCases.Commands;
using Users.Infrastructure.Data;

namespace ContactUs.Application.UseCases.Handlers.OperationHandlers
{
    public class CreateMessageHandler : IRequestHandler<CreateMessageCommand>
    {
        private readonly IMediator mediator;
        private readonly ContactUsDbContext dbContext;

        public CreateMessageHandler(ContactUsDbContext dbContext, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
        }

        public async Task Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Creating new message with details");

                Message message = new Message
                {
                    Name = request.model.Name,
                    Email = request.model.Email,
                    User_Message = request.model.User_Message,
                    UserId = request.model.UserId
                };

                var model = await dbContext.Messages.AddAsync(message, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                Console.WriteLine("Successfully created message");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while creating message");
                throw;
            }
        }
    }
}
