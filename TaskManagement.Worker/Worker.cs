using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TaskManagement.Domain.Dto.Input;
using TaskManagement.Domain.Dto.Output;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Util.Enumerators;
using TaskManagement.Service;
using entities = TaskManagement.Domain.Entities;

namespace TaskManagement.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        IRepository _repository;
        IMessageBus _messageBus;
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(4);


        public Worker(ILogger<Worker> logger, IRepository repository, IMessageBus messageBus)
        {
            _logger = logger;
            _repository = repository;
            _messageBus = messageBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            foreach (MessageType item in Enum.GetValues(typeof(MessageType)))
            {
                switch (item)
                {
                    case MessageType.Create:
                        _messageBus.Consume<TaskInput>(MessageType.Create, CreateReceiver);
                        break;
                    case MessageType.Update:
                        _messageBus.Consume<TaskInput>(MessageType.Update, UpdateReceiver);
                        break;
                    case MessageType.Delete:
                        _messageBus.Consume<TaskInput>(MessageType.Delete, DeleteReceiver);
                        break;
                }
            }

            await Task.Delay(1000, stoppingToken);
        }

        public void CreateReceiver(object? sender, BasicDeliverEventArgs e)
        {
            string content = Encoding.UTF8.GetString(e.Body.ToArray());
            TaskInput input = JsonSerializer.Deserialize<TaskInput>(content)!;

            _logger.LogInformation("Worker running Create at: {time}", DateTimeOffset.Now);
            _logger.LogInformation($"CreateTask {JsonSerializer.Serialize(input)}");

            try
            {
                _repository.Add<entities.Task>(new entities.Task()
                {
                    Description = input.Description,
                    Status = input.Status,
                    Date = input.Date,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateReceiver: {ex.Message}");
                throw;
            }
        }

        public void UpdateReceiver(object? sender, BasicDeliverEventArgs e)
        {
            string content = Encoding.UTF8.GetString(e.Body.ToArray());
            TaskInput input = JsonSerializer.Deserialize<TaskInput>(content)!;

            _logger.LogInformation("Worker running Update at: {time}", DateTimeOffset.Now);
            _logger.LogInformation($"UpdateTask {JsonSerializer.Serialize(input)}");

            try
            {
                entities.Task obj = _repository.List<entities.Task>(0, 1, a => a.Id == Guid.Parse(input.Id!))!.FirstOrDefault()!;

                obj.Description = input.Description;
                obj.Status = input.Status;
                obj.Date = input.Date;
                obj.CreatedAt = input.CreatedAt;
                obj.UpdatedAt = DateTime.Now;

                _repository.Update<entities.Task>(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateReceiver: {ex.Message}");
                throw;
            }
        }

        public void DeleteReceiver(object? sender, BasicDeliverEventArgs e)
        {
            string content = Encoding.UTF8.GetString(e.Body.ToArray());
            TaskInput input = JsonSerializer.Deserialize<TaskInput>(content)!;

            _logger.LogInformation("Worker running Delete at: {time}", DateTimeOffset.Now);
            _logger.LogInformation($"DeleteTask {JsonSerializer.Serialize(input)}");

            try
            {
                entities.Task obj = _repository.List<entities.Task>(0, 1, a => a.Id == Guid.Parse(input.Id!))!.FirstOrDefault()!;
                _repository.Delete<entities.Task>(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteReceiver: {ex.Message}");
                throw;
            }
        }
    }
}