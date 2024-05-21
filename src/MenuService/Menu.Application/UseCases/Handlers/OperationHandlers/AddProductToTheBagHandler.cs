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
    public class AddProductToTheBagHandler : IRequestHandler<AddProductToTheBagCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly MenuDbContext dbContext;

        public AddProductToTheBagHandler(MenuDbContext dbContext, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(AddProductToTheBagCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"Start handling AddProductToTheBagCommand for UserId: {request.userId}, ProductId: {request.model.ProductId}");

                var isTheProductInTheBag = await dbContext.Bags.FirstOrDefaultAsync(
                    x => x.UserId == request.userId && x.ProductId == request.model.ProductId);

                if (isTheProductInTheBag == null || isTheProductInTheBag.Count == 0)
                {
                    Console.WriteLine("Product not in the bag or count is zero. Adding new product to the bag.");

                    Bag bag = new Bag
                    {
                        UserId = request.userId,
                        ProductId = request.model.ProductId,
                        Count = 1
                    };

                    await dbContext.AddAsync(bag);
                    await dbContext.SaveChangesAsync();

                    Console.WriteLine("Product added to the bag and changes saved.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Product already in the bag. Incrementing the count.");

                    isTheProductInTheBag.Count += 1;
                    await dbContext.SaveChangesAsync();

                    Console.WriteLine("Product count incremented and changes saved.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while adding product to the bag for UserId: {request.userId}, ProductId: {request.model.ProductId} - {ex}");
                return false;
            }
        }
    }
}