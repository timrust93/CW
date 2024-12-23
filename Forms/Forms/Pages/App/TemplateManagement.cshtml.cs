using Forms.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static Forms.Pages.App.TemplateManagementModel;
using Forms.Model;
using Forms.Services;

namespace Forms.Pages.App
{

    public class TemplateManagementModel : PageModel
    {

        public List<Question> QuestionList { get; set; } = new List<Question>();

        [BindProperty]
        public List<QuestionTypeInfo> QuestionTypeInfos { get; set; } = new List<QuestionTypeInfo>();
        public Template Template { get; set; }
        public bool QuestonsLeftToAdd { get; set; } = false;

        private ApplicationDbContext _dbContext;
        private TemplateService _templateService;

        public TemplateManagementModel(ApplicationDbContext dbContext, TemplateService templateService)
        {
            _dbContext = dbContext;
            _templateService = templateService;
        }


        public void OnGet(int id = 1002)
        {            
            Template = _templateService.GetTemplateById(id);
            if (Template != null)
            {
                QuestionTypeInfos = _templateService.GetQuestionTypeCounts(Template);                
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


        public IActionResult OnPostSave()
        {
            Console.WriteLine("is valid model: " + ModelState.IsValid);
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                return Page();
            }
        }

        public IActionResult OnPost(int id)
        {
            if (ModelState.TryGetValue("X", out ModelStateEntry stateEntry))
            {
                Console.WriteLine(stateEntry.RawValue.ToString());                
            }
            
            Console.WriteLine(string.Join(",", ModelState.Keys));
            Console.WriteLine(string.Join(",", ModelState.Values));

            //ModelState.Keys.Join(',');
            Console.WriteLine("id: " + id);
            Console.WriteLine("model state valid? " + ModelState.IsValid);
            return Page(); // Or any response logic
        }
    }
}
