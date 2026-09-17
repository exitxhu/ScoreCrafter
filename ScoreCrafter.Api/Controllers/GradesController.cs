using Microsoft.AspNetCore.Mvc;

using ScoreCrafter.Application.Commands.Grade;
using ScoreCrafter.Application.Dtos;
using ScoreCrafter.SDK.Model.Rest;

namespace ScoreCrafter.Api.Controllers;

[ApiController]
[Route("api/grades")]
public sealed class GradesController : ControllerBase
{
    private readonly CreateGradeCommandHandler _create;
    private readonly UpdateGradeCommandHandler _update;
    private readonly DeleteGradeCommandHandler _delete;

    public GradesController(
        CreateGradeCommandHandler create,
        UpdateGradeCommandHandler update,
        DeleteGradeCommandHandler delete)
    {
        _create = create;
        _update = update;
        _delete = delete;
    }

    [HttpPost]
    public async Task<ActionResult<GradeResponse>> Create(
        CreateGradeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _create.Handle(
            new CreateGradeCommand(
                request.Name,
                request.Description),
            cancellationToken);

        return Ok(ToResponse(result));
    }

    [HttpPut("{gradeId:guid}")]
    public async Task<ActionResult<GradeResponse>> Update(
        int gradeId,
        UpdateGradeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _update.Handle(
            new UpdateGradeCommand(
                gradeId,
                request.Description),
            cancellationToken);

        return Ok(ToResponse(result));
    }


    [HttpDelete("{gradeId:guid}")]
    public async Task<IActionResult> Delete(
        int gradeId,
        CancellationToken cancellationToken)
    {
        await _delete.Handle(
            new DeleteGradeCommand(gradeId),
            cancellationToken);

        return NoContent();
    }

    private static GradeResponse ToResponse(
        GradeDto result)
        => new(
            result.GradeId,
            result.Name,
            result.Description);
}