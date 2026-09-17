using ProtoBuf;
using ProtoBuf.Grpc;

using ScoreCrafter.SDK.Model.gRpc.Protos;

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace ScoreCrafter.SDK.Model.gRpc.Contractc;

[ServiceContract]
public interface IPurchaseService
{
    [OperationContract]
    Task<RegisterPurchaseResponse> RegisterPurchase(
        RegisterPurchaseRequest request,
        CallContext context = default);
}
[ServiceContract]
public interface IUserService
{
    [OperationContract]
    Task<UserSummaryGrpcResponse> GetUser(
        UserSummaryGrpcRequest request,
        CallContext context = default);
}
[ServiceContract]
public interface IFormulaService
{
    [OperationContract]
    Task<FormulaGrpcResponse> CreateFormula(
        CreateFormulaGrpcRequest request,
        CallContext context = default);

    [OperationContract]
    Task<FormulaGrpcResponse> UpdateFormula(
        UpdateFormulaGrpcRequest request,
        CallContext context = default);

    [OperationContract]
    Task<TestFormulaGrpcResponse> TestFormula(
        TestFormulaGrpcRequest request,
        CallContext context = default);

    [OperationContract]
    Task DeleteFormula(
        DeleteFormulaGrpcRequest request,
        CallContext context = default);
}

[ProtoContract]
public sealed class DeleteFormulaGrpcRequest
{
    [ProtoMember(1)]
    public Guid FormulaId { get; set; }
}
[ServiceContract]
public interface IGradeService
{
    [OperationContract]
    Task<GradeGrpcResponse> CreateGrade(
        CreateGradeGrpcRequest request,
        CallContext context = default);

    [OperationContract]
    Task<GradeGrpcResponse> UpdateGrade(
        UpdateGradeGrpcRequest request,
        CallContext context = default);

    [OperationContract]
    Task DeleteGrade(
        DeleteGradeGrpcRequest request,
        CallContext context = default);
}

[ProtoContract]
public sealed class DeleteGradeGrpcRequest
{
    [ProtoMember(1)]
    public int GradeId { get; set; }
}