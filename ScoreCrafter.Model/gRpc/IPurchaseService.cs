namespace ScoreCrafter.Api.gRpc.Contracts;

using ProtoBuf;
using ProtoBuf.Grpc;

using System.ServiceModel;


[ServiceContract]
public interface IPurchaseService
{
    [OperationContract]
    Task<RegisterPurchaseResponse> RegisterPurchase(
        RegisterPurchaseRequest request,
        CallContext context = default);
}

[ProtoContract]
public sealed class RegisterPurchaseRequest
{
    [ProtoMember(1)]
    public Guid PurchaseId { get; set; }

    [ProtoMember(2)]
    public Guid UserId { get; set; }

    [ProtoMember(3)]
    public decimal Amount { get; set; }

    [ProtoMember(4)]
    public DateTime PurchaseDate { get; set; }

    [ProtoMember(5)]
    public Dictionary<string, string> Metadata { get; set; } = new();
}

[ProtoContract]
public sealed class RegisterPurchaseResponse
{
    [ProtoMember(1)]
    public bool Success { get; set; }
}