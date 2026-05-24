using BudgetFlow.Domain.Enums;

namespace BudgetFlow.Application.DTOs;

public class CreateBudgetRequest
{
    public Guid UserId { get; set; }
    public ExpenseCategory Category { get; set; }
    public decimal MonthlyLimit { get; set; }
}
