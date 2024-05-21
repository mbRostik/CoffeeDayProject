using MediatR;
using Menu.Application.UseCases.Commands;
using Menu.Domain;
using Menu.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.UseCases.Handlers.OperationHandlers
{
    public class RemoveProductFromTheBagHandler : IRequestHandler<RemoveProductFromTheBagCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly MenuDbContext dbContext;

        public RemoveProductFromTheBagHandler(MenuDbContext dbContext, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(RemoveProductFromTheBagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"Start handling RemoveProductFromTheBagCommand for UserId: {request.userId}, ProductId: {request.model.ProductId}");

                var isTheProductInTheBag = await dbContext.Bags.FirstOrDefaultAsync(
                    x => x.UserId == request.userId && x.ProductId == request.model.ProductId);

                if (isTheProductInTheBag == null || isTheProductInTheBag.Count == 0)
                {
                    Console.WriteLine("Product not found in the bag or count is zero.");
                    return true;
                }
                else
                {
                    if (isTheProductInTheBag.Count <= 1)
                    {
                        Console.WriteLine("Product count is 1 or less. Removing product from the bag.");

                        dbContext.Bags.Remove(isTheProductInTheBag);
                        await dbContext.SaveChangesAsync();

                        Console.WriteLine("Product removed from the bag and changes saved.");
                        return true;
                    }

                    Console.WriteLine("Decrementing product count in the bag.");

                    isTheProductInTheBag.Count -= 1;
                    await dbContext.SaveChangesAsync();

                    Console.WriteLine("Product count decremented and changes saved.");
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