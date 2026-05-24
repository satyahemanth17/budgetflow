using BudgetFlow.Domain.Entities;
using HotChocolate.Types;

namespace BudgetFlow.API.GraphQL.Types;

public class ExpenseType : ObjectType<Expense>
{
    protected override void Configure(IObjectTypeDescriptor<Expense> descriptor)
    {
        descriptor.Field(e => e.Id);
        descriptor.Field(e => e.UserId);
        descriptor.Field(e => e.BudgetId);
        descriptor.Field(e => e.Description);
        descriptor.Field(e => e.Amount);
        descriptor.Field(e => e.Category);
        descriptor.Field(e => e.ExpenseDate);
        descriptor.Field(e => e.CreatedAt);
        descriptor.Ignore(e => e.User);
        descriptor.Ignore(e => e.Budget);
        descriptor.Ignore(e => e.IsDeleted);
        descriptor.Ignore(e => e.UpdatedAt);
    }
}
