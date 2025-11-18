using qwe.Models;
using System.Data.Entity;

namespace qwe.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<User> Users { get; set; }
    }
}