using MediatR;
using Menu.Application.UseCases.Queries;
using Menu.Domain;
using Menu.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.UseCases.Handlers.QueryHandlers
{
    public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategoryQuery, List<Product>>
    {

        private readonly MenuDbContext dbContext;

        public GetProductsByCategoryHandler(MenuDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Product>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var categoryWithProductIds = dbContext.CategoriesWithProducts
                    .Where(x => x.CategoryId == request.categoryId)
                    .Select(x => x.ProductId)
                    .ToArray();

                var products = dbContext.Products
                    .Where(p => categoryWithProductIds.Contains(p.Id))
                    .ToList();
                Console.WriteLine();
                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}