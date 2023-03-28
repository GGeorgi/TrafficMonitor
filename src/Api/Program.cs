using Application.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var config = configurationBuilder.Build();

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.ConfigureUseCases(config);
    })
    .Build();

await host.RunAsync();