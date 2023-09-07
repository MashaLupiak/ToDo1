using ToDoList.Models;

namespace ToDoList.Data
{
    public class ApiResponse<T>
    {
        public T Value { get; set; }
        public List<string> Formatters { get; set; }
        public List<string> ContentTypes { get; set; }
        public Type DeclaredType { get; set; }
        public int StatusCode { get; set; }
    }
}
