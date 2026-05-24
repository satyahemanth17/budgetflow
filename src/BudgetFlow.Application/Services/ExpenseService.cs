using AutoMapper;
using BudgetFlow.Application.DTOs;
using BudgetFlow.Application.Interfaces;
using BudgetFlow.Domain.Entities;
using BudgetFlow.Domain.Enums;

namespace BudgetFlow.Application.Services;

public class ExpenseService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ExpenseService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ExpenseDto> CreateExpenseAsync(CreateExpenseRequest request)
    {
        var budget = await _unitOfWork.Budgets.GetByCategoryAsync(request.UserId, request.Category);
        if (budget != null && budget.CurrentSpending + request.Amount > budget.MonthlyLimit)
            throw new InvalidOperationException(
                $"Expense of {request.Amount:C} would exceed the monthly budget limit of {budget.MonthlyLimit:C}.");

        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Description = request.Description,
            Amount = request.Amount,
            Category = request.Category,
            ExpenseDate = request.ExpenseDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        if (budget != null)
        {
            expense.BudgetId = budget.Id;
            budget.CurrentSpending += request.Amount;
            budget.UpdatedAt = DateTime.UtcNow;

            if (budget.CurrentSpending >= budget.MonthlyLimit * 0.8m)
            {
                budget.Alerts.Add(new BudgetAlert
                {
                    Id = Guid.NewGuid(),
                    BudgetId = budget.Id,
                    Status = AlertStatus.Pending,
                    Message = $"Spending has reached {budget.CurrentSpending / budget.MonthlyLimit * 100:F1}% of the monthly limit.",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            await _unitOfWork.Budgets.UpdateAsync(budget);
        }

        await _unitOfWork.Expenses.AddAsync(expense);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<ExpenseDto>(expense);
    }

    public async Task<ExpenseDto?> GetExpenseAsync(Guid id)
    {
        var expense = await _unitOfWork.Expenses.GetByIdAsync(id);
        return expense is null ? null : _mapper.Map<ExpenseDto>(expense);
    }

    public async Task<IEnumerable<ExpenseDto>> GetUserExpensesAsync(Guid userId)
    {
        var expenses = await _unitOfWork.Expenses.GetAllByUserAsync(userId);
        return _mapper.Map<IEnumerable<ExpenseDto>>(expenses);
    }

    public async Task DeleteExpenseAsync(Guid id)
    {
        var expense = await _unitOfWork.Expenses.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Expense {id} not found.");
        expense.IsDeleted = true;
        expense.UpdatedAt = DateTime.UtcNow;
        await _unitOfWork.Expenses.UpdateAsync(expense);
        await _unitOfWork.SaveChangesAsync();
    }
}
