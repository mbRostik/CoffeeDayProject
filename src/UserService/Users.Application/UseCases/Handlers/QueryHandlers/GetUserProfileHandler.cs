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
               
                var dbUser = await dbContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == request.id);

           
                GetUserProfileDTO userInfo = new GetUserProfileDTO 
                {
                    Name= dbUser.Name,
                    Email= dbUser.Email,
                    Preferences = dbUser.Preferences,
                    Photo = dbUser.Photo,
                    DateOfBirth = dbUser.DateOfBirth
                };

              
                return userInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }
    }
}
