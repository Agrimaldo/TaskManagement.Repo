using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Repository;
using TaskManagement.Repository.Context;
using TaskManagement.Service;

var builder = WebApplication.CreateBuilder(args);


var mainConnecttion = builder.Configuration.GetConnectionString("sqlServer");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddCors(a => a.AddPolicy("General", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

builder.Services.AddDbContext<TaskManagementContext>(options =>
{
    options.UseSqlServer(mainConnecttion);
});

builder.Services.AddScoped<IRepository, TaskManagementRepository>(serviceProvider =>
{
    var context = serviceProvider.GetService<TaskManagementContext>()!;
    context.Database.SetCommandTimeout(400);
    return new TaskManagementRepository(context);
});


builder.Services.AddTransient<ITaskService, TaskService>();
builder.Services.AddTransient<IMessageBus, MessageBus>(serviceProvider => {

    ConnectionFactory factory = new ConnectionFactory();
    factory.HostName = builder.Configuration.GetValue<String>("RabbitMQ:HostName");
    factory.VirtualHost = builder.Configuration.GetValue<String>("RabbitMQ:VirtualHost");
    factory.Port = builder.Configuration.GetValue<int>("RabbitMQ:Port");
    factory.UserName = builder.Configuration.GetValue<String>("RabbitMQ:UserName");
    factory.Password = builder.Configuration.GetValue<String>("RabbitMQ:Password");
    return new MessageBus(factory);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.UseCors();
app.Run();
