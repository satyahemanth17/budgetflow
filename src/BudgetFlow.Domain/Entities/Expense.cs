using BudgetFlow.Domain.Enums;

namespace BudgetFlow.Domain.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? BudgetId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public ExpenseCategory Category { get; set; }
    public DateTime ExpenseDate { get; set; }
    public User User { get; set; } = null!;
    public Budget? Budget { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
