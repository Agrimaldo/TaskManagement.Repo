
using System.Text.Json.Serialization;

namespace TaskManagement.Domain.Entities
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = null!;
        public Util.Enumerators.TaskStatus Status { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
