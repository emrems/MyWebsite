using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MyWebsite.Entities;

namespace MyWebsite.Repository.config
{
    public class MyWebSiteData : DbContext
    {
        public MyWebSiteData(DbContextOptions<MyWebSiteData> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ArticleLike> ArticleLikes { get; set; }
        public DbSet<Experience> Experinces { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Media> Media { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Article tablosundaki Content alanını açıkça büyük metin tipi olarak ayarlayın.
            modelBuilder.Entity<Article>()
                .Property(a => a.Content)
                .HasColumnType("nvarchar(max)"); 

          
        }
    }
}
