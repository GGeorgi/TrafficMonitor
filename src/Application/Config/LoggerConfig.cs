namespace Application.Config;

public class LoggerConfig
{
    public string AccessLog { get; set; } = null!;
    public int StatsInterval { get; set; }
    public int AlertInterval { get; set; }
    public int Threshold { get; set; }
}