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
                Console.WriteLine($"Fetching products for CategoryId: {request.categoryId}");

                var categoryWithProductIds = await dbContext.CategoriesWithProducts
                    .Where(x => x.CategoryId == request.categoryId)
                    .Select(x => x.ProductId)
                    .ToArrayAsync();

                Console.WriteLine($"Found {categoryWithProductIds.Length} product IDs for CategoryId: {request.categoryId}");

                var products = await dbContext.Products
                    .Where(p => categoryWithProductIds.Contains(p.Id))
                    .ToListAsync();

                Console.WriteLine($"Fetched {products.Count} products for CategoryId: {request.categoryId}");

                return products;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while fetching products for CategoryId: {request.categoryId} - {ex}");
                return null;
            }
        }
    }
}