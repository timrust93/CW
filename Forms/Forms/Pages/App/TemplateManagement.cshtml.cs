using Forms.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static Forms.Pages.App.TemplateManagementModel;
using Forms.AuthorizationHelpers;
using Forms.Model;
using Forms.Services;
using NuGet.Protocol;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Forms.Pages.App
{
    [Authorize]
    public class TemplateManagementModel : PageModel
    {
        public class OrderChange
        {
            public int QId { get; set; }
            public int OrderIndex { get; set; }
        }

        public class FormDisplay
        {
            public string Author { get; set; } = string.Empty;
            public int Id { get; set; }
        }

        public List<Question> QuestionList { get; set; } = new List<Question>();
        public List<FormDisplay> FormDisplayList { get; set; } = new List<FormDisplay>();

        [BindProperty]
        public List<QuestionTypeInfo> QuestionTypeInfos { get; set; } = new List<QuestionTypeInfo>();
        public Template Template { get; set; }

        public int MaxQuestionCount { get; set; }
        public int TemplateId { get; set; }

        private readonly ApplicationDbContext _dbContext;
        private readonly TemplateService _templateService;
        private readonly FormsService _formService;

        public TemplateManagementModel(ApplicationDbContext dbContext, TemplateService templateService, FormsService formService)
        {
            _dbContext = dbContext;
            _templateService = templateService;
            _formService = formService;
        }


        public IActionResult OnGet(int id = 1002)
        {
            TemplateId = id;
            Template = _templateService.GetTemplateById(id);
            if (!_templateService.IsAuthorized(User, Template))
            {
                return BadRequest("Bad Request");
            }
            if (Template == null)
            {
                return NotFound();
            }
            
            QuestionTypeInfos = _templateService.GetQuestionTypeCounts(Template);      
            MaxQuestionCount = QuestionTypeInfos.Sum(x => x.MaxCount);
            InitializeQuestionList(Template);
            QuestionList.Sort((x, y) => x.OrderIndex.CompareTo(y.OrderIndex));

            var forms = _formService.GetFormList(Template.Id);
            foreach (var form in forms)
            {
                FormDisplay formDisplay = new FormDisplay
                {
                    Author = form.Author,
                    Id = form.Id,
                };
                FormDisplayList.Add(formDisplay);
            }

            return Page();
        }

        private void InitializeQuestionList(Template template)
        {
            foreach (Question question in template.QuestionList)
            {
                QuestionList.Add(question);
            }
        }



        public JsonResult OnPostSaveQuestion([FromBody]Question question, [FromQuery] int id)
        {
            Console.WriteLine("is valid model: " + ModelState.IsValid);
            Console.WriteLine(question.ToJson());
            Template template = _templateService.GetTemplateById(id);
            if (!_templateService.IsAuthorized(User, template))
            {
                return new JsonResult(new { success = false, message = "Forbidden. Unauthorized" });
            }

            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }
            else
            {                
                template.LastModified = DateTime.Now;
                Question originalQuestion = (template.QuestionList as List<Question>).Find(x => x.Id == question.Id);
                originalQuestion.Title = question.Title;
                originalQuestion.Description = question.Description;
                originalQuestion.Type = question.Type;
                originalQuestion.LastModified = DateTime.Now;
                _dbContext.SaveChanges();
                return new JsonResult(new { success = true, message = "Rows processed successfully!" });
            }
        }

        public JsonResult OnPostDeleteQuestion([FromBody] Question question, [FromQuery] int id)
        {
            Template template = _templateService.GetTemplateById(id);
            if (!_templateService.IsAuthorized(User, template))
            {
                return new JsonResult(new { success = false, message = "Forbidden. Unauthorized" });
            }
            Console.WriteLine("post delete handler");
            try
            {                
                template.LastModified = DateTime.Now;
                (template.QuestionList as List<Question>)?.RemoveAll(x => x.Id == question.Id);
                _dbContext.SaveChanges();
                return new JsonResult(new { success = true, message = "Rows processed successfully!" });
            }
            catch
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }            
        }

        public JsonResult OnPostChangeOrder([FromBody] List<OrderChange> list, [FromQuery] int id)
        {
            Template template = _templateService.GetTemplateById(id);
            if (!_templateService.IsAuthorized(User, template))
            {
                return new JsonResult(new { success = false, message = "Forbidden. Unauthorized" });
            }
            try
            {                
                for (int i = 0; i < list.Count; i++)
                {
                    Question question = template.QuestionList.FirstOrDefault(x => x.Id == list[i].QId);
                    if (question != null)
                        question.OrderIndex = list[i].OrderIndex;
                }
                _dbContext.SaveChanges();
                return new JsonResult(new { success = true, message = "Rows processed successfully!" });
            }
            catch
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }
        }
    }
}
