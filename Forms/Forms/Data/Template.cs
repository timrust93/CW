
using System.Collections.ObjectModel;
using System.Data;

namespace Forms.Data
{
    public class Template
    {
        public int Id { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public int TopicId { get; set; }
        public bool IsPublic { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public ICollection<Question> QuestionList { get; set; } = new List<Question>();
        public ICollection<TemplateAccess> TemplateAccessList { get; set; }
        public DateTime LastModified { get; set; }
    }
}
