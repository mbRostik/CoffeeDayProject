using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contracts.SendDTOs;
using Users.Application.UseCases.Queries;
using Users.Infrastructure.Data;

namespace Users.Application.UseCases.Handlers.QueryHandlers
{
    public class GetUserProfileHandler : IRequestHandler<GetUserProfileQuery, GetUserProfileDTO>
    {

        private readonly UserDbContext dbContext;

        public GetUserProfileHandler(UserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<GetUserProfileDTO> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"Start handling GetUserProfileQuery for UserId: {request.id}");

                var dbUser = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == request.id);

                if (dbUser == null)
                {
                    Console.WriteLine($"User with UserId: {request.id} not found");
                    return null;
                }

                Console.WriteLine($"User found: {dbUser.Name}");

                GetUserProfileDTO userInfo = new GetUserProfileDTO
                {
                    Name = dbUser.Name,
                    Email = dbUser.Email,
                    Preferences = dbUser.Preferences,
                    Photo = dbUser.Photo,
                    DateOfBirth = dbUser.DateOfBirth
                };

                Console.WriteLine($"Returning user profile for UserId: {request.id}");

                return userInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while handling GetUserProfileQuery for UserId: {request.id} - {ex}");
                return null;
            }
        }
    }
}