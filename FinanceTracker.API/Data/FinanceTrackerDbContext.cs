using FinanceTracker.API.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.API.Data
{
    public class FinanceTrackerDbContext : DbContext
    {
        // Constructor
        public FinanceTrackerDbContext(DbContextOptions<FinanceTrackerDbContext> options)
            : base(options)
        {
        }

        // DbSets for your entities
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }

        // OnModelCreating method to configure the model
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //UserProfile
            


            //// User
            //modelBuilder.Entity<User>(entity =>
            //{
            //    entity.HasKey(u => u.Id);

            //    entity.HasMany(c => c.Categories)
            //        .WithOne(u => u.User)
            //        .HasForeignKey(c => c.UserId)
            //        .OnDelete(DeleteBehavior.Cascade);

            //    entity.HasMany(t => t.Transactions)
            //        .WithOne(u => u.User)
            //        .HasForeignKey(t => t.UserId)
            //        .OnDelete(DeleteBehavior.Cascade);

            //    entity.HasMany(b => b.Budgets)
            //        .WithOne(u => u.User)
            //        .HasForeignKey(b => b.UserId)
            //        .OnDelete(DeleteBehavior.Cascade);
            //});

            // Category 
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasMany(b => b.Budgets)
                .WithOne(c => c.Category)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(c => c.Transactions)
                .WithOne(t => t.Category)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

                entity.Property(t => t.Type)
                .HasConversion<string>();
            });
            // BUDGET
            modelBuilder.Entity<Budget>(entity => { 
                
                entity.HasKey(b => b.Id);

                entity.Property(b => b.LimitAmount)
                .HasColumnType("decimal(18,2)");
            });
                

           
        }
    }
}
