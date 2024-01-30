using Microsoft.EntityFrameworkCore;
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
builder.Services.AddTransient<IMessageBus, MessageBus>();

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
