using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BudgetFlow.Functions;

public class BudgetAlertFunction
{
    private readonly ILogger<BudgetAlertFunction> _logger;

    public BudgetAlertFunction(ILogger<BudgetAlertFunction> logger) =>
        _logger = logger;

    [Function("BudgetAlertFunction")]
    public void Run([TimerTrigger("0 */5 * * * *")] TimerInfo timer)
    {
        _logger.LogInformation("BudgetAlertFunction fired at {time}", DateTime.UtcNow);
        // Check all budgets at 80%+ threshold and send alerts via ServiceBusPublisher.
    }
}
