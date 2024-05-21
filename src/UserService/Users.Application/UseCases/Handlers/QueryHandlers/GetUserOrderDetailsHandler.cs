using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contracts.GetDTOs;
using Users.Application.UseCases.Queries;
using Users.Infrastructure.Data;

namespace Users.Application.UseCases.Handlers.QueryHandlers
{
    public class GetUserOrderDetailsHandler : IRequestHandler<GetUserOrderDetailsQuery, List<GetUserOrderDetailsDTO>>
    {

        private readonly UserDbContext dbContext;

        public GetUserOrderDetailsHandler(UserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<GetUserOrderDetailsDTO>> Handle(GetUserOrderDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"Start handling GetUserOrderDetailsQuery for OrderId: {request.id}");

                var orderProducts = await dbContext.OrderProducts
                    .Where(op => op.OrderId == request.id)
                    .ToListAsync();

                Console.WriteLine($"Fetched {orderProducts.Count} order products for OrderId: {request.id}");

                var productIds = orderProducts.Select(op => op.ProductId).ToList();

                var products = await dbContext.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

                Console.WriteLine($"Fetched {products.Count} products associated with OrderId: {request.id}");

                var result = products.Select(product =>
                {
                    var orderProduct = orderProducts.First(op => op.ProductId == product.Id);
                    Console.WriteLine($"Processing product: {product.ProductName}, Count: {orderProduct.Count}");

                    return new GetUserOrderDetailsDTO
                    {
                        ProductDescription = product.ProductDescription,
                        ProductName = product.ProductName,
                        ProductPrice = product.ProductPrice,
                        ProductPhoto = product.ProductPhoto,
                        Count = orderProduct.Count
                    };
                }).ToList();

                Console.WriteLine($"Returning {result.Count} order details for OrderId: {request.id}");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while handling GetUserOrderDetailsQuery for OrderId: {request.id} - {ex}");
                return null;
            }
        }
    }
}