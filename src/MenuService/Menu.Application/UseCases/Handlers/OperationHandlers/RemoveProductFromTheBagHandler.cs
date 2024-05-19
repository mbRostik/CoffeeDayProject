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

                var isTheProductInTheBag = await dbContext.Bags.FirstOrDefaultAsync(x => x.UserId == request.userId && x.ProductId == request.model.ProductId);

                if (isTheProductInTheBag == null || isTheProductInTheBag.Count == 0)
                {
                    return true;
                }
                else
                {
                    if (isTheProductInTheBag.Count <= 1)
                    {
                        dbContext.Bags.Remove(isTheProductInTheBag);
                        await dbContext.SaveChangesAsync();
                        return true;
                    }
                    isTheProductInTheBag.Count -= 1;
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
