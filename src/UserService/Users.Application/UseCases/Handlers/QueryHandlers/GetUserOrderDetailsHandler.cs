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
                var orderProducts = await dbContext.OrderProducts
                    .Where(op => op.OrderId == request.id)
                    .ToListAsync();

                var productIds = orderProducts.Select(op => op.ProductId).ToList();

                var products = await dbContext.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToListAsync();

                var result = products.Select(product =>
                {
                    var orderProduct = orderProducts.First(op => op.ProductId == product.Id);
                    return new GetUserOrderDetailsDTO
                    {
                        ProductDescription = product.ProductDescription,
                        ProductName = product.ProductName,
                        ProductPrice = product.ProductPrice,
                        ProductPhoto = product.ProductPhoto,
                        Count = orderProduct.Count
                    };
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }
    }
}
