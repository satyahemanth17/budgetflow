using BudgetFlow.Application.DTOs;
using BudgetFlow.Application.Services;

namespace BudgetFlow.API.GraphQL;

public class Mutation
{
    public async Task<ExpenseDto> CreateExpense([Service] ExpenseService service, CreateExpenseRequest request) =>
        await service.CreateExpenseAsync(request);

    public async Task<bool> DeleteExpense([Service] ExpenseService service, Guid id)
    {
        await service.DeleteExpenseAsync(id);
        return true;
    }

    public async Task<bool> UpdateBudgetLimit([Service] BudgetService service, Guid budgetId, decimal newLimit)
    {
        await service.UpdateBudgetLimitAsync(budgetId, newLimit);
        return true;
    }
}
