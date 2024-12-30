using Forms.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace Forms.Pages.App
{
    [IgnoreAntiforgeryToken]
    public class SearchTestModel : PageModel
    {
        public class UserAccessPOCO
        {
            public int TemplateId { get; set; }
            public string UserId { get; set; }
        }

        private readonly ApplicationDbContext _appDbContext;

        public SearchTestModel(ApplicationDbContext dbContext)
        {
            _appDbContext = dbContext;
        }

        public void OnGet()
        {
            var useres = _appDbContext.Users;
            foreach (var user in useres)
            {
                Console.WriteLine("user: " + user);
            }
        }

        public async Task<IActionResult> OnGetSearchUsers(string query)
        {
            //Console.WriteLine("get search users");
            var users = await _appDbContext.Users
                .Where(u => u.Email.Contains(query)).OrderBy(x => x.Email)
                .Take(10) // Limit the results
                .Select(u => new { u.Id, u.Email })
                .ToListAsync();

            string json = users.ToJson();
            //Console.WriteLine(users.ToJson());
            return new JsonResult(users);
        }

        public async Task<JsonResult> OnPostAddUserToTemplate([FromBody] UserAccessPOCO userAccess)
        {
            //Console.WriteLine("trying to add");
            Console.WriteLine("json: " + userAccess.ToJson());
            return new JsonResult(new { success = true, message = "User added" });
        }

        public async Task<JsonResult> OnPostDeleteUserFromTemplate([FromBody] UserAccessPOCO userAccess)
        {
            Console.WriteLine("json delete: " + userAccess.ToJson());
            return new JsonResult(new { success = true, message = "User added" });
        }
    }
}
