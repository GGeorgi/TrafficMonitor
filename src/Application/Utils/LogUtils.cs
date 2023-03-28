namespace Application.Utils;

public class LogUtils
{
    public static async Task<long> GetEndOfFile(string path)
    {
        var cmdLogReader = new StreamReader(path);
        await cmdLogReader.ReadToEndAsync();
        var offset = cmdLogReader.BaseStream.Position;
        cmdLogReader.Dispose();
        return offset;
    }
}