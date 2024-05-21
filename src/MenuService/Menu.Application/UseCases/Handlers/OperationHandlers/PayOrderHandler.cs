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
                Console.WriteLine($"Start handling PayOrderCommand for UserId: {request.userId}");

                var isTheProductInTheBag = await dbContext.Bags
                    .Where(x => x.UserId == request.userId)
                    .ToListAsync();

                if (isTheProductInTheBag.Count == 0)
                {
                    Console.WriteLine($"No products in the bag for UserId: {request.userId}");
                    return false;
                }

                Console.WriteLine($"Fetched {isTheProductInTheBag.Count} products in the bag for UserId: {request.userId}");

                OrderPayedEvent orderPayedEvent = new OrderPayedEvent
                {
                    UserId = request.userId,
                    Order_Products = new List<Order_Product>()
                };

                foreach (var item in isTheProductInTheBag)
                {
                    var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == item.ProductId);

                    if (product == null)
                    {
                        Console.WriteLine($"Product with Id: {item.ProductId} not found");
                        continue;
                    }

                    Order_Product temp = new Order_Product
                    {
                        ProductName = product.Name,
                        ProductCount = item.Count,
                        ProductDescription = product.Description,
                        ProductPrice = product.Price,
                        ProductPhoto = product.Photo
                    };

                    orderPayedEvent.Order_Products.Add(temp);

                    Console.WriteLine($"Added product to OrderPayedEvent: {product.Name}, Count: {item.Count}");
                }

                dbContext.Bags.RemoveRange(isTheProductInTheBag);
                await dbContext.SaveChangesAsync();
                Console.WriteLine("Removed products from the bag and saved changes");

                await _publisher.Publish(orderPayedEvent);
                Console.WriteLine($"Published OrderPayedEvent for UserId: {request.userId}");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing order payment for UserId: {request.userId} - {ex}");
                return false;
            }
        }
    }
}