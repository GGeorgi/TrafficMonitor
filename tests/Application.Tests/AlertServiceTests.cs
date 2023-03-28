using Application.Config;
using Application.Services;
using Microsoft.Extensions.Options;
using Xunit;

namespace Application.Tests;

public class AlertServiceTests
{

    [Fact]
    public void Set_Hit_Alert_Off_And_On()
    {
        var loggerConfig = Options.Create(new LoggerConfig { Threshold = 10 });
        var alertService = new AlertService(loggerConfig);
        
        var result = alertService.CheckHitAlert(15);
        Assert.True(result);
    }

    [Fact]
    public void Set_Hit_Alert_On_And_Off()
    {
        var loggerConfig = Options.Create(new LoggerConfig { Threshold = 10 });
        var alertService = new AlertService(loggerConfig);
        
        var result = alertService.CheckHitAlert(15);
        Assert.True(result);
        
        result = alertService.CheckHitAlert(10);
        Assert.False(result);
    }
}