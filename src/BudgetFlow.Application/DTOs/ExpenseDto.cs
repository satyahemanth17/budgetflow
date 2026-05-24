using BudgetFlow.Domain.Enums;

namespace BudgetFlow.Application.DTOs;

public class ExpenseDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid? BudgetId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public ExpenseCategory Category { get; set; }
    public DateTime ExpenseDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
