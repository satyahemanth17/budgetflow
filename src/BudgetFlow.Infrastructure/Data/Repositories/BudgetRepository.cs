using BudgetFlow.Application.Interfaces;
using BudgetFlow.Domain.Entities;
using BudgetFlow.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BudgetFlow.Infrastructure.Data.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly AppDbContext _db;
    public BudgetRepository(AppDbContext db) => _db = db;

    public async Task<Budget?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _db.Budgets.Include(b => b.Alerts).FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task<IEnumerable<Budget>> GetAllByUserAsync(Guid userId, CancellationToken ct = default) =>
        await _db.Budgets.Where(b => b.UserId == userId).ToListAsync(ct);

    public async Task<Budget?> GetByCategoryAsync(Guid userId, ExpenseCategory category, CancellationToken ct = default) =>
        await _db.Budgets.FirstOrDefaultAsync(b => b.UserId == userId && b.Category == category, ct);

    public async Task AddAsync(Budget budget, CancellationToken ct = default) =>
        await _db.Budgets.AddAsync(budget, ct);

    public Task UpdateAsync(Budget budget, CancellationToken ct = default)
    {
        _db.Budgets.Update(budget);
        return Task.CompletedTask;
    }
}
