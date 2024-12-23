
using System.Collections.ObjectModel;

namespace Forms.Data
{
    public class Template
    {
        public int Id { get; set; }
        public string OwnerId { get; set; }
        public int TopicId { get; set; }
        public bool IsPublic { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int SingleLineQLimit { get; set; } = 4;
        public int MultilineQLimint { get; set; } = 4;        
        public int NumberQLimit { get; set; } = 4;
        public int CheckboxQLimit { get; set; } = 4;
        public ICollection<Question> QuestionList { get; set; } = new List<Question>();
        public ICollection<TemplateAccess> TemplateAccessList { get; set; }
    }
}
