namespace ScoreCrafter.Api.gRpc.Imple;

using global::ScoreCrafter.Application.Commands.Purchase;

using ProtoBuf.Grpc;

using ScoreCrafter.SDK.Model.gRpc.Contractc;
using ScoreCrafter.SDK.Model.gRpc.Protos;

public sealed class PurchaseService : IPurchaseService
{
    private readonly RegisterPurchaseCommandHandler _handler;

    public PurchaseService(
        RegisterPurchaseCommandHandler handler)
    {
        _handler = handler;
    }

    public async Task<RegisterPurchaseResponse> RegisterPurchase(
        RegisterPurchaseRequest request,
        CallContext context = default)
    {
        var command = new RegisterPurchaseCommand(
            request.PurchaseId,
            request.UserId,
            request.Amount,
            request.PurchaseDate,
            request.Metadata);

        await _handler.Handle(
            command,
            context.CancellationToken);

        return new RegisterPurchaseResponse
        {
            Success = true
        };
    }
}
