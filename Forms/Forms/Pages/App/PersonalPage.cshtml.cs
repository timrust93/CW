using Forms.Data;
using Forms.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Forms.Pages.App
{
    [Authorize]
    public class PersonalPageModel : PageModel
    {
        private TemplateService _formSerivce;

        public List<Template> Templates { get; set; }

        public PersonalPageModel(TemplateService formService)
        {
            _formSerivce = formService;
        }

        public void OnGet()
        {
            Templates = _formSerivce.GetTemplateList();
        }
    }
}
