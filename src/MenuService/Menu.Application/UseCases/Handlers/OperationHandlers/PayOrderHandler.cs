using MassTransit;
using MediatR;
using Menu.Application.UseCases.Commands;
using Menu.Infrastructure.Data;
using MessageBus.Messages.Menu;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Application.UseCases.Handlers.OperationHandlers
{
    public class PayOrderHandler : IRequestHandler<PayOrderCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly MenuDbContext dbContext;
        private readonly IPublishEndpoint _publisher;

        public PayOrderHandler(MenuDbContext dbContext, IMediator mediator, IPublishEndpoint _publisher)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
            this._publisher = _publisher;
        }

        public async Task<bool> Handle(PayOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var isTheProductInTheBag =await dbContext.Bags.Where(x => x.UserId == request.userId).ToListAsync();

                OrderPayedEvent orderPayedEvent = new OrderPayedEvent();
                foreach(var item in isTheProductInTheBag)
                {
                    var product = await dbContext.Products.FirstOrDefaultAsync(x=>x.Id == item.ProductId);
                    Order_Product temp = new Order_Product
                    {
                        ProductName = product.Name,
                        ProductCount = item.Count,
                        ProductDescription = product.Description,
                        ProductPrice = product.Price,
                        ProductPhoto = product.Photo
                    };
                    orderPayedEvent.Order_Products.Add(temp);
                }

                dbContext.Bags.RemoveRange(isTheProductInTheBag);
                await dbContext.SaveChangesAsync();
                await _publisher.Publish(orderPayedEvent);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while creating user");
                return false;
            }
        }
    }
}
