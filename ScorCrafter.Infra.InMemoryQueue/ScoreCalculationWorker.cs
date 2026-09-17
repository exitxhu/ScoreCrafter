using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using ScoreCrafter.Application.Abstraction.Queue;
using ScoreCrafter.Application.Commands.User;

namespace ScoreCrafter.Infra.InMemoryQueue;

public sealed class ScoreCalculationWorker : BackgroundService
{
    private readonly IScoreCalculationQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ScoreCalculationWorker> _logger;

    public ScoreCalculationWorker(
        IScoreCalculationQueue queue,
        IServiceScopeFactory scopeFactory,
        ILogger<ScoreCalculationWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        await foreach (var item in _queue.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();

                var handler =
                    scope.ServiceProvider
                        .GetRequiredService<CalculateUserScoreCommandHandler>();

                await handler.Handle(
                    new CalculateUserScoreCommand(
                        item.UserId,
                        item.PurchaseId),
                    stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to calculate score for User {UserId}, Purchase {PurchaseId}",
                    item.UserId,
                    item.PurchaseId);
            }
        }
    }
}