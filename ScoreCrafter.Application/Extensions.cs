using Microsoft.Extensions.DependencyInjection;

using ScoreCrafter.Application.Abstraction.Data;
using ScoreCrafter.Application.Facilatores;

using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
  

        return services;
    }
}
