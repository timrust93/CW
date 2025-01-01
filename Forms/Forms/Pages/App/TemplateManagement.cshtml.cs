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
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using static Forms.Pages.App.CreateTemplateModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Forms.Pages.App
{
    [Authorize]
    public class TemplateManagementModel : PageModel
    {
        public class OrderChange
        {
            public int QId { get; set; }
            public int OrderIndex { get; set; }
        }

        public class FormDisplay
        {
            public string Author { get; set; } = string.Empty;
            public int Id { get; set; }
        }

        public class TemplateCreatePoco
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string Description { get; set; }
        }


        public List<Question> QuestionList { get; set; } = new List<Question>();
        public List<FormDisplay> FormDisplayList { get; set; } = new List<FormDisplay>();

        [BindProperty]
        public List<QuestionTypeInfo> QuestionTypeInfos { get; set; } = new List<QuestionTypeInfo>();
        public Template Template { get; set; }
        public TemplateCreatePoco TemplateCreatePOCO { get; set; }

        public int MaxQuestionCount { get; set; }
        public int TemplateId { get; set; }
        public List<UserAccessPOCO> UsersList { get; set; } = new List<UserAccessPOCO>();

        private readonly ApplicationDbContext _dbContext;
        private readonly TemplateService _templateService;
        private readonly FormsService _formService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TemplateManagementModel(ApplicationDbContext dbContext, TemplateService templateService,
            FormsService formService, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _templateService = templateService;
            _formService = formService;
            _userManager = userManager;
        }


        public async Task<IActionResult> OnGet(int id = 1002)
        {
            TemplateId = id;
            Template = _templateService.GetTemplateById(id);
            if (!_templateService.IsAuthorized(User, Template))
            {
                return BadRequest("Bad Request");
            }
            if (Template == null)
            {
                return NotFound();
            }

            TemplateCreatePOCO = new TemplateCreatePoco { Title = Template.Title, Description =  Template.Description };

            QuestionTypeInfos = _templateService.GetQuestionTypeCounts(Template);      
            MaxQuestionCount = QuestionTypeInfos.Sum(x => x.MaxCount);
            InitializeQuestionList(Template);
            QuestionList.Sort((x, y) => x.OrderIndex.CompareTo(y.OrderIndex));

            var forms = _formService.GetFormList(Template.Id);
            foreach (var form in forms)
            {
                FormDisplay formDisplay = new FormDisplay
                {
                    Author = form.Author,
                    Id = form.Id,
                };
                FormDisplayList.Add(formDisplay);
            }

            await InitializeUsers(Template);

            return Page();
        }

        private void InitializeQuestionList(Template template)
        {
            foreach (Question question in template.QuestionList)
            {
                QuestionList.Add(question);
            }
        }


        #region qeustions
        public JsonResult OnPostSaveQuestion([FromBody] Question question, [FromQuery] int id)
        {                        
            Template template = _templateService.GetTemplateById(id);
            if (!_templateService.IsAuthorized(User, template))
            {
                return new JsonResult(new { success = false, message = "Forbidden. Unauthorized" });
            }

            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }
            else
            {
                template.LastModified = DateTime.Now;
                Question originalQuestion = (template.QuestionList as List<Question>).Find(x => x.Id == question.Id);
                originalQuestion.Title = question.Title;
                originalQuestion.Description = question.Description;
                originalQuestion.Type = question.Type;
                originalQuestion.LastModified = DateTime.Now;
                _dbContext.SaveChanges();
                return new JsonResult(new { success = true, message = "Rows processed successfully!" });
            }
        }

        public JsonResult OnPostDeleteQuestion([FromBody] Question question, [FromQuery] int id)
        {
            Template template = _templateService.GetTemplateById(id);
            if (!_templateService.IsAuthorized(User, template))
            {
                return new JsonResult(new { success = false, message = "Forbidden. Unauthorized" });
            }
            try
            {
                template.LastModified = DateTime.Now;
                (template.QuestionList as List<Question>)?.RemoveAll(x => x.Id == question.Id);
                _dbContext.SaveChanges();
                return new JsonResult(new { success = true, message = "Rows processed successfully!" });
            }
            catch
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }
        }

        public JsonResult OnPostChangeOrder([FromBody] List<OrderChange> list, [FromQuery] int id)
        {
            Template template = _templateService.GetTemplateById(id);
            if (!_templateService.IsAuthorized(User, template))
            {
                return new JsonResult(new { success = false, message = "Forbidden. Unauthorized" });
            }
            try
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Question question = template.QuestionList.FirstOrDefault(x => x.Id == list[i].QId);
                    if (question != null)
                        question.OrderIndex = list[i].OrderIndex;
                }
                _dbContext.SaveChanges();
                return new JsonResult(new { success = true, message = "Rows processed successfully!" });
            }
            catch
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }
        }

        public JsonResult OnPostUpdateTemplateInfo([FromBody] TemplateCreatePoco templatePOCO, [FromQuery] int id)
        {
            Template template = _templateService.GetTemplateById(id);
            if (!_templateService.IsAuthorized(User, template))
            {
                return new JsonResult(new { success = false, message = "Forbidden. Unauthorized" });
            }
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }

            template.LastModified = DateTime.Now;
            template.Title = templatePOCO.Title;
            template.Description = templatePOCO.Description;
            _dbContext.SaveChanges();
            return new JsonResult(new { success = true, message = "Rows processed successfully!" });
        }

        #endregion

        #region user access
        private async Task InitializeUsers(Template template)
        {            
           foreach (var ta in template.TemplateAccessList)
           {
                var user = await _userManager.FindByIdAsync(ta.UserId);
                if (user == null)
                    continue;
                UsersList.Add(new UserAccessPOCO()
                {
                    UserId = ta.UserId,
                    Email = user.Email!
                });
           }
        }

        public async Task<JsonResult> OnGetSearchUsers(string query, int templateId)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }
            Template template = _templateService.GetTemplateById(templateId);            
            if (!_templateService.IsAuthorized(User, template))
            {
                return new JsonResult(new { success = false, message = "Not authorized" });
            }            

            var users = await _dbContext.Users
                .Where(u => u.Email.Contains(query) && u.Email != template.Author)
                .OrderBy(x => x.Email)
                .Take(10) // Limit the results
                .Select(u => new { UserId = u.Id, u.Email })
                .ToListAsync();
            

            return new JsonResult(users);
        }

        public async Task<JsonResult> OnPostAddUserToTemplate([FromBody] UserAccessPOCO userAccess, int templateId)
        {
            Console.WriteLine("add for: " + templateId);
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }

            Template template = _templateService.GetTemplateById(templateId);
            if (!_templateService.IsAuthorized(User, template))
            {
                return new JsonResult(new { success = false, message = "Not authorized" });
            }

            TemplateAccess templateAccess = template.TemplateAccessList.FirstOrDefault(x => x.UserId == userAccess.UserId);
            if (templateAccess != null)
            {
                return new JsonResult(new { success = false, message = "This user already has access" });
            }            
            templateAccess = new TemplateAccess();
            templateAccess.TemplateId = templateId;
            templateAccess.UserId = userAccess.UserId;
            template.TemplateAccessList.Add(templateAccess);
            await _dbContext.SaveChangesAsync();            
            return new JsonResult(new { success = true, message = "User added" });
        }

        public async Task<JsonResult> OnPostDeleteUserFromTemplate([FromBody] UserAccessPOCO userAccess, [FromQuery] int templateId)
        {
            Console.WriteLine("delete for: " + templateId);
            foreach (var state in ModelState)
            {
                var key = state.Key; // This is the name of the field
                var value = state.Value;

                if (value.Errors.Any())
                {
                    foreach (var error in value.Errors)
                    {
                        // You can log the error or inspect it
                        Console.WriteLine($"Field: {key}, Error: {error.ErrorMessage}");
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }
            Template template = _templateService.GetTemplateById(templateId);
            if (!_templateService.IsAuthorized(User, template))
            {
                return new JsonResult(new { success = false, message = "Not authorized" });
            }

            TemplateAccess ta = template.TemplateAccessList.FirstOrDefault(x => x.UserId == userAccess.UserId);
            template.TemplateAccessList.Remove(ta);
            _dbContext.SaveChangesAsync();

            
            return new JsonResult(new { success = true, message = "User added" });
        }

        public async Task<JsonResult> OnPostChangePrivacy(bool isPublic, int templateId)
        {
            Console.WriteLine("on change privacey: " + isPublic + ". for: " + templateId);
            if (!ModelState.IsValid)
            {
                return new JsonResult(new { success = false, message = "Data violation" });
            }
            Template template = _templateService.GetTemplateById(templateId);
            if (!_templateService.IsAuthorized(User, template))
            {
                return new JsonResult(new { success = false, message = "Not authorized" });
            }

            template.IsPublic = isPublic;
            _dbContext.SaveChanges();            
            return new JsonResult(new { success = true, message = "User added" });
        }
        #endregion
    }
}
