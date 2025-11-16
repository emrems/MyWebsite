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

    }
}
