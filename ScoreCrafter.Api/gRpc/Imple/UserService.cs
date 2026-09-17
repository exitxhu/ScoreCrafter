namespace ScoreCrafter.Api.gRpc.Imple;

using ProtoBuf.Grpc;

using ScoreCrafter.Application.Commands.User;
using ScoreCrafter.Application.Queries.User;
using ScoreCrafter.SDK.Model.gRpc.Contractc;
using ScoreCrafter.SDK.Model.gRpc.Protos;

public sealed class UserService : IUserService
{
    private readonly GetUserSummaryQueryHandler _getUserSummaryQueryHandler;
    private readonly SetUserGradeCommandHandler _setUserGradeCommandHandler;

    public UserService(
        GetUserSummaryQueryHandler getUserSummaryQueryHandler,
        SetUserGradeCommandHandler setUserGradeCommandHandler)
    {
        _getUserSummaryQueryHandler = getUserSummaryQueryHandler;
        _setUserGradeCommandHandler = setUserGradeCommandHandler;
    }

    public async Task<UserSummaryGrpcResponse> GetUser(
        UserSummaryGrpcRequest request,
        CallContext context = default)
    {
        var result = await _getUserSummaryQueryHandler.Handle(
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

    public async Task<UserGradeGrpcResponse> SetUserGrade(
        SetUserGradeGrpcRequest request,
        CallContext context = default)
    {
        var result = await _setUserGradeCommandHandler.Handle(
            new SetUserGradeCommand(
                request.UserId,
                request.GradeId),
            context.CancellationToken);

        return new UserGradeGrpcResponse
        {
            UserId = result.UserId,
            GradeId = result.GradeId,
            GradeName = result.GradeName,
            IsCurrent = result.IsCurrent
        };
    }
}