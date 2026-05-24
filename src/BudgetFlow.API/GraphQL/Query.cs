using BudgetFlow.Application.DTOs;
using BudgetFlow.Application.Services;

namespace BudgetFlow.API.GraphQL;

public class Query
{
    public async Task<ExpenseDto?> GetExpense([Service] ExpenseService service, Guid id) =>
        await service.GetExpenseAsync(id);

    public async Task<IEnumerable<ExpenseDto>> GetUserExpenses([Service] ExpenseService service, Guid userId) =>
        await service.GetUserExpensesAsync(userId);

    public async Task<BudgetDto> GetBudgetSummary([Service] BudgetService service, Guid budgetId) =>
        await service.GetBudgetSummaryAsync(budgetId);
}
