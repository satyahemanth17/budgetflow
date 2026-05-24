using BudgetFlow.Application.Interfaces;

namespace BudgetFlow.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
        Expenses = new ExpenseRepository(db);
        Budgets = new BudgetRepository(db);
    }

    public IExpenseRepository Expenses { get; }
    public IBudgetRepository Budgets { get; }

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _db.SaveChangesAsync(ct);

    public void Dispose() => _db.Dispose();
}
