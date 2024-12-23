namespace Forms.Data
{
    public class Question
    {
        public int Id { get; set; }
        public int TemplateId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int OrderIndex { get; set; }
    }
}
