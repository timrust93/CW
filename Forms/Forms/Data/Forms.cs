using System.Numerics;

namespace Forms.Data
{
    public class Forms
    {
        public int Id { get; set; }
        public string OwnerId { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int TemplateId { get; set; }

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public DateTime LastModified { get; set; }
    }
}
