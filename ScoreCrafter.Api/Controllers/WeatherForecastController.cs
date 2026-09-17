using Microsoft.AspNetCore.Mvc;

using ScoreCrafter.Application.Commands.Purchase;

namespace ScoreCrafter.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PurchaseController : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> Get(RegisterPurchaseRequest request,
                RegisterPurchaseCommandHandler handler,
                CancellationToken cancellationToken)
    {
        var command = new RegisterPurchaseCommand(
            request.PurchaseId,
            request.UserId,
            request.Amount,
            request.PurchaseDate,
            request.Metadata);

        await handler.Handle(command, cancellationToken);

        return Ok();
    }
}
public sealed record RegisterPurchaseRequest(
    Guid PurchaseId,
    Guid UserId,
    decimal Amount,
    DateTime PurchaseDate,
    Dictionary<string, string>? Metadata);