namespace Forms.Data
{
    public class Question
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OrderIndex { get; set; }

        public DateTime LastModified { get; set; }
    }
}
