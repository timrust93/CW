using Forms.Data;

namespace Forms.Services
{
    public class FormService
    {
        private ApplicationDbContext _appDbContext;
        public FormService(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public List<Template> GetTemplateList()
        {
            return _appDbContext.Templates.ToList();
        }

    }
}
