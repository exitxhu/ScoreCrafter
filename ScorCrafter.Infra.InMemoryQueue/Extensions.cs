using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using ScorCrafter.Infra.InMemoryQueue;

using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Application.Abstraction.Queue;
using ScoreCrafter.Application.Facilatores;

using System;
using System.Collections.Generic;
using System.Text;

namespace ScoreCrafter.Infra.InMemoryQueue;

public static class Extensions
{
    public static IServiceCollection AddInmemoryQueueInfrastructure(
        this IServiceCollection services)
    {
        services.AddSingleton<IScoreCalculationQueue, ScoreCalculationQueue>();
        services.AddHostedService<ScoreCalculationWorker>();

        return services;
    }
}
