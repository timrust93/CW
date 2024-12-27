using Forms.Data;
using Forms.Data.Migrations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

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

        public Data.Forms? GetForm(int templateId)
        {            
            var form = _appDbContext.Forms.Include(f => f.Answers)
                .FirstOrDefault(x => x.TemplateId == templateId);
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
    }
}
