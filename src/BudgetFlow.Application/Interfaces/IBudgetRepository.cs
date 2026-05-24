using BudgetFlow.Domain.Entities;
using BudgetFlow.Domain.Enums;

namespace BudgetFlow.Application.Interfaces;

public interface IBudgetRepository
{
    Task<Budget?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Budget>> GetAllByUserAsync(Guid userId, CancellationToken ct = default);
    Task<Budget?> GetByCategoryAsync(Guid userId, ExpenseCategory category, CancellationToken ct = default);
    Task AddAsync(Budget budget, CancellationToken ct = default);
    Task UpdateAsync(Budget budget, CancellationToken ct = default);
}
