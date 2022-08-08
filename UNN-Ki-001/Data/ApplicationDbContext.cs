using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<m_kensakushain> shain { get; set; }
        public DbSet<shozokukensaku> shozoku { get; set; }
        public DbSet<shokushukensaku> shokushu { get; set; }
        public DbSet<koyokeitaikensaku> koyokeitai { get; set; }
    }
   
}