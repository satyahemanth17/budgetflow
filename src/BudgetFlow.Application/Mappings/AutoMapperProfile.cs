using AutoMapper;
using BudgetFlow.Application.DTOs;
using BudgetFlow.Domain.Entities;

namespace BudgetFlow.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Expense, ExpenseDto>();
        CreateMap<Budget, BudgetDto>()
            .ForMember(dest => dest.SpendingPercentage, opt => opt.MapFrom(src =>
                src.MonthlyLimit > 0 ? Math.Round(src.CurrentSpending / src.MonthlyLimit * 100, 2) : 0m));
    }
}
