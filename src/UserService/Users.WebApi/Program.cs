using MassTransit;
using MessageBus.Messages.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Text;
using Users.Application.UseCases.Commands;
using Users.Application.UseCases.Consumers;
using Users.Application.UseCases.Handlers.OperationHandlers;
using Users.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
string? connectionString = builder.Configuration.GetConnectionString("MSSQLConnection");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(typeof(CreateUserCommand).Assembly);

});
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserCreation_Consumer>();
    x.AddConsumer<OrderPayedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("User_UserConsumer_queue", e =>
        {
            e.ConfigureConsumer<UserCreation_Consumer>(context);
        });

        cfg.ReceiveEndpoint("User_OrderPayedConsumer_queue", e =>
        {
            e.ConfigureConsumer<OrderPayedConsumer>(context);
        });
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtBearerSettings = builder.Configuration.GetSection("JwtBearer");
        options.Authority = jwtBearerSettings["Authority"];
        options.Audience = "Users.WebApi";
    });
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
