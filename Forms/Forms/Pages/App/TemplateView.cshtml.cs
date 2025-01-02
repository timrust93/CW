using Forms.Data;
using Forms.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Forms.Pages.App
{
    public class TemplateView : PageModel
    {        
        public Template Template { get; set; }
        public List<Question> Questions { get; set; }

        private TemplateService _templateService;

        public TemplateView(TemplateService templateService, FormsService formService)
        {
            _templateService = templateService;            
        }

        public IActionResult OnGetPage(int templateId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Template = _templateService.GetTemplateById(templateId);
            if (Template == null)
            {
                return NotFound();
            }

            if (userId != null)
            {
                return RedirectToPage("AnswerForm", new { templateId = templateId});
            }
            
            if (!Template.IsPublic)
            {
                return RedirectToPage("/TemplatePrivacyRestricted");
            }
            
            Questions = Template.QuestionList.ToList();
            Questions.Sort((x, y) => x.OrderIndex.CompareTo(y.OrderIndex));
            return Page();
            
        }
    }
}
