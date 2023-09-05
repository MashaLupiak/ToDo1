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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ToDoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoItemExists(ToDoItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(ToDoItem), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("https://localhost:7044/api/Item/UpdateItem", content);

            return RedirectToPage("./Index");
        }

        private bool ToDoItemExists(int id)
        {
          return (_context.ToDoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
