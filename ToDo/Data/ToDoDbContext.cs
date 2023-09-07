using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList.Data
{
    public class ToDoDbContext : IdentityDbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
            : base(options)
        {
        }
        public DbSet<ToDoList.Models.ToDoItem> ToDoItems { get; set; } = default!;
    }
}