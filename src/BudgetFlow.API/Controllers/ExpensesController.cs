using BudgetFlow.Application.DTOs;
using BudgetFlow.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BudgetFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly ExpenseService _expenseService;

    public ExpensesController(ExpenseService expenseService) => _expenseService = expenseService;

    [HttpGet("{id}")]
    public async Task<ActionResult<ExpenseDto>> Get(Guid id)
    {
        var expense = await _expenseService.GetExpenseAsync(id);
        return expense is null ? NotFound() : Ok(expense);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ExpenseDto>>> GetByUser(Guid userId) =>
        Ok(await _expenseService.GetUserExpensesAsync(userId));

    [HttpPost]
    public async Task<ActionResult<ExpenseDto>> Create(CreateExpenseRequest request)
    {
        var expense = await _expenseService.CreateExpenseAsync(request);
        return CreatedAtAction(nameof(Get), new { id = expense.Id }, expense);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _expenseService.DeleteExpenseAsync(id);
        return NoContent();
    }
}
