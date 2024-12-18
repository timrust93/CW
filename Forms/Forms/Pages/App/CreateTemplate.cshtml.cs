using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

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

        [BindProperty]
        public TemplateCreatePoco TemplateInit { get; set; }

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
                return Redirect("PersonalPage");                
            }
            
        }
    }
}
