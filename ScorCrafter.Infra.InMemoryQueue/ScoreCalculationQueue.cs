namespace ScorCrafter.Infra.InMemoryQueue;

using ScoreCrafter.Application.Abstraction.Queue;

using System.Threading.Channels;

public sealed class ScoreCalculationQueue : IScoreCalculationQueue
{
    private readonly Channel<ScoreCalculationItem> _channel;

    public ScoreCalculationQueue()
    {
        _channel = Channel.CreateUnbounded<ScoreCalculationItem>(
            new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
    }

    public ValueTask EnqueueAsync(
        ScoreCalculationItem item,
        CancellationToken cancellationToken = default)
    {
        return _channel.Writer.WriteAsync(
            item,
            cancellationToken);
    }

    public IAsyncEnumerable<ScoreCalculationItem> ReadAllAsync(
        CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}