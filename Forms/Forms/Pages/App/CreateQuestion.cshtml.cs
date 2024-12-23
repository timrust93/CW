using Forms.Data;
using Forms.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Forms.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using NuGet.Protocol;

namespace Forms.Pages.App
{
    public class CreateQuestionModel : PageModel
    {
        public CreateQuestionModel(ApplicationDbContext dbContext, TemplateService templateService)
        {
            _dbContext = dbContext;
            _templateService = templateService;
        }

        [BindProperty]
        public Question Question { get; set; }

        [BindProperty]
        public int TemplateId { get; set; }

        [BindProperty]
        [Display(Name = "Question Type")]
        public QuestionTypeInfo QuestionTypeInfo { get; set; }
        

        private ApplicationDbContext _dbContext;
        private TemplateService _templateService;

        public ICollection<SelectListItem> QuestionTypesSelect { get; set; }  = new List<SelectListItem>();

        public void OnGet(int id)
        {            
            Template template = _templateService.GetTemplateById(id);
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

            
        }

        public IActionResult OnPost(int templateId)
        {

            if (ModelState.IsValid)
            {
                Console.WriteLine("Model state is not valid");
                return Page();                
            }

            Template template = _templateService.GetTemplateById(TemplateId);
            Question.TemplateId = template.Id;
            Question.OrderIndex = template.QuestionList.Count;
            template.QuestionList.Add(Question);
            _dbContext.SaveChanges();

            return RedirectToPage("TemplateManagement", new { id = TemplateId });            
        }
    }
}
