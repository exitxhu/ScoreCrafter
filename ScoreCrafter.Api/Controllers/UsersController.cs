using Microsoft.AspNetCore.Mvc;

using ScoreCrafter.Application.Commands.User;
using ScoreCrafter.Application.Dtos;
using ScoreCrafter.Application.Queries.User;
using ScoreCrafter.SDK.Model.Rest;

namespace ScoreCrafter.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserSummaryResponse>> Get(
        Guid userId,
        [FromServices] GetUserSummaryQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new GetUserSummaryQuery(userId),
            cancellationToken);

        if (result is null)
            return NotFound();

        return Ok(
            new UserSummaryResponse(
                result.UserId,
                result.Score,
                result.Grade,
                result.PurchaseCount));
    }


    [HttpPut("{userId:guid}/grade")]
    public async Task<ActionResult<UserGradeDto>> SetGrade(
        Guid userId,
        SetUserGradeRequest request,
        [FromServices] SetUserGradeCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new SetUserGradeCommand(
                userId,
                request.GradeId),
            cancellationToken);

        return Ok(result);
    }


}