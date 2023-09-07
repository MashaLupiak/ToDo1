using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public string? Title { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
