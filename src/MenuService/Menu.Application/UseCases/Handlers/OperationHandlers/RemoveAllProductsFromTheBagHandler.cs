using MediatR;
using Menu.Application.UseCases.Commands;
using Menu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.UseCases.Handlers.OperationHandlers
{
    public class RemoveAllProductsFromTheBagHandler : IRequestHandler<RemoveAllProductsFromTheBagCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly MenuDbContext dbContext;

        public RemoveAllProductsFromTheBagHandler(MenuDbContext dbContext, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(RemoveAllProductsFromTheBagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"Start handling RemoveAllProductsFromTheBagCommand for UserId: {request.userId}, ProductId: {request.model.ProductId}");

                var isTheProductInTheBag = await dbContext.Bags.FirstOrDefaultAsync(
                    x => x.UserId == request.userId && x.ProductId == request.model.ProductId);

                if (isTheProductInTheBag == null)
                {
                    Console.WriteLine("Product not found in the bag.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Product found in the bag. Removing...");

                    dbContext.Bags.Remove(isTheProductInTheBag);
                    await dbContext.SaveChangesAsync();

                    Console.WriteLine("Product removed from the bag and changes saved.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while removing product from the bag for UserId: {request.userId}, ProductId: {request.model.ProductId} - {ex}");
                return false;
            }
        }
    }
}

