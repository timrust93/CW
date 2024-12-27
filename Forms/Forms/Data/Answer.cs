namespace Forms.Data
{
    public class Answer
    {
        public int Id { get; set; }
        public int FormsId { get; set; }
        public int QuestionId { get; set; }
        
        public int IntAnswer {  get; set; }
        public bool BoolAnswer { get; set; }
        public string StringAnswer { get; set; } = string.Empty;
        public DateTime LastModified { get; set; }
        public bool NonValid { get; set; } = false;
    }
}
