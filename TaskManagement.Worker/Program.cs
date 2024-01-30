using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
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
        services.AddTransient<IMessageBus, MessageBus>(serviceProvider => {

            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = configuration.GetValue<String>("RabbitMQ:HostName");
            factory.VirtualHost = configuration.GetValue<String>("RabbitMQ:VirtualHost");
            factory.Port = configuration.GetValue<int>("RabbitMQ:Port");
            factory.UserName = configuration.GetValue<String>("RabbitMQ:UserName");
            factory.Password = configuration.GetValue<String>("RabbitMQ:Password");
            return new MessageBus(factory);
        });
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
