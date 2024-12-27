using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Forms.Pages.App
{
    public class FormCompletedModel : PageModel
    {
        [BindProperty]
        public bool QuestionModified { get; set; }
        [BindProperty]
        public bool QuestionDeleted { get; set; }
        [BindProperty]
        public bool TemplateDeleted { get; set; }
        [BindProperty]
        public bool QuestionsMayBeenAdded { get; set; }


        public void OnGet(bool questionsModified, bool questionsDeleted, 
            bool templateDeleted, bool questionsMayBeenAdded)
        {
            QuestionModified = questionsModified;
            QuestionDeleted = questionsDeleted;
            TemplateDeleted = templateDeleted;
            QuestionsMayBeenAdded = questionsMayBeenAdded;
        }
    }
}
