using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Users.Application.UseCases.Commands;
using Users.Application.UseCases.Consumers;
using Users.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);
string? connectionString = builder.Configuration.GetConnectionString("MSSQLConnection");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ContactUsDbContext>(options =>
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

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ReceiveEndpoint("ContactUs_UserConsumer_queue", e =>
        {
            e.ConfigureConsumer<UserCreation_Consumer>(context);
        });

    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtBearerSettings = builder.Configuration.GetSection("JwtBearer");
        options.Authority = jwtBearerSettings["Authority"];
        options.Audience = "ContactUs.WebApi";
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
