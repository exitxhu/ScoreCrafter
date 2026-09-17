namespace ScoreCrafter.SDK.Model.gRpc.Protos;

using ProtoBuf;
using ProtoBuf.Grpc;

using System.ServiceModel;




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

[ProtoContract]
public sealed class UserSummaryGrpcRequest
{
    [ProtoMember(1)]
    public Guid UserId { get; set; }
}

[ProtoContract]
public sealed class UserSummaryGrpcResponse
{
    [ProtoMember(1)]
    public Guid UserId { get; set; }

    [ProtoMember(2)]
    public decimal Score { get; set; }

    [ProtoMember(3)]
    public string? Grade { get; set; }

    [ProtoMember(4)]
    public int PurchaseCount { get; set; }
}

[ProtoContract]
public sealed class CreateFormulaGrpcRequest
{
    [ProtoMember(1)]
    public int GradeId { get; set; }

    [ProtoMember(2)]
    public string Recipe { get; set; } = string.Empty;

    [ProtoMember(3)]
    public int Version { get; set; }
}

[ProtoContract]
public sealed class FormulaGrpcResponse
{
    [ProtoMember(1)]
    public Guid FormulaId { get; set; }

    [ProtoMember(2)]
    public int GradeId { get; set; }

    [ProtoMember(3)]
    public string Definition { get; set; } = string.Empty;

    [ProtoMember(4)]
    public int Version { get; set; }
}

[ProtoContract]
public sealed class UpdateFormulaGrpcRequest
{
    [ProtoMember(1)]
    public Guid FormulaId { get; set; }

    [ProtoMember(2)]
    public string Recipe { get; set; } = string.Empty;
}

[ProtoContract]
public sealed class TestFormulaGrpcRequest
{
    [ProtoMember(1)]
    public Guid FormulaId { get; set; }

    [ProtoMember(2)]
    public decimal PurchaseAmount { get; set; }

    [ProtoMember(3)]
    public int PurchaseCount { get; set; }

    [ProtoMember(4)]
    public int? Version { get; set; }
    public int GradeId { get; set; }
}

[ProtoContract]
public sealed class TestFormulaGrpcResponse
{
    [ProtoMember(1)]
    public bool IsValid { get; set; }

    [ProtoMember(2)]
    public decimal? DecimalResult { get; set; }

    [ProtoMember(3)]
    public bool? BooleanResult { get; set; }

    [ProtoMember(4)]
    public string? Error { get; set; }
}

[ProtoContract]
public sealed class CreateGradeGrpcRequest
{
    [ProtoMember(1)]
    public string Name { get; set; } = string.Empty;

    [ProtoMember(2)]
    public string Description { get; set; } = string.Empty;

}

[ProtoContract]
public sealed class GradeGrpcResponse
{
    [ProtoMember(1)]
    public int GradeId { get; set; }

    [ProtoMember(2)]
    public string Name { get; set; } = string.Empty;

    [ProtoMember(3)]
    public string Description { get; set; } = string.Empty;

}

[ProtoContract]
public sealed class UpdateGradeGrpcRequest
{
    [ProtoMember(1)]
    public int GradeId { get; set; }

    [ProtoMember(2)]
    public string Description { get; set; } = string.Empty;

}

[ProtoContract]
public sealed class TestGradeGrpcRequest
{
    [ProtoMember(1)]
    public Guid GradeId { get; set; }

    [ProtoMember(2)]
    public decimal PurchaseAmount { get; set; }

    [ProtoMember(3)]
    public int PurchaseCount { get; set; }
}

[ProtoContract]
public sealed class TestGradeGrpcResponse
{
    [ProtoMember(1)]
    public bool IsValid { get; set; }

    [ProtoMember(2)]
    public bool Result { get; set; }

    [ProtoMember(3)]
    public string? Error { get; set; }
}