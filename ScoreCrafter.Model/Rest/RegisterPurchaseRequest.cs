using System;
using System.Collections.Generic;
using System.Text;

namespace ScoreCrafter.SDK.Model.Rest;

public sealed record RegisterPurchaseRequest(
    Guid PurchaseId,
    Guid UserId,
    decimal Amount,
    DateTime PurchaseDate,
    Dictionary<string, string>? Metadata);
public sealed record UserSummaryResponse(
    Guid UserId,
    decimal Score,
    string? Grade,
    int PurchaseCount);
public sealed record CreateFormulaRequest(
    int GradeId,
    string Definition,
    int Version);

public sealed record UpdateFormulaRequest(
    string Recipe);

public sealed record FormulaResponse(
    Guid FormulaId,
    int GradeId,
    string Definition,
    int Version);

public sealed record TestFormulaRequest(
    Guid FormulaId,
    decimal PurchaseAmount,
    int PurchaseCount,
    int GradeId,
    int? Version = null);

public sealed record TestFormulaResponse(
    bool IsValid,
    decimal? DecimalResult,
    bool? BooleanResult,
    string? Error);

public sealed record CreateGradeRequest(
    string Name,
    string Description);

public sealed record UpdateGradeRequest(
    string Description);

public sealed record GradeResponse(
    int GradeId,
    string Name,
    string Description);

public sealed record TestGradeRequest(
    int GradeId,
    decimal PurchaseAmount,
    int PurchaseCount);

public sealed record TestGradeResponse(
    bool IsValid,
    bool Result,
    string? Error);


