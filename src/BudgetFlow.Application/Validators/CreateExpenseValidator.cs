using BudgetFlow.Application.DTOs;
using FluentValidation;

namespace BudgetFlow.Application.Validators;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseRequest>
{
    public CreateExpenseValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ExpenseDate).NotEmpty();
    }
}
