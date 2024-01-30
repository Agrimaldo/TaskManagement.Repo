using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using TaskManagement.Domain.Dto.Input;
using TaskManagement.Domain.Dto.Output;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Domain.Util.Enumerators;
using entities = TaskManagement.Domain.Entities;

namespace TaskManagement.Service
{
    public class TaskService : ITaskService
    {
        private ILogger _logger;
        private IMessageBus _messageBus;
        private IRepository _repository;
        public TaskService(ILogger<TaskService> logger, IMessageBus messageBus, IRepository repository)
        {
            _logger = logger;
            _messageBus = messageBus;
            _repository = repository;
        }
        public bool Create(TaskInput input)
        {
            try
            {
                _logger.LogInformation(JsonSerializer.Serialize(input));
                _messageBus.Publish<TaskInput>(MessageType.Create, input);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"input : {JsonSerializer.Serialize(input)} \r\n Msg: {ex.Message} ");
                return false;
            }
        }

        public bool Delete(Guid id)
        {
            _logger.LogInformation(JsonSerializer.Serialize(id));
            try
            {
                _logger.LogInformation(JsonSerializer.Serialize(new TaskInput() { Id = id.ToString() }));
                _messageBus.Publish<TaskInput>(MessageType.Delete, new TaskInput() { Id = id.ToString() });
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"input : {JsonSerializer.Serialize(id)} \r\n Msg: {ex.Message} ");
                return false;
            }
        }

        public PaginationOutput<TaskOutput> Get(TaskReadInput input)
        {
            PaginationOutput<TaskOutput> responseList = new PaginationOutput<TaskOutput>();

            responseList.Page = input.Page;
            responseList.Total = _repository.Counter<entities.Task>() ?? 0;
            int skip = (input.Size * input.Page) - input.Size;
            responseList.Content = _repository.List<entities.Task>(skip, input.Size)?.ConvertAll(FromEntity) ?? new List<TaskOutput>();

            return responseList;
        }

        public bool Update(TaskInput input)
        {
            try
            {
                _logger.LogInformation(JsonSerializer.Serialize(input));
                _messageBus.Publish<TaskInput>(MessageType.Update, input);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"input : {JsonSerializer.Serialize(input)} \r\n Msg: {ex.Message} ");
                return false;
            }
        }

        private TaskOutput FromEntity(entities.Task task)
        {
            return new TaskOutput()
            {
                Id = task.Id.ToString(),
                Description = task.Description,
                Status = task.Status,
                Date = task.Date,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }
    }
}
