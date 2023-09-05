using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ToDoList.Data;
using ToDoList.Models;

namespace ToDoList.Pages.ToDoListCRUD
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ToDoList.Data.ToDoDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly HttpClient _httpClient;

        public IndexModel(ToDoList.Data.ToDoDbContext context, UserManager<IdentityUser> userManager, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _userManager = userManager;
            _httpClient = httpClientFactory.CreateClient();
        }

        public IList<ToDoItem> ToDoItem { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.ToDoItems != null)
            {
                string userId = _userManager.GetUserId(User);
                var response = await _httpClient.GetAsync($"https://localhost:7044/api/Item/GetAllItemsByUserId?userId={userId}");
                string json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ApiResponse<List<ToDoItem>>>(json);
                ToDoItem = apiResponse.Value;
            }
        }
    }
}
