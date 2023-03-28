using System.Text.RegularExpressions;
using Application.Config;
using Application.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Application.HostedServices;

public class StatsHosterService : BackgroundService
{
    private readonly LoggerConfig _config;

    public StatsHosterService(IOptions<LoggerConfig> config)
    {
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

                var hits = new Dictionary<string, int>();

                while (!reader.EndOfStream)
                {
                    var content = await reader.ReadLineAsync();
                    CalculateHits(content, hits);
                }

                offset = reader.BaseStream.Position;

                PrintHits(hits);
            }

            await Task.Delay(_config.StatsInterval, stoppingToken);
        }
    }

    private static void PrintHits(Dictionary<string, int> hits)
    {
        if (!hits.Any()) return;

        var sortedHits = from entry in hits orderby entry.Value ascending select entry;
        foreach (var sortedHit in sortedHits)
        {
            Console.WriteLine($"uri: {sortedHit.Key}, hits:{sortedHit.Value}");
        }
    }

    private static void CalculateHits(string? content, Dictionary<string, int> hits)
    {
        if (string.IsNullOrWhiteSpace(content)) return;
        var groups = Regex.Match(content, @"""(\S*)\s(\S*)\s([^""]*)""");
        var uri = groups.Groups[2].Value;

        var secondSlash = uri.IndexOf("/", 1, StringComparison.Ordinal);
        if (secondSlash == -1) return;

        var section = uri[..secondSlash];
        if (hits.ContainsKey(section)) hits[section]++;
        else hits.Add(section, 1);
    }
}