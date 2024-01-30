
namespace TaskManagement.Domain.Dto.Output
{
    public class PaginationOutput<T> where T : class
    {
        public int Page { get; set; }
        public int Total { get; set; }
        public IList<T> Content { get; set; } = new List<T>();
    }
}
