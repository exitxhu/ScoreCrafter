using Microsoft.AspNetCore.Mvc;

using ScoreCrafter.Application.Commands.Formula;
using ScoreCrafter.Application.Dtos;
using ScoreCrafter.SDK.Model.Rest;

namespace ScoreCrafter.Api.Controllers;

[ApiController]
[Route("api/formulas")]
public sealed class FormulasController : ControllerBase
{
    private readonly CreateFormulaCommandHandler _create;
    private readonly UpdateFormulaCommandHandler _update;
    private readonly DeleteFormulaCommandHandler _delete;
    private readonly TestFormulaCommandHandler _test;

    public FormulasController(
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

    [HttpPost]
    public async Task<ActionResult<FormulaResponse>> Create(
        CreateFormulaRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _create.Handle(
            new CreateFormulaCommand(
                request.Definition,
                request.Version),
            cancellationToken);

        return Ok(ToResponse(result));
    }

    [HttpPut("{formulaId:guid}")]
    public async Task<ActionResult<FormulaResponse>> Update(
        Guid formulaId,
        UpdateFormulaRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _update.Handle(
            new UpdateFormulaCommand(
                formulaId,
                request.Definition),
            cancellationToken);

        return Ok(ToResponse(result));
    }

    [HttpPost("{formulaId:guid}/test")]
    public async Task<ActionResult<TestFormulaResponse>> Test(
        Guid formulaId,
        TestFormulaRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _test.Handle(
            new TestFormulaCommand(
                formulaId,
                request.PurchaseAmount,
                request.PurchaseCount,
                request.GradeId,
                request.Version),
            cancellationToken);

        return Ok(
            new TestFormulaResponse(
                result.IsValid,
                result.DecimalResult,
                result.BooleanResult,
                result.Error));
    }

    [HttpDelete("{formulaId:guid}")]
    public async Task<IActionResult> Delete(
        Guid formulaId,
        CancellationToken cancellationToken)
    {
        await _delete.Handle(
            new DeleteFormulaCommand(formulaId),
            cancellationToken);

        return NoContent();
    }

    private static FormulaResponse ToResponse(
        FormulaDto result)
        => new(
            result.FormulaId,
            result.Definition,
            result.Version);
}