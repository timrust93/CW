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

namespace Forms.Pages.App
{
    //[IgnoreAntiforgeryToken]
    public class TemplateManagementModel : PageModel
    {
        public class OrderChange
        {
            public int QId { get; set; }
            public int OrderIndex { get; set; }
        }

        public List<Question> QuestionList { get; set; } = new List<Question>();

        [BindProperty]
        public List<QuestionTypeInfo> QuestionTypeInfos { get; set; } = new List<QuestionTypeInfo>();
        public Template Template { get; set; }
        public bool QuestonsLeftToAdd { get; set; } = false;

        public int MaxQuestionCount { get; set; }
        public int TemplateId { get; set; }

        private ApplicationDbContext _dbContext;
        private TemplateService _templateService;

        public TemplateManagementModel(ApplicationDbContext dbContext, TemplateService templateService)
        {
            _dbContext = dbContext;
            _templateService = templateService;
        }


        public void OnGet(int id = 1002)
        {
            TemplateId = id;
            Template = _templateService.GetTemplateById(id);
            if (Template != null)
            {
                QuestionTypeInfos = _templateService.GetQuestionTypeCounts(Template);      
                MaxQuestionCount = QuestionTypeInfos.Sum(x => x.MaxCount);
                InitializeQuestionList(Template);
                QuestionList.Sort((x, y) => x.OrderIndex.CompareTo(y.OrderIndex));
                int questionSLeft = QuestionTypeInfos.Sum(x => x.Left);
                QuestonsLeftToAdd = questionSLeft > 0;                                             
            }           
        }

        private void InitializeQuestionList(Template template)
        {
            foreach (Question question in template.QuestionList)
            {
                QuestionList.Add(question);
            }
        }

        private int GetQuestionLeftCount(Template template, int questionTypeId)
        {
            return template.QuestionList.Count(x => x.Type == questionTypeId);
        }



        public async Task<JsonResult> OnPostSaveQuestion([FromBody]Question question, [FromQuery] int id)
        {
            Console.WriteLine("is valid model: " + ModelState.IsValid);
            Console.WriteLine(question.ToJson());

            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }
            else
            {
                Template template = _templateService.GetTemplateById(id);
                Question originalQuestion = (template.QuestionList as List<Question>).Find(x => x.Id == question.Id);
                originalQuestion.Title = question.Title;
                originalQuestion.Description = question.Description;
                originalQuestion.Type = question.Type;
                await _dbContext.SaveChangesAsync();
                return new JsonResult(new { success = true, message = "Rows processed successfully!" });
            }
        }

        public async Task<JsonResult> OnPostDeleteQuestion([FromBody] Question question, [FromQuery] int id)
        {
            Console.WriteLine("post delete handler");
            try
            {
                Template template = _templateService.GetTemplateById(id);
                (template.QuestionList as List<Question>)?.RemoveAll(x => x.Id == question.Id);
                await _dbContext.SaveChangesAsync();
                return new JsonResult(new { success = true, message = "Rows processed successfully!" });
            }
            catch
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }
            
        }

        
        public async Task<JsonResult> OnPostChangeOrder([FromBody]List<OrderChange> list, [FromQuery] int id)
        {
            try
            {
                Template template = _templateService.GetTemplateById(id);

                for (int i = 0; i < list.Count; i++)
                {
                    Question question = template.QuestionList.FirstOrDefault(x => x.Id == list[i].QId);
                    if (question != null)
                        question.OrderIndex = list[i].OrderIndex;
                }
                await _dbContext.SaveChangesAsync();
                return new JsonResult(new { success = true, message = "Rows processed successfully!" });
            }
            catch
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }            
        }
    }
}
