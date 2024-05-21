using MediatR;
using Menu.Application.Contracts.GetDTOs;
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
    public class GetUserBagHandler : IRequestHandler<GetUserBagQuery, List<GetUserBagDTO>>
    {

        private readonly MenuDbContext dbContext;

        public GetUserBagHandler(MenuDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<GetUserBagDTO>> Handle(GetUserBagQuery request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"Fetching bag items for UserId: {request.userId}");

                var result = await dbContext.Bags
                    .Where(b => b.UserId == request.userId)
                    .Join(
                        dbContext.Products,
                        bag => bag.ProductId,
                        product => product.Id,
                        (bag, product) => new GetUserBagDTO
                        {
                            ProductId = product.Id,
                            Name = product.Name,
                            Description = product.Description,
                            Price = product.Price,
                            Count = bag.Count
                        })
                    .ToListAsync(cancellationToken);

                Console.WriteLine($"Fetched {result.Count} bag items for UserId: {request.userId}");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while fetching bag items for UserId: {request.userId} - {ex}");
                return null;
            }
        }
    }
}