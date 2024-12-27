using Forms.Data;
using Forms.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Forms.Pages
{
    public class IndexModel : PageModel
    {
        public List<Template> TemplateList { get; set; }

        private readonly ILogger<IndexModel> _logger;
        private readonly TemplateService _templateService;
        private UserManager<ApplicationUser> _userManager;

        public IndexModel(ILogger<IndexModel> logger, TemplateService templateService)
        {
            _logger = logger;
            _templateService = templateService;
        }

        public IActionResult OnGet()
        {
            TemplateList = _templateService.GetTemplateList();
            return Page();
            return RedirectToPage("App/TemplateManagement");
        }
    }
}
