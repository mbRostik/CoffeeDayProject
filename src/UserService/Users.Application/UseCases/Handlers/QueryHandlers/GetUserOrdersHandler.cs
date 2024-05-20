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
                var orders = await dbContext.OrderHistories
                .Where(x => x.UserId == request.id)
                .ToListAsync();

                var orderIds = orders.Select(o => o.Id).ToList();

                var orderProducts = await dbContext.OrderProducts
                    .Where(op => orderIds.Contains(op.OrderId))
                    .ToListAsync();

                var productIds = orderProducts.Select(op => op.ProductId).ToList();

                var products = await dbContext.Products
                    .Where(p => productIds.Contains(p.Id))
                    .ToDictionaryAsync(p => p.Id, p => p.ProductPrice);

                var result = orders.Select(order =>
                {
                    var productsForOrder = orderProducts
                        .Where(op => op.OrderId == order.Id)
                        .ToList();

                    var summ = productsForOrder
                        .Sum(op => op.Count * products[op.ProductId]);

                    return new GetUserOrdersDTO
                    {
                        Id = order.Id,
                        Date = DateTime.Now,
                        Summ = summ
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
