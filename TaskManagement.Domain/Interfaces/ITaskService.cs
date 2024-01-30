
using TaskManagement.Domain.Dto.Input;
using TaskManagement.Domain.Dto.Output;

namespace TaskManagement.Domain.Interfaces
{
    public interface ITaskService
    {
        PaginationOutput<TaskOutput> Get(TaskReadInput input);
        bool Create(TaskInput input);
        bool Update(TaskInput input);
        bool Delete(Guid id);
    }
}
