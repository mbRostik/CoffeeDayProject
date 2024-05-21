using MediatR;
using Menu.Application.UseCases.Commands;
using Menu.Domain;
using Menu.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.UseCases.Handlers.OperationHandlers
{
    public class UserCreatedHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly IMediator mediator;
        private readonly MenuDbContext dbContext;

        public UserCreatedHandler(MenuDbContext dbContext, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
        }

        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Creating new user with details: ");

                var model = await dbContext.Users.AddAsync(request.model, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);

                Console.WriteLine($"Successfully created user with ID: {model.Entity.Id}");

                return model.Entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while creating user: {ex}");
                throw;
            }
        }
    }
}