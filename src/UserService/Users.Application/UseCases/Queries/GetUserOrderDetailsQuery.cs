using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contracts.GetDTOs;

namespace Users.Application.UseCases.Queries
{
    public record GetUserOrderDetailsQuery(int id) : IRequest<List<GetUserOrderDetailsDTO>>;


}
