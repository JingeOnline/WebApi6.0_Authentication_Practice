using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebApi_RefreshToken.Models;

namespace WebApi_RefreshToken.Services
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("tb_User").HasKey(x => x.Id);
            builder.Entity<RefreshToken>().ToTable("tb_RefreshToken").HasKey(x=>x.Id);
            builder.Entity<RefreshToken>()
                .HasOne(x=>x.User)
                .WithMany(y=>y.RefreshTokens)
                .HasForeignKey(x=>x.Fk_UserId)
                .IsRequired();
        }
    }
}
