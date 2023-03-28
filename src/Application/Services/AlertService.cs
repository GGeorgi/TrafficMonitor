using Application.Config;
using Application.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class AlertService : IAlertService
{
    private static bool _hitAlertIsOn;
    private readonly LoggerConfig _config;

    public AlertService(IOptions<LoggerConfig> config)
    {
        _config = config.Value;
    }

    public bool CheckHitAlert(int hits)
    {
        if (hits > _config.Threshold && !_hitAlertIsOn)
        {
            Console.WriteLine($"High traffic generated an alert - hits = {hits}, triggered at {DateTime.UtcNow:O}");
            _hitAlertIsOn = true;
        }
        else if (hits <= _config.Threshold && _hitAlertIsOn)
        {
            Console.WriteLine($"Traffic amount is recovered - hits = {hits}, triggered at {DateTime.UtcNow:O}");
            _hitAlertIsOn = false;
        }
        return _hitAlertIsOn;
    }
}