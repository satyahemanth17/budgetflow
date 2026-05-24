using AutoMapper;
using BudgetFlow.Application.DTOs;
using BudgetFlow.Application.Interfaces;
using BudgetFlow.Domain.Entities;

namespace BudgetFlow.Application.Services;

public class BudgetService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BudgetService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<BudgetDto> CreateBudgetAsync(CreateBudgetRequest request)
    {
        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Category = request.Category,
            MonthlyLimit = request.MonthlyLimit,
            CurrentSpending = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
        await _unitOfWork.Budgets.AddAsync(budget);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<BudgetDto>(budget);
    }

    public async Task<BudgetDto> GetBudgetSummaryAsync(Guid budgetId)
    {
        var budget = await _unitOfWork.Budgets.GetByIdAsync(budgetId)
            ?? throw new KeyNotFoundException($"Budget {budgetId} not found.");
        return _mapper.Map<BudgetDto>(budget);
    }

    public async Task<BudgetDto> UpdateBudgetLimitAsync(Guid budgetId, decimal newLimit)
    {
        if (newLimit <= 0)
            throw new ArgumentException("Budget limit must be greater than zero.", nameof(newLimit));

        var budget = await _unitOfWork.Budgets.GetByIdAsync(budgetId)
            ?? throw new KeyNotFoundException($"Budget {budgetId} not found.");

        budget.MonthlyLimit = newLimit;
        budget.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Budgets.UpdateAsync(budget);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<BudgetDto>(budget);
    }
}
