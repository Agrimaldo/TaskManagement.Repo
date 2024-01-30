
using System.Text.Json.Serialization;

namespace TaskManagement.Domain.Dto.Output
{
    public class TaskOutput
    {
        public string Id { get; set; } = ""!;
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Util.Enumerators.TaskStatus Status { get; set; }
    }
}
