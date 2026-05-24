using BudgetFlow.Domain.Entities;
using HotChocolate.Types;

namespace BudgetFlow.API.GraphQL.Types;

public class BudgetType : ObjectType<Budget>
{
    protected override void Configure(IObjectTypeDescriptor<Budget> descriptor)
    {
        descriptor.Field(b => b.Id);
        descriptor.Field(b => b.UserId);
        descriptor.Field(b => b.Category);
        descriptor.Field(b => b.MonthlyLimit);
        descriptor.Field(b => b.CurrentSpending);
        descriptor.Field(b => b.CreatedAt);
        descriptor.Ignore(b => b.User);
        descriptor.Ignore(b => b.Expenses);
        descriptor.Ignore(b => b.Alerts);
        descriptor.Ignore(b => b.IsDeleted);
        descriptor.Ignore(b => b.UpdatedAt);
    }
}
