using Forms.Data;
using Forms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Forms.Pages.App
{
    [Authorize]
    public class PersonalPageModel : PageModel
    {
        public class TemplateDisplay
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Author {  get; set; }
        }

        public class FormDisplay
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Author { get; set;}
            public int TemplateId { get; set; }
        }

        private readonly TemplateService _templateService;
        private readonly FormsService _formService;

        private List<Template> Templates { get; set; }
        private List<Data.Forms> Forms { get; set;}

        public List<TemplateDisplay> TemplateDisplayList { get; set; } = new List<TemplateDisplay>();
        public List<FormDisplay> FormDisplayList { get; set; } = new List<FormDisplay>();

        public PersonalPageModel(TemplateService templateService, FormsService formService)
        {
            _templateService = templateService;
            _formService = formService;
        }

        public void OnGet()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Templates = _templateService.GetTemplateList(userId);
            Forms = _formService.GetFormList(userId);
            CreateDisplayData();
        }


        public JsonResult OnPostDelete(int id)
        {
            Console.WriteLine("pst dlt jsn: " + id);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Template template = _templateService.GetTemplateById(id);
            if (!_templateService.IsAuthorized(User, template))
            {
                return new JsonResult(new { success = false, message = "Forbidden. Unauthorized" });
            }

            _templateService.DeleteTemplate(template);


            return new JsonResult(new { success = true, message = "Rows processed successfully!" });
        }

        public JsonResult OnPostDeleteForm(int id)
        {
            Console.WriteLine("pst dlt form: " + id);
            Data.Forms form = _formService.GetForm(id);
            if (!_formService.IsAuthorized(User, form))
            {
                return new JsonResult(new { success = false, message = "Forbidden. Unauthorized" });
            }
            _formService.DeleteForm(form);
            return new JsonResult(new { success = true, message = "Rows processed successfully!" });

        }

        private void CreateDisplayData()
        {
            foreach (Template template in Templates)
            {
                TemplateDisplay templateDisplay = new TemplateDisplay()
                {
                    Id = template.Id,
                    Title = template.Title,
                    Description = template.Description,
                    Author = template.Author
                };
                TemplateDisplayList.Add(templateDisplay);
            }

            foreach (Data.Forms forms in Forms)
            {
                Template templateForForm = _templateService.GetTemplateById(forms.TemplateId);
                FormDisplay formDisplay = new FormDisplay()
                {
                    Id = forms.Id,
                    Title = templateForForm.Title,
                    Description = templateForForm.Description,
                    Author = templateForForm.Author,
                    TemplateId = forms.TemplateId
                };
                FormDisplayList.Add(formDisplay);
            }
        }
    }
}
