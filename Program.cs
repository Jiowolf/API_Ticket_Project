using API_Ticket_Project.Entities;
using Microsoft.AspNetCore.Authentication;
using Scalar.AspNetCore;
using System.Net.Sockets;



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
    app.UseSwagger();
    app.UseSwaggerUI();
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
    new User { Id = 1, Name = "John", Email = "Johndoe@domain.be"},
    new User { Id = 2, Name = "Hector", Email = "Hector@domain.be" },
    new User { Id = 3, Name = "Alice", Email = "alice@domain.be"},
    new User { Id = 4, Name = "Sophie", Email = "sophie@domain.be"}
};


List<Ticket> tickets = new List<Ticket>
{
    new Ticket{ Id = 1, Title = "Need Ink", Description = "the printer leak black ink", status = "open", Create_at = new DateTime(2025,06,13) ,User = users[3] }, 
    new Ticket{ Id = 2, Title = "Help", Description = "i deteted some usless file (the sys32 one) and my computer stop working", status = "in progress", Create_at = new DateTime(2025,06,06),User = users[2]},
    new Ticket{ Id = 3, Title = "New keyboard", Description = "my keyboard broke when Hector use his head to hit it, i need a new one", status = "resolved", Create_at = new DateTime(2025,06,03),User = users[0]}
};



//page d'acceuil
app.MapGet("/", () => "Hello there");


app.MapPost("/users", (User newUser) =>
{
    newUser.Id = users.Max(u => u.Id) + 1;
    users.Add(newUser);

    return Results.Created($"/users/{newUser.Id}", newUser);
});

app.MapGet("/users", () => users);

app.MapGet("/users/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
    {
        return Results.NotFound("User not found");
    }
    return Results.Ok(user);
});

app.MapGet("/tickets", () => tickets);

app.MapGet("/tickets/{status}", (string? status) =>
{

    foreach(var t in tickets)
    {
        if (status == "open")
        {
            return Results.Ok(t);
        }
    }

    return Results.Ok("Filter Done");
});


app.MapPut("/tickets/{id}", (int id, UpdateTicket updateTicket) =>
{
    var ticket = tickets.FirstOrDefault(t => t.Id == id);
    if (ticket == null)
    {
        return Results.NotFound("Ticket not found");
    }

    ticket.status = updateTicket.status;

    return Results.Ok(ticket);
});

app.MapDelete("/tickets/{id}",(int id) =>
{
    var ticket = tickets.FirstOrDefault(t => t.Id == id);
    if (ticket == null)
    {
        return Results.NotFound("Ticket not found");
    }

    tickets.Remove(ticket);

    return Results.Ok("Ticket Deleted");
});

app.MapPost("/users/{id}/tickets", (int id, PostTicket newTicket) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
    {
        return Results.NotFound("User not found");
    }

    var ticket = new Ticket
    {
        Id = tickets.Max(t => t.Id) + 1,
        Title = newTicket.Title,
        Description = newTicket.Description,
        status = "open",
        Create_at = DateTime.Now,
        User = user
    };

    tickets.Add(ticket);
    return Results.Ok("New ticket created");
});

app.MapGet("/users/{id}/tickets", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    if (user == null)
    {
        return Results.NotFound("User not found");
    }

    foreach( var t in tickets)
    {
        if(t.User.Id == id)
        {
            return Results.Ok(t);
        }
    }

    return Results.Ok("Seach done");
}
);


app.Run();



