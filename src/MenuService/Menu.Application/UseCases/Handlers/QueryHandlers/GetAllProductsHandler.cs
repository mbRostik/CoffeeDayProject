using MediatR;
using Menu.Application.UseCases.Queries;
using Menu.Domain;
using Menu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.UseCases.Handlers.QueryHandlers
{
    internal class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, List<Product>>
    {

        private readonly MenuDbContext dbContext;

        public GetAllProductsHandler(MenuDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await dbContext.Products.ToListAsync(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
