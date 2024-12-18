using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Forms.Pages.App
{

    public class TemplateManagementModel : PageModel
    {
        public class TestQuestion
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string Description { get; set; }
        }

        [BindProperty]
        public List<TestQuestion> TestQuestionList { get; set; } = new List<TestQuestion>();
        

        public void OnGet(int id)
        {
            TestQuestionList.Add(new TestQuestion { Title = "Q1", Description = "Q1 Descr" });
            TestQuestionList.Add(new TestQuestion { Title = "Q2", Description = "Q2 Descr" });
            TestQuestionList.Add(new TestQuestion { Title = "Q3", Description = "Q3 Descr" });
            Console.WriteLine("id: " + id);
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
            Console.WriteLine("id: " + id);
            return Page(); // Or any response logic
        }
    }
}
