using AutoMapper;
using BudgetFlow.Application.DTOs;
using BudgetFlow.Application.Interfaces;
using BudgetFlow.Application.Services;
using BudgetFlow.Domain.Entities;
using BudgetFlow.Domain.Enums;
using FluentAssertions;
using Moq;

namespace BudgetFlow.UnitTests.Services;

public class ExpenseServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IExpenseRepository> _expenseRepo = new();
    private readonly Mock<IBudgetRepository> _budgetRepo = new();
    private readonly Mock<IMapper> _mapper = new();

    public ExpenseServiceTests()
    {
        _uow.Setup(u => u.Expenses).Returns(_expenseRepo.Object);
        _uow.Setup(u => u.Budgets).Returns(_budgetRepo.Object);
    }

    [Fact]
    public async Task CreateExpense_ValidInput_ReturnsExpenseDto()
    {
        var userId = Guid.NewGuid();
        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Category = ExpenseCategory.Food,
            MonthlyLimit = 1000m,
            CurrentSpending = 100m,
            Alerts = new List<BudgetAlert>()
        };
        var expectedDto = new ExpenseDto
        {
            Description = "Groceries",
            Amount = 50m,
            Category = ExpenseCategory.Food
        };

        _budgetRepo
            .Setup(r => r.GetByCategoryAsync(userId, ExpenseCategory.Food, default))
            .ReturnsAsync(budget);
        _budgetRepo
            .Setup(r => r.UpdateAsync(It.IsAny<Budget>(), default))
            .Returns(Task.CompletedTask);
        _expenseRepo
            .Setup(r => r.AddAsync(It.IsAny<Expense>(), default))
            .Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);
        _mapper
            .Setup(m => m.Map<ExpenseDto>(It.IsAny<Expense>()))
            .Returns(expectedDto);

        var service = new ExpenseService(_uow.Object, _mapper.Object);
        var request = new CreateExpenseRequest
        {
            UserId = userId,
            Description = "Groceries",
            Amount = 50m,
            Category = ExpenseCategory.Food,
            ExpenseDate = DateTime.UtcNow
        };

        var result = await service.CreateExpenseAsync(request);

        result.Should().NotBeNull();
        result.Description.Should().Be("Groceries");
        result.Amount.Should().Be(50m);
    }

    [Fact]
    public async Task CreateExpense_ExceedsBudget_ThrowsException()
    {
        var userId = Guid.NewGuid();
        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Category = ExpenseCategory.Food,
            MonthlyLimit = 100m,
            CurrentSpending = 90m,
            Alerts = new List<BudgetAlert>()
        };

        _budgetRepo
            .Setup(r => r.GetByCategoryAsync(userId, ExpenseCategory.Food, default))
            .ReturnsAsync(budget);

        var service = new ExpenseService(_uow.Object, _mapper.Object);
        var request = new CreateExpenseRequest
        {
            UserId = userId,
            Description = "Expensive item",
            Amount = 20m,
            Category = ExpenseCategory.Food,
            ExpenseDate = DateTime.UtcNow
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CreateExpenseAsync(request));
    }

    [Fact]
    public async Task DeleteExpense_SoftDeletesRecord()
    {
        var expenseId = Guid.NewGuid();
        var expense = new Expense { Id = expenseId, IsDeleted = false };

        _expenseRepo
            .Setup(r => r.GetByIdAsync(expenseId, default))
            .ReturnsAsync(expense);
        _expenseRepo
            .Setup(r => r.UpdateAsync(It.IsAny<Expense>(), default))
            .Returns(Task.CompletedTask);
        _uow.Setup(u => u.SaveChangesAsync(default)).ReturnsAsync(1);

        var service = new ExpenseService(_uow.Object, _mapper.Object);
        await service.DeleteExpenseAsync(expenseId);

        expense.IsDeleted.Should().BeTrue();
        _expenseRepo.Verify(r => r.UpdateAsync(It.IsAny<Expense>(), default), Times.Once);
    }
}
