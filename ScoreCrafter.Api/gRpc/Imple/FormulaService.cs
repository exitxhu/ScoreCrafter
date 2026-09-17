namespace ScoreCrafter.Api.gRpc.Imple;

using ProtoBuf.Grpc;

using ScoreCrafter.Application.Commands.Formula;
using ScoreCrafter.Application.Dtos;
using ScoreCrafter.SDK.Model.gRpc.Contractc;
using ScoreCrafter.SDK.Model.gRpc.Protos;

public sealed class FormulaService : IFormulaService
{
    private readonly CreateFormulaCommandHandler _create;
    private readonly UpdateFormulaCommandHandler _update;
    private readonly DeleteFormulaCommandHandler _delete;
    private readonly TestFormulaCommandHandler _test;

    public FormulaService(
        CreateFormulaCommandHandler create,
        UpdateFormulaCommandHandler update,
        DeleteFormulaCommandHandler delete,
        TestFormulaCommandHandler test)
    {
        _create = create;
        _update = update;
        _delete = delete;
        _test = test;
    }

    public async Task<FormulaGrpcResponse> CreateFormula(
        CreateFormulaGrpcRequest request,
        CallContext context = default)
    {
        var result = await _create.Handle(
            new CreateFormulaCommand(
                request.Definition,
                request.Version),
            context.CancellationToken);

        return ToResponse(result);
    }

    public async Task<FormulaGrpcResponse> UpdateFormula(
        UpdateFormulaGrpcRequest request,
        CallContext context = default)
    {
        var result = await _update.Handle(
            new UpdateFormulaCommand(
                request.FormulaId,
                request.Definition),
            context.CancellationToken);

        return ToResponse(result);
    }

    public async Task<TestFormulaGrpcResponse> TestFormula(
        TestFormulaGrpcRequest request,
        CallContext context = default)
    {
        var result = await _test.Handle(
            new TestFormulaCommand(
                request.FormulaId,
                request.PurchaseAmount,
                request.PurchaseCount,
                request.GradeId,
                request.Version),
            context.CancellationToken);

        return new TestFormulaGrpcResponse
        {
            IsValid = result.IsValid,
            DecimalResult = result.DecimalResult,
            BooleanResult = result.BooleanResult,
            Error = result.Error
        };
    }

    public async Task DeleteFormula(
        DeleteFormulaGrpcRequest request,
        CallContext context = default)
    {
        await _delete.Handle(
            new DeleteFormulaCommand(request.FormulaId),
            context.CancellationToken);
    }

    private static FormulaGrpcResponse ToResponse(
        FormulaDto result)
        => new()
        {
            FormulaId = result.FormulaId,
            Definition = result.Definition,
            Version = result.Version
        };
}