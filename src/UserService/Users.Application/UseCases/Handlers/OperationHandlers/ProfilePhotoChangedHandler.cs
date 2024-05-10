using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contracts.SendDTOs;
using Users.Application.UseCases.Commands;
using Users.Application.UseCases.Queries;
using Users.Infrastructure.Data;

namespace Users.Application.UseCases.Handlers.OperationHandlers
{
    public class ProfilePhotoChangedHandler : IRequestHandler<ChangeUserAvatarCommand, GetUserProfileDTO>
    {
        private readonly IMediator mediator;

        private readonly UserDbContext dbContext;

        public ProfilePhotoChangedHandler(UserDbContext dbContext, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
        }

        public async Task<GetUserProfileDTO> Handle(ChangeUserAvatarCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Attempting to change avatar for user ID");

                var user = await dbContext.Users.FindAsync(new object[] { request.model.Id }, cancellationToken);
                if (user != null)
                {
                    user.Photo = Convert.FromBase64String(request.model.Avatar);
                    await dbContext.SaveChangesAsync();

                    Console.WriteLine("Avatar changed successfully for user ID. Fetching updated user profile.");

                    var result = await mediator.Send(new GetUserProfileQuery(request.model.Id), cancellationToken);

                    if (result == null)
                    {
                        Console.WriteLine("Updated user profile for user ID not found after changing avatar.");
                        return null;
                    }

                    return result;
                }
                else
                {
                    Console.WriteLine("User with ID not found. Cannot change avatar.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while changing avatar for user ID" + ex);
                throw;
            }
        }
    }
}
