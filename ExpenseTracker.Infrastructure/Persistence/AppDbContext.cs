using ExpenseTracker.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExpenseTracker.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Expense -> User (Required)
            modelBuilder.Entity<Expense>()
                //Each expense belongs to a user
                .HasOne(e => e.User)
                //Each user can have many expenses.
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserId);
                //.OnDelete(DeleteBehavior.Cascade);

            //Expense -> Category (Optional)
            modelBuilder.Entity<Expense>()
                .HasOne(e => e.Category)
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId)
                //.OnDelete(DeleteBehavior.SetNull);
                //CategoryId is optional
                .IsRequired(false);

            //Category -> CreatedByUser (Optional)
            modelBuilder.Entity<Category>()
                .HasOne(c => c.CreatedByUser)
                .WithMany()
                .HasForeignKey(c => c.CreatedByUserId)
                //.OnDelete(DeleteBehavior.SetNull);
                //CreatedByUserId is optional
                .IsRequired(false);

            //Decimal precision for Expense.Amount
            modelBuilder.Entity<Expense>()
                .Property(e => e.Amount)
                .HasPrecision(18, 2); // Prevent silent truncation
        }

    }
}
