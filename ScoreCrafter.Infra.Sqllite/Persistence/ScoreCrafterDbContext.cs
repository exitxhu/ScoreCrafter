using Microsoft.EntityFrameworkCore;

using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace ScoreCrafter.Infra.Sqllite.Persistenc;

public sealed class ScoreCrafterDbContext : DbContext, IScoreCrafterDbContext
{
    public ScoreCrafterDbContext(DbContextOptions<ScoreCrafterDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Purchase> Purchases => Set<Purchase>();

    public DbSet<UserGrade> UserGrades => Set<UserGrade>();

    public DbSet<UserScore> UserScores => Set<UserScore>();

    public DbSet<Grade> Grades => Set<Grade>();

    public DbSet<Formula> Formulas => Set<Formula>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ScoreCrafterDbContext).Assembly);
    }
}
