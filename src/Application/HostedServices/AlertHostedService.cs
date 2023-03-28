using Application.Config;
using Application.Interfaces.Services;
using Application.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Application.HostedServices;

public class AlertHostedService : BackgroundService
{
    private readonly IAlertService _alertService;
    private readonly LoggerConfig _config;

    public AlertHostedService(IOptions<LoggerConfig> config, IAlertService alertService)
    {
        _alertService = alertService;
        _config = config.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var offset = await LogUtils.GetEndOfFile(_config.AccessLog);

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var reader = new StreamReader(_config.AccessLog))
            {
                reader.BaseStream.Position = offset;

                var hits = 0;

                while (!reader.EndOfStream)
                {
                    await reader.ReadLineAsync();
                    hits++;
                }

                offset = reader.BaseStream.Position;

                _alertService.CheckHitAlert(hits);
            }

            await Task.Delay(_config.AlertInterval, stoppingToken);
        }
    }
}