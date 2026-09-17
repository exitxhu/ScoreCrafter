using ScoreCrafter.Application.Abstraction.Data;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using ScoreCrafter.Application.Dtos;

namespace ScoreCrafter.Application.Queries.User;

public sealed record GetUserSummaryQuery(Guid UserId);

public sealed class GetUserSummaryQueryHandler
{
    private readonly IScoreCrafterDbContext _context;

    public GetUserSummaryQueryHandler(
        IScoreCrafterDbContext context)
    {
        _context = context;
    }

    public async Task<UserSummaryDto?> Handle(
        GetUserSummaryQuery query,
        CancellationToken cancellationToken)
    {
        return await _context.Users
            .Where(x => x.Id == query.UserId)
            .Select(x => new UserSummaryDto
            {
                UserId = x.Id,
                Score = _context.UserScores
                    .Where(s => s.UserId == x.Id && s.IsCurrent)
                    .Select(s => (decimal?)s.Score)
                    .FirstOrDefault() ?? 0,

                Grade = _context.UserGrades
                    .Where(g => g.UserId == x.Id && g.IsCurrent)
                    .Select(g => g.Grade.Name)
                    .FirstOrDefault(),

                PurchaseCount = _context.Purchases
                    .Count(p => p.UserId == x.Id)
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}

