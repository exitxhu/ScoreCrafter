namespace ScoreCrafter.Application.Commands.User;

using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Application.Dtos;
using ScoreCrafter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

public sealed record SetUserGradeCommand(
    Guid UserId,
    int GradeId);
public sealed class SetUserGradeCommandHandler
{
    private readonly IScoreCrafterDbContext _context;

    public SetUserGradeCommandHandler(
        IScoreCrafterDbContext context)
    {
        _context = context;
    }

    public async Task<UserGradeDto> Handle(
        SetUserGradeCommand command,
        CancellationToken cancellationToken = default)
    {
        var userExists = await _context.Users
            .AnyAsync(
                x => x.Id == command.UserId,
                cancellationToken);

        if (!userExists)
        {
            throw new InvalidOperationException(
                $"User '{command.UserId}' does not exist.");
        }

        var grade = await _context.Grades
            .SingleOrDefaultAsync(
                x => x.Id == command.GradeId,
                cancellationToken);

        if (grade is null)
        {
            throw new InvalidOperationException(
                $"Grade '{command.GradeId}' does not exist.");
        }

        var currentGrade = await _context.UserGrades
            .Include(x => x.Grade)
            .SingleOrDefaultAsync(
                x =>
                    x.UserId == command.UserId &&
                    x.IsCurrent,
                cancellationToken);

        if (currentGrade is not null &&
            currentGrade.GradeId == command.GradeId)
        {
            return UserGradeDto.From(currentGrade);
        }

        if (currentGrade is not null)
        {
            currentGrade.IsCurrent = false;
        }

        var userGrade = await _context.UserGrades
            .Include(x => x.Grade)
            .SingleOrDefaultAsync(
                x =>
                    x.UserId == command.UserId &&
                    x.GradeId == command.GradeId,
                cancellationToken);

        if (userGrade is null)
        {
            userGrade = new UserGrade
            {
                Id = Guid.NewGuid(),
                UserId = command.UserId,
                GradeId = command.GradeId,
                IsCurrent = true
            };

            await _context.UserGrades.AddAsync(
                userGrade,
                cancellationToken);
        }
        else
        {
            userGrade.IsCurrent = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        userGrade.Grade = grade;

        return UserGradeDto.From(userGrade);
    }
}
