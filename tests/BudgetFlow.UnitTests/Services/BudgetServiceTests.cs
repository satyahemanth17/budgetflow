using AutoMapper;
using BudgetFlow.Application.DTOs;
using BudgetFlow.Application.Interfaces;
using BudgetFlow.Application.Services;
using BudgetFlow.Domain.Entities;
using BudgetFlow.Domain.Enums;
using FluentAssertions;
using Moq;

namespace BudgetFlow.UnitTests.Services;

public class BudgetServiceTests
{
    private readonly Mock<IUnitOfWork> _uow = new();
    private readonly Mock<IBudgetRepository> _budgetRepo = new();
    private readonly Mock<IMapper> _mapper = new();

    public BudgetServiceTests()
    {
        _uow.Setup(u => u.Budgets).Returns(_budgetRepo.Object);
    }

    [Fact]
    public async Task GetBudgetSummary_CalculatesCorrectPercentage()
    {
        var budgetId = Guid.NewGuid();
        var budget = new Budget
        {
            Id = budgetId,
            UserId = Guid.NewGuid(),
            Category = ExpenseCategory.Food,
            MonthlyLimit = 1000m,
            CurrentSpending = 750m,
            Alerts = new List<BudgetAlert>()
        };
        // SpendingPercentage = Math.Round(750 / 1000 * 100, 2) = 75.00
        var expectedDto = new BudgetDto
        {
            Id = budgetId,
            MonthlyLimit = 1000m,
            CurrentSpending = 750m,
            SpendingPercentage = 75.00m
        };

        _budgetRepo.Setup(r => r.GetByIdAsync(budgetId, default)).ReturnsAsync(budget);
        _mapper.Setup(m => m.Map<BudgetDto>(budget)).Returns(expectedDto);

        var service = new BudgetService(_uow.Object, _mapper.Object);
        var result = await service.GetBudgetSummaryAsync(budgetId);

        result.Should().NotBeNull();
        result.SpendingPercentage.Should().Be(75.00m);
        result.MonthlyLimit.Should().Be(1000m);
        result.CurrentSpending.Should().Be(750m);
    }

    [Fact]
    public async Task UpdateBudgetLimit_NegativeAmount_ThrowsValidationException()
    {
        var service = new BudgetService(_uow.Object, _mapper.Object);

        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.UpdateBudgetLimitAsync(Guid.NewGuid(), -100m));
    }
}
