using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UNN_Ki_001.Data.Models;

namespace UNN_Ki_001.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }
    }
    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.Kigyo_cd).HasMaxLength(4);
            builder.Property(u => u.Shain_no).HasMaxLength(10);
            builder.Property(u => u.Name_sei).HasMaxLength(10);
            builder.Property(u => u.Name_mei).HasMaxLength(10);
            builder.Property(u => u.Name_kana_sei).HasMaxLength(20);
            builder.Property(u => u.Name_kana_mei).HasMaxLength(20);
            builder.Property(u => u.Nyusha_dt).HasMaxLength(8);
            builder.Property(u => u.Taishoku_dt).HasMaxLength(8);
            builder.Property(u => u.Shozoku_cd).HasMaxLength(3);
            builder.Property(u => u.Shokushu_cd).HasMaxLength(2);
            builder.Property(u => u.Koyokeitai_cd).HasMaxLength(1);
            builder.Property(u => u.User_kbn).HasMaxLength(1);
            builder.Property(u => u.Valid_flg).HasMaxLength(1);
        }
    }
}