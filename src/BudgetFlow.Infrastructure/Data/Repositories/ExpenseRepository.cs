using BudgetFlow.Application.Interfaces;
using BudgetFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetFlow.Infrastructure.Data.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly AppDbContext _db;
    public ExpenseRepository(AppDbContext db) => _db = db;

    public async Task<Expense?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.Expenses.FirstOrDefaultAsync(e => e.Id == id, ct);

    public async Task<IEnumerable<Expense>> GetAllByUserAsync(Guid userId, CancellationToken ct = default) =>
        await _db.Expenses.Where(e => e.UserId == userId).ToListAsync(ct);

    public async Task AddAsync(Expense expense, CancellationToken ct = default) =>
        await _db.Expenses.AddAsync(expense, ct);

    public Task UpdateAsync(Expense expense, CancellationToken ct = default)
    {
        _db.Expenses.Update(expense);
        return Task.CompletedTask;
    }
}
