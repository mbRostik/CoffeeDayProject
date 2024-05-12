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
    public class UserCreatedHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IMediator mediator;
        private readonly ContactUsDbContext dbContext;

        public UserCreatedHandler(ContactUsDbContext dbContext, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
        }

        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Creating new user with details");

                var model = await dbContext.Users.AddAsync(request.model, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                Console.WriteLine("Successfully created user with ID");

                return model.Entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while creating user");
                throw;
            }
        }
    }
}
