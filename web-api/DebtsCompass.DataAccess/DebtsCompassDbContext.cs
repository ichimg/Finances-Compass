using DebtsCompass.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DebtsCompass.DataAccess
{
    public class DebtsCompassDbContext : IdentityDbContext<User>
    {
        public DbSet<Debt> Debts { get; set; }
        public DbSet<User> Users { get; set; }
        public DebtsCompassDbContext(DbContextOptions options)
       : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Debt>()
                .HasOne(e => e.CreatorUser)
                .WithMany(u => u.Debts)
                .HasForeignKey(e => e.CreatorUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Debt>()
                .HasOne(e => e.SelectedUser)
                .WithMany()
                .HasForeignKey(e => e.SelectedUserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
