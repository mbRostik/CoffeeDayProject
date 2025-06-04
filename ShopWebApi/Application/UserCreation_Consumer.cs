using MassTransit;
using MessageBus.Messages.User;
using ShopWebApi.DAL;

namespace ShopWebApi.Application
{
    public class UserCreation_Consumer : IConsumer<UserCreationEvent>
    {
        public ShopDbContext _reservationDbContext { get; set; }

        public UserCreation_Consumer(ShopDbContext reservationDbContext)
        {
            _reservationDbContext = reservationDbContext;
        }
        public async Task Consume(ConsumeContext<UserCreationEvent> context)
        {
            Console.WriteLine($"Successfully consumed UserCreationEvent");

            User temp = new User
            {
                Id = context.Message.UserId

            };
            _reservationDbContext.Users.Add(temp);
            await _reservationDbContext.SaveChangesAsync();
        }
    }
}
