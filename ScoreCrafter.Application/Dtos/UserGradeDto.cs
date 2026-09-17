using ScoreCrafter.Domain.Entities;

namespace ScoreCrafter.Application.Dtos;

public sealed class UserGradeDto
{
    public Guid UserId { get; init; }
    public int GradeId { get; init; }
    public string GradeName { get; init; } = string.Empty;
    public bool IsCurrent { get; init; }

    public static UserGradeDto From(
        UserGrade userGrade)
    {
        return new UserGradeDto
        {
            UserId = userGrade.UserId,
            GradeId = userGrade.GradeId,

            GradeName = userGrade.Grade.Name,
            IsCurrent = userGrade.IsCurrent
        };
    }
}

