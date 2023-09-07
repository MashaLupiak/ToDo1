using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ToDoList.Data;
using ToDoList.Models;
using Newtonsoft.Json;
using System.Text;

namespace ToDoList.Pages.ToDoListCRUD
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ToDoList.Data.ToDoDbContext _context;
        private readonly HttpClient _httpClient;

        public EditModel(ToDoList.Data.ToDoDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty]
        public ToDoItem ToDoItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _httpClient.GetAsync($"https://localhost:7044/api/Item/GetItemById?id={id}");
            var stringJson = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ToDoItem>>(stringJson);
            ToDoItem item = apiResponse.Value;

            if (item == null)
            {
                return NotFound();
            }
            ToDoItem = item;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _httpClient.PutAsJsonAsync("https://localhost:7044/api/Item/UpdateItem", ToDoItem);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Помилка при оновленні даних.");
                return Page();
            }
        }

        private bool ToDoItemExists(int id)
        {
          return (_context.ToDoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
