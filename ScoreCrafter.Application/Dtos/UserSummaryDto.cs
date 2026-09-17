namespace ScoreCrafter.Application.Dtos;

public sealed class UserSummaryDto
{
    public Guid UserId { get; init; }
    public decimal Score { get; init; }
    public string? Grade { get; init; }
    public int PurchaseCount { get; init; }
}

