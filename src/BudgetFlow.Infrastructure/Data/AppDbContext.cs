using BudgetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetFlow.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<BudgetAlert> BudgetAlerts => Set<BudgetAlert>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Budget>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Expense>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<BudgetAlert>().HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.Property(x => x.Email).IsRequired().HasMaxLength(300);
            e.Property(x => x.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<Budget>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.MonthlyLimit).HasColumnType("decimal(18,2)");
            e.Property(x => x.CurrentSpending).HasColumnType("decimal(18,2)");
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Expense>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Description).IsRequired().HasMaxLength(500);
            e.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Budget).WithMany(b => b.Expenses).HasForeignKey(x => x.BudgetId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<BudgetAlert>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Message).IsRequired().HasMaxLength(500);
            e.HasOne(x => x.Budget).WithMany(b => b.Alerts).HasForeignKey(x => x.BudgetId).OnDelete(DeleteBehavior.Cascade);
        });
    }
}
