using Application.Config;
using Application.HostedServices;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class DependencyInjections
{
    public static void ConfigureUseCases(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<LoggerConfig>(configuration.GetSection(nameof(LoggerConfig)));
        services.AddSingleton<IAlertService, AlertService>();

        // register background services
        services.AddHostedService<StatsHosterService>();
        services.AddHostedService<AlertHostedService>();
    }
}