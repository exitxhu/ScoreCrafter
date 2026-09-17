using Microsoft.Extensions.DependencyInjection;

using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Application.Commands.Formula;
using ScoreCrafter.Application.Commands.Grade;
using ScoreCrafter.Application.Commands.Purchase;
using ScoreCrafter.Application.Commands.User;
using ScoreCrafter.Application.Facilatores;
using ScoreCrafter.Application.Queries.User;

using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        services.AddScoped<RegisterPurchaseCommandHandler>();

        services.AddScoped<GetUserSummaryQueryHandler>();
        services.AddScoped<CreateUserCommandHandler>();
        services.AddScoped<SetUserGradeCommandHandler>();
        services.AddScoped<CalculateUserScoreCommandHandler>();

        services.AddScoped<CreateFormulaCommandHandler>();
        services.AddScoped<UpdateFormulaCommandHandler>();
        services.AddScoped<DeleteFormulaCommandHandler>();
        services.AddScoped<TestFormulaCommandHandler>();

        services.AddScoped<CreateGradeCommandHandler>();
        services.AddScoped<UpdateGradeCommandHandler>();
        services.AddScoped<DeleteGradeCommandHandler>();


        return services;
    }
}
