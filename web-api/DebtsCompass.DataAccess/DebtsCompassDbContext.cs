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
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
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

            modelBuilder.Entity<Friendship>().HasKey(f => new { f.RequesterUserId, f.SelectedUserId });

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.RequesterUser)
                .WithMany()
                .HasForeignKey(f => f.RequesterUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.SelectedUser)
                .WithMany()
                .HasForeignKey(f => f.SelectedUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Expense>()
                .HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Expense>().Property(e => e.Note).IsRequired(false);

            modelBuilder.Entity<ExpenseCategory>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExpenseCategory>().HasData(
                new ExpenseCategory { Id= Guid.NewGuid(), Name = "Food" },
                new ExpenseCategory { Id = Guid.NewGuid(), Name = "Clothes" },
                new ExpenseCategory { Id = Guid.NewGuid(), Name = "Invoices" },
                new ExpenseCategory { Id = Guid.NewGuid(), Name = "Rent" },
                new ExpenseCategory { Id = Guid.NewGuid(), Name = "Car" }
                );
        }
    }
}
