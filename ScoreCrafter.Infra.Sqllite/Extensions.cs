using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Application.Facilatores;
using ScoreCrafter.Infra.Sqllite.Implementations;
using ScoreCrafter.Infra.Sqllite.Persistenc;

using System;
using System.Collections.Generic;
using System.Text;

namespace ScoreCrafter.Infra.Sqllite;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<IScoreCrafterDbContext, ScoreCrafterDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });
        services.AddScoped<ITransactionManager, TransactionManager>();

        return services;
    }
}
