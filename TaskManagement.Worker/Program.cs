using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Repository;
using TaskManagement.Repository.Context;
using TaskManagement.Service;
using TaskManagement.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        var optionsBuilder = new DbContextOptionsBuilder<TaskManagementContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("sqlServer"));
        services.AddTransient<TaskManagementContext>(s => new TaskManagementContext(optionsBuilder.Options));
        services.AddTransient<IRepository, TaskManagementRepository>(serviceProvider =>
        {
            var context = serviceProvider.GetService<TaskManagementContext>()!;
            context.Database.SetCommandTimeout(400);
            return new TaskManagementRepository(context);
        });
        services.AddTransient<IMessageBus, MessageBus>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
