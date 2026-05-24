using BudgetFlow.Domain.Enums;

namespace BudgetFlow.Application.DTOs;

public class BudgetDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal MonthlyLimit { get; set; }
    public decimal CurrentSpending { get; set; }
    public decimal SpendingPercentage { get; set; }
    public DateTime CreatedAt { get; set; }
}
