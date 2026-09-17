using Microsoft.AspNetCore.Mvc;

using ScoreCrafter.Application.Queries.User;
using ScoreCrafter.SDK.Model.Rest;

namespace ScoreCrafter.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly GetUserSummaryQueryHandler _handler;

    public UsersController(
        GetUserSummaryQueryHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserSummaryResponse>> Get(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var result = await _handler.Handle(
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
}