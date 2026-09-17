namespace ScoreCrafter.Api.gRpc.Imple;

using ProtoBuf.Grpc;

using ScoreCrafter.Application.Commands.Grade;
using ScoreCrafter.Application.Dtos;
using ScoreCrafter.SDK.Model.gRpc.Contractc;
using ScoreCrafter.SDK.Model.gRpc.Protos;

public sealed class GradeService : IGradeService
{
    private readonly CreateGradeCommandHandler _create;
    private readonly UpdateGradeCommandHandler _update;
    private readonly DeleteGradeCommandHandler _delete;

    public GradeService(
        CreateGradeCommandHandler create,
        UpdateGradeCommandHandler update,
        DeleteGradeCommandHandler delete)
    {
        _create = create;
        _update = update;
        _delete = delete;
    }

    public async Task<GradeGrpcResponse> CreateGrade(
        CreateGradeGrpcRequest request,
        CallContext context = default)
    {
        var result = await _create.Handle(
            new CreateGradeCommand(
                request.Name,
                request.Description),
            context.CancellationToken);

        return ToResponse(result);
    }

    public async Task<GradeGrpcResponse> UpdateGrade(
        UpdateGradeGrpcRequest request,
        CallContext context = default)
    {
        var result = await _update.Handle(
            new UpdateGradeCommand(
                request.GradeId,
                request.Description),
            context.CancellationToken);

        return ToResponse(result);
    }

    public async Task DeleteGrade(
        DeleteGradeGrpcRequest request,
        CallContext context = default)
    {
        await _delete.Handle(
            new DeleteGradeCommand(request.GradeId),
            context.CancellationToken);
    }

    private static GradeGrpcResponse ToResponse(
        GradeDto result)
        => new()
        {
            GradeId = result.GradeId,
            Name = result.Name,
            Description = result.Description
        };
}