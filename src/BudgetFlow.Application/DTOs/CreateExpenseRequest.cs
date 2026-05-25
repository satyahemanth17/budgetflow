using BudgetFlow.Domain.Enums;

namespace BudgetFlow.Application.DTOs;

public class CreateExpenseRequest
{
    public Guid UserId { get; set; }
    public Guid? BudgetId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public ExpenseCategory Category { get; set; }
    public DateTime ExpenseDate { get; set; }
}
