using DebtsCompass.Domain.Entities;
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
        public DebtsCompassDbContext(DbContextOptions options)
       : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
                .HasOne(da => da.NonUserDebtAssignment)
                .WithMany()
                .HasForeignKey(da => da.NonUserDebtAssignmentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DebtAssignment>()
                .HasOne(da => da.Debt)
                .WithMany(d => d.DebtAssignments)
                .HasForeignKey(da => da.DebtId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
