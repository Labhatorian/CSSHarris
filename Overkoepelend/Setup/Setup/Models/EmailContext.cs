using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Setup.Models.DeveloperModels;

namespace Setup.Models
{
    public class EmailContext : DbContext
    {
        public DbSet<Email> Email { get; set; }

        public EmailContext(DbContextOptions<EmailContext> options) : base(options) { }
    }
}