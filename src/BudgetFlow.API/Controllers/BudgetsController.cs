using BudgetFlow.Application.DTOs;
using BudgetFlow.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetsController : ControllerBase
{
    private readonly BudgetService _budgetService;

    public BudgetsController(BudgetService budgetService) => _budgetService = budgetService;

    [HttpGet("{id}/summary")]
    public async Task<ActionResult<BudgetDto>> GetSummary(Guid id) =>
        Ok(await _budgetService.GetBudgetSummaryAsync(id));

    [HttpPut("{id}/limit")]
    public async Task<IActionResult> UpdateLimit(Guid id, [FromBody] decimal newLimit)
    {
        await _budgetService.UpdateBudgetLimitAsync(id, newLimit);
        return NoContent();
    }
}
