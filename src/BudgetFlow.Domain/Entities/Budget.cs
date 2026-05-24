using BudgetFlow.Domain.Enums;

namespace BudgetFlow.Domain.Entities;

public class Budget
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal MonthlyLimit { get; set; }
    public decimal CurrentSpending { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
    public ICollection<BudgetAlert> Alerts { get; set; } = new List<BudgetAlert>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
