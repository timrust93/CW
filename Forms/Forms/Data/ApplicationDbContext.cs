using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Forms.Data;

namespace Forms.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Template> Templates { get; set; }
        public DbSet<Forms> Forms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Template>()
                .HasMany(t => t.QuestionList)
                .WithOne()
                .HasForeignKey(q  => q.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Template>()
                .HasMany(t => t.TemplateAccessList)
                .WithOne()
                .HasForeignKey(t => t.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);


            // Forms -> Answers (Cascade Delete)
            modelBuilder.Entity<Forms>()
                .HasMany(f => f.Answers)
                .WithOne()
                .HasForeignKey(a => a.FormsId)
                .OnDelete(DeleteBehavior.Cascade);            
        }
    }
}
