using Forms.Data;
using Forms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Forms.Pages.App
{
    [Authorize]
    public class CreateTemplateModel : PageModel
    {
        public class TemplateCreatePoco
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string Description { get; set; }
        }

        private readonly TemplateService _templateService;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        [Required]        
        public TemplateCreatePoco TemplateInit { get; set; }

        public CreateTemplateModel(TemplateService templateService, UserManager<ApplicationUser> userManager)
        {
            _templateService = templateService;
            _userManager = userManager;
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
                Template template = new Template();                
                template.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                template.Author = _userManager.GetUserName(User);
                template.Title = TemplateInit.Title;
                template.Description = TemplateInit.Description;
                template.LastModified = DateTime.Now;

                _templateService.AddNewTemplate(template);                

                return RedirectToPage("PersonalPage");           
            }            
        }
    }
}
