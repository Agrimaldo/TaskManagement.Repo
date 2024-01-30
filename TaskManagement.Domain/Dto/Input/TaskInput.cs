
using System.Text.Json.Serialization;

namespace TaskManagement.Domain.Dto.Input
{
    public class TaskInput
    {
        public string? Id { get; set; }
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Util.Enumerators.TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
