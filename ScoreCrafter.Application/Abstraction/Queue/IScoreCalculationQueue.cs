using System;
using System.Collections.Generic;
using System.Text;

namespace ScoreCrafter.Application.Abstraction.Queue;

public interface IScoreCalculationQueue
{
    ValueTask EnqueueAsync(ScoreCalculationItem item, CancellationToken cancellationToken = default);
    IAsyncEnumerable<ScoreCalculationItem> ReadAllAsync(CancellationToken cancellationToken = default);
}

public sealed record ScoreCalculationItem(Guid UserId, Guid PurchaseId);