using Forms.Data;
using Forms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Forms.Pages.App
{
    [Authorize]
    public class AnswerFormViewModel : PageModel
    {
        private readonly TemplateService _templateService;
        private readonly FormsService _formService;

        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }

        public Template Template { get; set; }
        public Data.Forms Form { get; set; }
        

        public AnswerFormViewModel(TemplateService templateService, FormsService formService)
        {
            _templateService = templateService;
            _formService = formService;
        }

       

        public IActionResult OnGet(int id)
        {            
            Data.Forms? form = _formService.GetForm(id);
            Template template = _templateService.GetTemplateById(form.TemplateId);
            Template = template;
            Form = form;

            Questions = template.QuestionList.ToList();
            Questions.Sort((x, y) => x.OrderIndex.CompareTo(y.OrderIndex));
            Answers = new List<Answer>();

            for (int i = 0; i < Questions.Count; i++)
            {
                Question question = Questions[i];
                Answer? answer = form.Answers.FirstOrDefault(x => x.QuestionId == question.Id);
                Answers.Add(answer!);
            }
            return Page();
        }
    }
}
