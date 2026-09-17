using Microsoft.AspNetCore.Mvc;

using ScoreCrafter.Application.Commands.Purchase;
using ScoreCrafter.SDK.Model.Rest;

namespace ScoreCrafter.Api.Controllers;


[ApiController]
[Route("api/purchases")]
public sealed class PurchasesController : ControllerBase
{
    private readonly RegisterPurchaseCommandHandler _handler;

    public PurchasesController(
        RegisterPurchaseCommandHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> Register(
        RegisterPurchaseRequest request,
        CancellationToken cancellationToken)
    {
        await _handler.Handle(
            new RegisterPurchaseCommand(
                request.PurchaseId,
                request.UserId,
                request.Amount,
                request.PurchaseDate,
                request.Metadata),
            cancellationToken);

        return Ok();
    }
}
