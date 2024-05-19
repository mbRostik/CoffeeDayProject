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

                var isTheProductInTheBag = await dbContext.Bags.FirstOrDefaultAsync(x => x.UserId == request.userId && x.ProductId == request.model.ProductId);

                if (isTheProductInTheBag == null)
                {
                    return true;
                }
                else
                {
                    dbContext.Bags.Remove(isTheProductInTheBag);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while creating user");
                return false;
            }
        }
    }
}
