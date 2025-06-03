using MassTransit;
using MessageBus.Messages.User;
using TablesWebApi.DAL;

namespace TablesWebApi.Application
{
    public class UserCreation_Consumer : IConsumer<UserCreationEvent>
    {
        public ReservationDbContext _reservationDbContext { get; set; }

        public UserCreation_Consumer (ReservationDbContext reservationDbContext)
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
