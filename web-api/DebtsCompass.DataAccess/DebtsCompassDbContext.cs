using DebtsCompass.Domain.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DebtsCompass.DataAccess
{
    public class DebtsCompassDbContext : IdentityDbContext<User>
    {
        public DbSet<Debt> Debts { get; set; }
        public DbSet<DebtAssignment> DebtAssignments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<NonUser> NonUsers { get; set; }
        public DebtsCompassDbContext(DbContextOptions options)
       : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignore redundant Identity tables
            base.OnModelCreating(modelBuilder);
            modelBuilder.Ignore<IdentityRole>();
            modelBuilder.Ignore<IdentityUserToken<string>>();
            modelBuilder.Ignore<IdentityUserRole<string>>();
            modelBuilder.Ignore<IdentityUserLogin<string>>();
            modelBuilder.Ignore<IdentityUserClaim<string>>();
            modelBuilder.Ignore<IdentityRoleClaim<string>>();
            modelBuilder.Entity<User>()
                .Ignore(c => c.AccessFailedCount)
                .Ignore(c => c.LockoutEnabled)
                .Ignore(c => c.TwoFactorEnabled)
                .Ignore(c => c.LockoutEnd)
                .Ignore(c => c.PhoneNumberConfirmed);

            modelBuilder.Entity<User>().ToTable("Users");

            modelBuilder.Entity<DebtAssignment>()
                   .HasOne(da => da.CreatorUser)
                   .WithMany(u => u.CreatedDebts)
                   .HasForeignKey(da => da.CreatorUserId)
                   .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DebtAssignment>()
                .HasOne(da => da.SelectedUser)
                .WithMany(u => u.DebtsAssigned)
                .HasForeignKey(da => da.SelectedUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DebtAssignment>()
                .HasOne(da => da.NonUser)
                .WithMany()
                .HasForeignKey(da => da.NonUserDebtAssignmentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DebtAssignment>()
                .HasOne(da => da.Debt)
                .WithMany(d => d.DebtAssignments)
                .HasForeignKey(da => da.DebtId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Friendship>().HasKey(f => new { f.UserOneId, f.UserTwoId });

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.UserOne)
                .WithMany()
                .HasForeignKey(f => f.UserOneId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.UserTwo)
                .WithMany()
                .HasForeignKey(f => f.UserTwoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
