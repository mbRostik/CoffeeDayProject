using MediatR;
using Menu.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.UseCases.Queries
{
    public record GetAllProductsQuery() : IRequest<List<Product>>;

}
