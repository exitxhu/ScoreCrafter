using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

using Microsoft.EntityFrameworkCore;

using ScoreCrafter.Domain.Entities;

namespace ScoreCrafter.Application.Abstraction.Data;

public interface IScoreCrafterDbContext 
{
    DbSet<User> Users { get; }

    DbSet<Purchase> Purchases { get; }

    DbSet<UserGrade> UserGrades { get; }

    DbSet<UserScore> UserScores { get; }

    DbSet<Grade> Grades { get; }

    DbSet<Formula> Formulas { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}