using BudgetFlow.Domain.Enums;

namespace BudgetFlow.Domain.Entities;

public class BudgetAlert
{
    public Guid Id { get; set; }
    public Guid BudgetId { get; set; }
    public AlertStatus Status { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime? SentAt { get; set; }
    public Budget Budget { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
