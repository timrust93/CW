using Forms.Data;
using Microsoft.EntityFrameworkCore;
using Forms.Model;

namespace Forms.Services
{
    public class TemplateService
    {
        private ApplicationDbContext _appDbContext;
        public TemplateService(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<Template> GetTemplateList()
        {
            return _appDbContext.Templates.ToList();
        }

        public Template GetTemplateById(int id)
        {
            Template? template = _appDbContext.Templates.Include(t => t.QuestionList)
                .FirstOrDefault(x => x.Id == id);
            return template;            
        }

        public List<QuestionTypeInfo> GetQuestionTypeCounts(Template template)
        {
            List<QuestionTypeInfo> questionTypeInfos  = new List<QuestionTypeInfo>();
            QuestionTypeInfo qTypeInfo1 = new QuestionTypeInfo();

            for (int i = 0; i < 4; i++)
            {
                QuestionTypeInfo qTypeInfo = new QuestionTypeInfo { Id = i };
                qTypeInfo.DisplayName = GetQuestionTypeName(i);
                qTypeInfo.MaxCount = GetQuestionTypeLimit(i);
                qTypeInfo.Left = GetQuestionTypeLeftCount(template, i);
                questionTypeInfos.Add(qTypeInfo);
            }

            return questionTypeInfos;
        }

        public int GetQuestionTypeLeftCount(Template template, int questionTypeId)
        {
            return GetQuestionTypeLimit(questionTypeId) -
                GetQuestionTypeUsedCount(template, questionTypeId);
        }

        public int GetQuestionTypeUsedCount(Template template, int questionTypeId)
        {
            return template.QuestionList.Count(x => x.Type == questionTypeId);
        }
        

        public string GetQuestionTypeName(int questionTypeId)
        {
            if (questionTypeId == 0)
                return "Single Line";
            else if (questionTypeId == 1)
                return "Multi Line";
            else if (questionTypeId == 2)
                return "Number";
            else if (questionTypeId == 3)
                return "Checkbox";
            else
                return "N/A";
        }

        public int GetQuestionTypeLimit(int questionTypeId)
        {
            if (questionTypeId == 0)
                return 1;
            else if (questionTypeId == 1)
                return 1;
            else if (questionTypeId == 2)
                return 1;
            else if (questionTypeId == 3)
                return 1;
            else
                return 0;
        }
    }
}
