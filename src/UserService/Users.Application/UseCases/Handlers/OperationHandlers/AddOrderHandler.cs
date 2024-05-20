using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Contracts.SendDTOs;
using Users.Application.UseCases.Commands;
using Users.Application.UseCases.Queries;
using Users.Domain;
using Users.Infrastructure.Data;

namespace Users.Application.UseCases.Handlers.OperationHandlers
{
    public class AddOrderHandler : IRequestHandler<AddOrderCommand>
    {
        private readonly IMediator mediator;

        private readonly UserDbContext dbContext;

        public AddOrderHandler(UserDbContext dbContext, IMediator mediator)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
        }

        public async Task Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<Product> newProducts = new List<Product>();

                foreach (var model in request.model.Order_Products)
                {
                    var existingProduct = await dbContext.Products
                        .FirstOrDefaultAsync(x => x.ProductName == model.ProductName &&
                                                  x.ProductPrice == model.ProductPrice &&
                                                  x.ProductDescription == model.ProductDescription);

                    if (existingProduct == null)
                    {
                        var product = new Product
                        {
                            ProductName = model.ProductName,
                            ProductPrice = model.ProductPrice,
                            ProductDescription = model.ProductDescription,
                            ProductPhoto = model.ProductPhoto
                        };
                        var newProduct = await dbContext.Products.AddAsync(product);
                        newProducts.Add(newProduct.Entity);
                    }
                    else
                    {
                        newProducts.Add(existingProduct);
                    }
                }

                await dbContext.SaveChangesAsync();

                var order = new OrderHistory
                {
                    UserId = request.model.UserId,
                    Date = DateTime.UtcNow,
                };

                var newOrder = await dbContext.OrderHistories.AddAsync(order);
                await dbContext.SaveChangesAsync();

                List<Order_Products> orderProducts = new List<Order_Products>();

                foreach (var model in request.model.Order_Products)
                {
                    var product = newProducts
                        .FirstOrDefault(p => p.ProductName == model.ProductName &&
                                             p.ProductPrice == model.ProductPrice &&
                                             p.ProductDescription == model.ProductDescription);

                    if (product != null)
                    {
                        Order_Products orderProduct = new Order_Products
                        {
                            OrderId = newOrder.Entity.Id,
                            ProductId = product.Id,
                            Count = model.ProductCount
                        };
                        orderProducts.Add(orderProduct);
                    }
                }

                await dbContext.OrderProducts.AddRangeAsync(orderProducts);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }
        }
    }
}
