using MediatR;
using Menu.Application.Contracts.GetDTOs;
using Menu.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.UseCases.Queries
{
    public record GetUserBagQuery(string userId) : IRequest<List<GetUserBagDTO>>;

}
