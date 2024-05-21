using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contracts.GetDTOs;
using Users.Application.Contracts.SendDTOs;
using Users.Application.UseCases.Queries;
using Users.Infrastructure.Data;

namespace Users.Application.UseCases.Handlers.QueryHandlers
{
    public class GetUserOrdersHandler : IRequestHandler<GetUserOrdersQuery, List<GetUserOrdersDTO>>
    {

        private readonly UserDbContext dbContext;

        public GetUserOrdersHandler(UserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<GetUserOrdersDTO>> Handle(GetUserOrdersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"Start handling GetUserOrdersQuery for UserId: {request.id}");

                var orders = await dbContext.OrderHistories
                    .Where(x => x.UserId == request.id)
                    .ToListAsync();

                Console.WriteLine($"Fetched {orders.Count} orders for UserId: {request.id}");

                var orderIds = orders.Select(o => o.Id).ToList();

                var orderProducts = await dbContext.OrderProducts
                    .Where(op => orderIds.Contains(op.OrderId))
                    .ToListAsync();

                Console.WriteLine($"Fetched {orderProducts.Count} order products for orders of UserId: {request.id}");

                var productIds = orderProducts.Select(op => op.ProductId).ToList();

                var products = await dbContext.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id, p => p.ProductPrice);

                Console.WriteLine($"Fetched {products.Count} products associated with orders of UserId: {request.id}");

                var result = orders.Select(order =>
                {
                    var productsForOrder = orderProducts
                        .Where(op => op.OrderId == order.Id)
                        .ToList();

                    var summ = productsForOrder
                        .Sum(op => op.Count * products[op.ProductId]);

                    Console.WriteLine($"Processed order: {order.Id}, Total: {summ}");

                    return new GetUserOrdersDTO
                    {
                        Id = order.Id,
                        Date = order.Date, // Ensure correct date
                        Summ = summ
                    };
                }).ToList();

                Console.WriteLine($"Returning {result.Count} orders for UserId: {request.id}");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred while handling GetUserOrdersQuery for UserId: {request.id} - {ex}");
                return null;
            }
        }
    }
}
