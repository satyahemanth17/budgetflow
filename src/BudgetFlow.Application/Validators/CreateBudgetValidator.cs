using BudgetFlow.Application.DTOs;
using FluentValidation;

namespace BudgetFlow.Application.Validators;

public class CreateBudgetValidator : AbstractValidator<CreateBudgetRequest>
{
    public CreateBudgetValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.MonthlyLimit).GreaterThan(0);
    }
}
