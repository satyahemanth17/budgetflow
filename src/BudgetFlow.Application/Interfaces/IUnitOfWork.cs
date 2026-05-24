namespace BudgetFlow.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IExpenseRepository Expenses { get; }
    IBudgetRepository Budgets { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
