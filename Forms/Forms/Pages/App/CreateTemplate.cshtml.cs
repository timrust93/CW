using Forms.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Forms.Pages.App
{
    public class CreateTemplateModel : PageModel
    {
        public class TemplateCreatePoco
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string Description { get; set; }
        }

        private ApplicationDbContext _dbContext;

        [BindProperty]
        [Required]        
        public TemplateCreatePoco TemplateInit { get; set; }

        public CreateTemplateModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void OnGet()
        {
                        
        }

        public IActionResult OnPost(string returnUrl = null)
        {           
            Console.WriteLine("post form");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Template template = new Template();
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                template.OwnerId = userId;
                template.Title = TemplateInit.Title;
                template.Description = TemplateInit.Description;

                _dbContext.Templates.Add(template);
                _dbContext.SaveChanges();

                return RedirectToPage("PersonalPage");
                //return RedirectToPage("TemplateManagement");
                //return Redirect("PersonalPage");                
            }
            
        }
    }
}
