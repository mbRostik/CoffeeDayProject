using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contracts.SendDTOs;

namespace Users.Application.UseCases.Queries
{
    public record GetUserProfileQuery(string id) : IRequest<GetUserProfileDTO>;

}
