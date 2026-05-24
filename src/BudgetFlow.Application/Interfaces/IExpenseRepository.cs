using BudgetFlow.Domain.Entities;

namespace BudgetFlow.Application.Interfaces;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Expense>> GetAllByUserAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Expense expense, CancellationToken ct = default);
    Task UpdateAsync(Expense expense, CancellationToken ct = default);
}
