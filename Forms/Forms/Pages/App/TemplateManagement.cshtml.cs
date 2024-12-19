using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static Forms.Pages.App.TemplateManagementModel;

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

        public class QuestionTypeInfo
        {
            public int Id { get; set; }
            public string DisplayName { get; set; }            
            public int MaxCount { get; set; }
            public int Left { get; set; }
        }

        [BindProperty]
        public List<TestQuestion> TestQuestionList { get; set; } = new List<TestQuestion>();

        public List<QuestionTypeInfo> QuestionTypeInfos { get; set; } = new List<QuestionTypeInfo>();
        

        public void OnGet(int id)
        {
            QuestionTypeInfos.Add(new QuestionTypeInfo { Id = 0, DisplayName = "Single Line", MaxCount = 4, Left = 1 });
            QuestionTypeInfos.Add(new QuestionTypeInfo { Id = 1, DisplayName = "Multi Line", MaxCount = 4, Left = 1 });
            QuestionTypeInfos.Add(new QuestionTypeInfo { Id = 2, DisplayName = "Number", MaxCount = 4, Left = 1 });
            QuestionTypeInfos.Add(new QuestionTypeInfo { Id = 3, DisplayName = "Checkbox", MaxCount = 4, Left = 1 });

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
