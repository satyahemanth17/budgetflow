using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BudgetFlow.Functions;

public class MonthlySummaryFunction
{
    private readonly ILogger<MonthlySummaryFunction> _logger;

    public MonthlySummaryFunction(ILogger<MonthlySummaryFunction> logger) =>
        _logger = logger;

    [Function("MonthlySummaryFunction")]
    public void Run([TimerTrigger("0 0 1 * *")] TimerInfo timer)
    {
        _logger.LogInformation("MonthlySummaryFunction fired at {time}", DateTime.UtcNow);
        // Aggregate monthly spending totals and publish summary events.
    }
}
