
namespace Forms.Data
{
    public class Template
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int TopicId { get; set; }
        public bool IsPublic { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string QStringState1 { get; set; } = string.Empty;
        public string QString1 { get; set; } = string.Empty;
        public string QStringState2 { get; set; } = string.Empty;
        public string QString2 { get; set; } = string.Empty;
        public string QStringState3 { get; set; } = string.Empty;
        public string QString3 { get; set; } = string.Empty;
        public string QStringState4 { get; set; } = string.Empty;
        public string QString4 { get; set; } = string.Empty;
        public string QMultiStringState1 { get; set; } = string.Empty;
        public string QMultiString1 { get; set; } = string.Empty;
        public string QMultiStringState2 { get; set; } = string.Empty;
        public string QMultiString2 { get; set; } = string.Empty;
        public string QMultiStringState3 { get; set; } = string.Empty;
        public string QMultiString3 { get; set; } = string.Empty;
        public string QMultiStringState4 { get; set; } = string.Empty;
        public string QMultiString4 { get; set; } = string.Empty;
        public bool QCheckboxState1 { get;set; }
        public bool QCheckbox1 { get; set; }
        public bool QCheckboxState2 { get; set; }
        public bool QCheckbox2 { get; set; }
        public bool QCheckboxState3 { get; set; }
        public bool QCheckbox3 { get; set; }
        public bool QCheckboxState4 { get; set; }
        public bool QCheckbox4 { get; set; }
        public int QIntState1 { get; set; }
        public int QInt1 { get; set; }
        public int QIntState2 { get; set; }
        public int QInt2 { get; set; }
        public int QIntState3 { get; set; }
        public int QInt3 { get; set; }
        public int QIntState4 { get; set; }
        public int QInt4 { get; set; }
        public ICollection<TemplateAccess> TemplateAccessList { get; set; }
    }
}
