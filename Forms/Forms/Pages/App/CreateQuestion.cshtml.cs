using Forms.Data;
using Forms.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Forms.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using NuGet.Protocol;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authorization;

namespace Forms.Pages.App
{
    [Authorize]
    public class CreateQuestionModel : PageModel
    {
        [BindProperty]
        public Question Question { get; set; }

        [BindProperty]
        public int TemplateId { get; set; }

        [BindProperty]
        [Display(Name = "Question Type")]
        public QuestionTypeInfo QuestionTypeInfo { get; set; }

        private readonly ApplicationDbContext _dbContext;
        private readonly TemplateService _templateService;

        public CreateQuestionModel(ApplicationDbContext dbContext, TemplateService templateService)
        {
            _dbContext = dbContext;
            _templateService = templateService;            
        }        
               

        public ICollection<SelectListItem> QuestionTypesSelect { get; set; }  = new List<SelectListItem>();

        public IActionResult OnGet(int id)
        {
            Template template = _templateService.GetTemplateById(id);
            if (!_templateService.IsAuthorized(User, template))
            {
                return BadRequest("Forbidden. Unauthorized");
            }
            Question question = new Question();
            TemplateId = template.Id;
            List<QuestionTypeInfo> questionTypeInfos = _templateService.GetQuestionTypeCounts(template);
            foreach (var qTypeInfo in questionTypeInfos)
            {
                QuestionTypesSelect.Add(new SelectListItem { Value = qTypeInfo.Id.ToString(),
                    Text = qTypeInfo.DisplayName + $" ({_templateService.GetQuestionTypeLeftCount(template, qTypeInfo.Id)})",
                    Disabled = qTypeInfo.Left <= 0
                });
            }
            return Page();
            
        }

        public IActionResult OnPost(int templateId)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("Model state is not valid");
                return Page();                
            }
            Template template = _templateService.GetTemplateById(TemplateId);
            if (!_templateService.IsAuthorized(User, template))
            {
                return BadRequest("Forbidden. Unauthorized");
            }
            if (_templateService.GetQuestionTypeLeftCount(template, QuestionTypeInfo.Id) == 0)
            {
                return BadRequest("REJECTED for data violation");
            }
            
            Question.TemplateId = template.Id;
            Question.OrderIndex = template.QuestionList.Count;
            Question.Description = string.Empty;
            Question.Type = QuestionTypeInfo.Id;
            template.QuestionList.Add(Question);
            _dbContext.SaveChanges();

            return RedirectToPage("TemplateManagement", new { id = TemplateId });            
        }
    }
}
