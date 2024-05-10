using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contracts.ChangeDTOs;
using Users.Infrastructure.Data;

namespace Users.Application.UseCases.Handlers.OperationHandlers
{
    public class UserInformationChangedHandler : IRequestHandler<ChangeUserInformationCommand>
    {
        private readonly IMediator mediator;

        private readonly UserDbContext dbContext;

        public UserInformationChangedHandler(UserDbContext dbContext, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
        }
        public async Task Handle(ChangeUserInformationCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Attempting to change user information for User");

            try
            {
                var userInDb = await dbContext.Users.FindAsync(new object[] { request.model.Id }, cancellationToken);

                if (userInDb == null)
                {
                    Console.WriteLine("User with User not found");
                    return;
                }

                userInDb.Name = request.model.Name;
                userInDb.Preferences = request.model.Preferences;
                userInDb.DateOfBirth = request.model.DateOfBirth;

                await dbContext.SaveChangesAsync(cancellationToken);

                Console.WriteLine("User information successfully changed for User");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + "Error occurred while changing user information for User");
                throw;
            }
        }
    }
}
