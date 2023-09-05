using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Data;

namespace ToDoListApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly ToDoDbContext _context;

        public ItemController(ToDoDbContext context)
        {
            _context = context;
        }

        // Create
        [HttpPost]
        public JsonResult CreateItem(ToDoItem item)
        {
            _context.ToDoItems.Add(item);

            _context.SaveChangesAsync();

            return new JsonResult(Ok(item));
        }

        // Edit
        [HttpPut]
        public JsonResult UpdateItem(ToDoItem item)
        {
            int id = item.Id;
            ToDoItem existedItem = _context.ToDoItems.FirstOrDefault(n => n.Id == id);

            if (existedItem == null)
            {
                return new JsonResult(NotFound());
            }
            else
            {
                existedItem.Title = item.Title;
                existedItem.Description = item.Description;
                existedItem.ReleaseDate = item.ReleaseDate;
                existedItem.IsCompleted = item.IsCompleted;

                _context.SaveChanges();
            }

            return new JsonResult(Ok(existedItem));
        }

        // Get User`s Notes
        [HttpGet]
        public JsonResult GetAllItemsByUserId(string userId)
        {
            List<ToDoItem> items = _context.ToDoItems.Where(note => note.UserId.Equals(userId)).ToList();
            if (items == null)
            {
                return new JsonResult(NotFound());
            }
            else
            {
                return new JsonResult(Ok(items));
            }
        }

        // Get Note by Id
        [HttpGet]
        public JsonResult GetItemById(int id)
        {
            ToDoItem item = _context.ToDoItems.FirstOrDefault(n => n.Id == id);

            if (item == null)
            {
                return new JsonResult(NotFound());
            }
            else
            {
                return new JsonResult(Ok(item));
            }
        }

        // Delete Note
        [HttpDelete]
        public JsonResult DeleteItem(int id)
        {
            ToDoItem itemInDb = _context.ToDoItems.FirstOrDefault(note => note.Id == id);

            if (itemInDb == null)
            {
                return new JsonResult(NoContent());
            }
            else
            {
                _context.ToDoItems.Remove(itemInDb);
                _context.SaveChanges();
                return new JsonResult(NoContent());
            }
        }

    }
}
