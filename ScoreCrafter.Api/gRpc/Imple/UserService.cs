namespace ScoreCrafter.Api.gRpc.Imple;

using ProtoBuf.Grpc;

using ScoreCrafter.Application.Queries.User;
using ScoreCrafter.SDK.Model.gRpc.Contractc;
using ScoreCrafter.SDK.Model.gRpc.Protos;

public sealed class UserService : IUserService
{
    private readonly GetUserSummaryQueryHandler _handler;

    public UserService(
        GetUserSummaryQueryHandler handler)
    {
        _handler = handler;
    }

    public async Task<UserSummaryGrpcResponse> GetUser(
        UserSummaryGrpcRequest request,
        CallContext context = default)
    {
        var result = await _handler.Handle(
            new GetUserSummaryQuery(request.UserId),
            context.CancellationToken);

        if (result is null)
            throw new KeyNotFoundException("User not found.");

        return new UserSummaryGrpcResponse
        {
            UserId = result.UserId,
            Score = result.Score,
            Grade = result.Grade,
            PurchaseCount = result.PurchaseCount
        };
    }
}