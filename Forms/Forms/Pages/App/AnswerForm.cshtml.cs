using Forms.Data;
using Forms.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using NuGet.Protocol;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Forms.Pages.App
{
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = -1, NoStore = true)]
    [Authorize]
    public class AnswerFormModel : PageModel
    {
        public class AnswerPOCO : IValidatableObject
        {
            public int QuestionType { get; set; }
            public int QuestionId { get; set; }            
            public int IntAnswer { get; set; }
            public bool BoolAnswer { get; set; }            
            public string StringAnswer { get; set; } = string.Empty;
            public DateTime LastModified { get; set; }
            public bool IsValid { get; set; } = true;
            public bool IsNew { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {                
                // Conditional validation logic
                if (QuestionType == 0 || QuestionType == 1) // Text input
                {
                    if (string.IsNullOrWhiteSpace(StringAnswer))
                    {
                        yield return new ValidationResult("Please provide an answer.", new[] { nameof(StringAnswer) });
                    }
                }
                
                // Add more cases for other types as needed.
            }
        }
                              

        public Template Template { get; set; }
        [BindProperty]
        public List<Question> Questions { get; set; }
        [BindProperty]
        public List<AnswerPOCO> Answers { get; set; } = new List<AnswerPOCO>();
        [BindProperty]
        public DateTime FormDownloadTime { get; set; }

        [TempData]
        public DateTime GetTime { get; set; }

        private readonly TemplateService _templateService;
        private readonly FormsService _formService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnswerFormModel(TemplateService templateService, FormsService formService, UserManager<ApplicationUser> userManager)
        {
            _templateService = templateService;
            _formService = formService;
            _userManager = userManager;
        }
        
        public IActionResult OnGet(int templateId)
        {            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);            

            Template = _templateService.GetTemplateById(templateId);
            if (Template == null)
            {
                return NotFound();
            }
            if (!Template.IsPublic && Template.OwnerId != userId &&
                Template.TemplateAccessList.FirstOrDefault(x => x.UserId == userId) == null)
            {
                return RedirectToPage("/TemplatePrivacyRestricted");
            }

            Questions = Template.QuestionList.ToList();
            Questions.Sort((x, y) => x.OrderIndex.CompareTo(y.OrderIndex));

            Data.Forms? form = _formService.GetForm(Template.Id, userId);

            foreach (Question question in Questions)
            {
                Answer answer = _formService.GetAnswer(form, question.Id);
                AnswerPOCO answerPOCO = new AnswerPOCO();
                if (answer == null)
                {
                    answer = new Answer() { QuestionId = question.Id };
                    answerPOCO.IsNew = true;
                }
                                
                answerPOCO.QuestionType = question.Type;
                Answers.Add(CopyAnswerValues(answer, answerPOCO));
            }
            FormDownloadTime = DateTime.Now;
            return Page();
        }

        

        public IActionResult OnPost(int templateId)
        {                        
            if (!ModelState.IsValid)
            {
                return BadRequest("REJECTED. BAD REQUEST");
            }
            
            Template template = _templateService.GetTemplateById(templateId);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!template.IsPublic &&
                    template.TemplateAccessList.FirstOrDefault(x => x.UserId == userId) == null)
            {
                return RedirectToPage("/TemplatePrivacyRestricted");
            }

            SaveForm(template, out bool questionAtLeastDeleted, out bool questionAtLeastModified, 
                out bool questionsMayBeenAdded);

            return RedirectToPage("FormCompleted", new
            {
                questionsModified = questionAtLeastModified,
                questionsDeleted = questionAtLeastDeleted,
                templateDeleted = false,
                questionsMayBeenAdded = questionsMayBeenAdded
            });
        }

        private void SaveForm(Template template, out bool questionAtLeastDeleted, 
            out bool questionAtLeastModified, out bool questionAtLeastAdded)
        {
            questionAtLeastDeleted = false;
            questionAtLeastModified = false;
            questionAtLeastAdded = false;
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var form = _formService.GetForm(template.Id, userId);
            if (form == null)
            {
                form = _formService.AddForm(CreateNewFormForUser(template));
            }

            List<Question> questions = template.QuestionList.ToList();
            int acceptedAnswerCount = 0;
            for (int i = 0; i < Answers.Count; i++)
            {                
                AnswerPOCO answerPOCO = Answers[i];                
                bool isAnswerNonValid = false;
                Question? question = questions.Find(x => x.Id == answerPOCO.QuestionId);
                if (question == null)
                {
                    questionAtLeastDeleted = true;
                    continue;
                }
                if (question.LastModified > FormDownloadTime)
                {
                    isAnswerNonValid = true;
                    questionAtLeastModified = true;
                }

                Answer? answer = form.Answers.FirstOrDefault(x => x.QuestionId == answerPOCO.QuestionId);
                acceptedAnswerCount += 1;
                if (answer == null)
                {
                    answer = new Answer();
                    CopyAnswerValues(answerPOCO, answer);
                    answer.FormsId = form.Id;
                    answer.LastModified = FormDownloadTime;
                    _formService.AddAnswer(answer, form);
                }
                else
                {
                    CopyAnswerValues(answerPOCO, answer);
                    answer.LastModified = FormDownloadTime;
                    _formService.UpdateAnswer(answer);
                }
            }
            questionAtLeastAdded = IsChanceQuestionsWereAdded(questions.Count, acceptedAnswerCount, questionAtLeastDeleted);
        }

        private bool IsChanceQuestionsWereAdded(int questionCount, int answerCount, bool somethingRemoved)
        {
            if (!somethingRemoved)
            {
                if (questionCount > answerCount)
                    return true;
                return false;
            }
            else
            {
                if (questionCount != answerCount)
                    return true;                               
                return false;
            }            
        }
        

        private Data.Forms CreateNewFormForUser(Template forTemplate)
        {
            Data.Forms form = new Data.Forms();
            form.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);            
            form.Author = _userManager.GetUserName(User);
            form.TemplateId = forTemplate.Id;
            form.LastModified = FormDownloadTime;
            return form;
        }
        

        private AnswerPOCO CopyAnswerValues(Answer from, AnswerPOCO to)
        {
            to.QuestionId = from.QuestionId;
            to.IntAnswer = from.IntAnswer;
            to.BoolAnswer = from.BoolAnswer;
            to.StringAnswer = from.StringAnswer;
            to.LastModified = from.LastModified;
            return to;
        }

        private Answer CopyAnswerValues(AnswerPOCO from, Answer to)
        {
            to.QuestionId = from.QuestionId;
            to.IntAnswer = from.IntAnswer;
            to.BoolAnswer = from.BoolAnswer;
            to.StringAnswer = from.StringAnswer;
            to.LastModified = from.LastModified;
            return to;
        }
    }
}
