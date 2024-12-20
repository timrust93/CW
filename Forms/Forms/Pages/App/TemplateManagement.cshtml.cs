using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [BindProperty]
        public List<QuestionTypeInfo> QuestionTypeInfos { get; set; } = new List<QuestionTypeInfo>();

        public class Stuff
        {
            [Required]
            public string X { get; set; }
        }

        [BindProperty]        
        public Stuff SomeStuff { get; set; }

        public ICollection<SelectListItem> ItemsWithGroups { get; set; } = new List<SelectListItem>
        {
           new SelectListItem{Value= "js", Text="JavaScript"},
           new SelectListItem{Value= "cpp", Text="C++"},
           new SelectListItem{Value= "python", Text= "Python"},
           new SelectListItem{Value= "csharp", Text="C#"},
        };


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
