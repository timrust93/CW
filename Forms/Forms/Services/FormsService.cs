using Forms.Data;
using Forms.Data.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Security.Claims;

namespace Forms.Services
{
    public class FormsService
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly TemplateService _templateService;

        public FormsService(ApplicationDbContext appDbContext, TemplateService templateService)
        {
            _appDbContext = appDbContext;
            _templateService = templateService;
        }

        public Data.Forms? GetForm(int templateId, string userId)
        {            
            var form = _appDbContext.Forms.Include(f => f.Answers)
                .FirstOrDefault(x => x.TemplateId == templateId && x.OwnerId == userId);
            if (form == null)
            {
                return null;
            }

            Template template = _templateService.GetTemplateById(templateId);
            List<Question>? questions = template.QuestionList as List<Question>;
            List<Answer>? answers = form.Answers as List<Answer>;

            for (int i = answers.Count - 1; i >= 0; i--)
            {
                Answer answer = answers[i];
                Question question = questions.Find(x => x.Id == answer.QuestionId);
                if (question == null)
                    answers.RemoveAt(i);
            }
            _appDbContext.SaveChanges();
            return form;
        }

        public Data.Forms? GetForm(int formId)
        {
            return _appDbContext.Forms.Include(f => f.Answers)
                .FirstOrDefault(x => x.Id == formId);
        }

        public List<Data.Forms> GetFormList(int templateId)
        {            
            return _appDbContext.Forms.Where(x => x.TemplateId == templateId).ToList();         
        }

        public List<Data.Forms> GetFormList(string ownerId)
        {
            return _appDbContext.Forms.Where(x => x.OwnerId == ownerId).ToList();
        }

        public void DeleteForm(Data.Forms form)
        {
            if (form == null)
            {
                throw new ArgumentNullException(nameof(form), "The form to delete cannot be null.");
            }

            _appDbContext.Forms.Remove(form);
            _appDbContext.SaveChanges();
        }

        public Answer? GetAnswer(Data.Forms? form, int questionId)
        {
            if (form == null)
                return null;
            Answer? answer = form.Answers.FirstOrDefault(x => x.QuestionId == questionId);
            return answer;
        }

        public void AddAnswer(Answer answer, Data.Forms form, bool saveToDb = true)
        {
            form.Answers.Add(answer);
            if (saveToDb)
                _appDbContext.SaveChanges();
        }

        public void UpdateAnswer(Answer answer, bool saveToDb = true)
        {
            if (saveToDb)
                _appDbContext.SaveChanges();
        }

        public Data.Forms AddForm(Data.Forms form, bool saveToDb = true)
        {
            _appDbContext.Forms.Add(form);
            if (saveToDb)
                _appDbContext.SaveChanges();
            return form;
        }

        public bool IsAuthorized(ClaimsPrincipal cp, Data.Forms form)
        {
            if (form == null || cp == null)
                return false;
            if (form.OwnerId == cp.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return true;
            }
            return false;
        }
    }
}
