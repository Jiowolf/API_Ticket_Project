using API_Ticket_Project.Entities;
using Scalar.AspNetCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.RouteTemplate = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}


List<User> users = new List<User>
{
    new User { Id = 1, Name = "John", Email = "Johndoe@domain.be" },
    new User { Id = 2, Name = "Hector", Email = "Hector@domain.be" },
    new User { Id = 3, Name = "Alice", Email = "alice@domain.be" },
    new User { Id = 4, Name = "Sophie", Email = "sophie@domain.be" }
};



app.Run();



