namespace Talaryon.StackManager;

public static class LogMessage
{
    // Basic color methods (now using LogBuilder internally)
    public static void AsError(string message)
    {
        LogBuilder.Message(message).AsError().Run();
    }
    
    public static void AsSuccess(string message)
    {
        LogBuilder.Message(message).AsSuccess().Run();
    }
    
    public static void AsWarning(string message)
    {
        LogBuilder.Message(message).AsWarning().Run();
    }
    
    public static void AsInfo(string message)
    {
        LogBuilder.Message(message).Run();
    }

    // Enhanced confirmation methods
    public static bool AsConfirmInfo(string message)
    {
        return LogBuilder.Question(message)
            .AsYesNo()
            .Run();
    }
    
    public static bool AsConfirmWarning(string message)
    {
        return LogBuilder.Question(message)
            .AsYesNo()
            .AsWarning()
            .Run();
    }
    
    public static bool AsConfirmError(string message)
    {
        return LogBuilder.Question(message)
            .AsYesNo()
            .AsError()
            .Run();
    }

    // New meaningful methods
    public static void AsCustom(string message, ConsoleColor color)
    {
        LogBuilder.Message(message).AsColored(color).Run();
    }
    
    public static void AsBoxed(string message)
    {
        LogBuilder.Message(message).InBox().Run();
    }
    
    public static void AsBoxed(string message, int width)
    {
        LogBuilder.Message(message).InBox(width).Run();
    }
    
    public static void AsBoxed(string message, ConsoleColor color)
    {
        LogBuilder.Message(message).InBox().AsColored(color).Run();
    }
    
    public static void AsTimestamped(string message)
    {
        LogBuilder.Message(message).WithTimestamp().Run();
    }
    
    public static void AsIndented(string message, int level)
    {
        LogBuilder.Message(message).Indented(level).Run();
    }

    // Separator methods (now using LogBuilder internally)
    public static void Separator(char c = '-', int length = 40)
    {
        LogBuilder.Message(new string(c, length)).Run();
    }
    
    public static void Separator(string title, char c = '=', int length = 40)
    {
        int padding = (length - title.Length - 2) / 2;
        string left = new string(c, padding);
        string right = new string(c, length - title.Length - 2 - padding);
        LogBuilder.Message($"{left} {title} {right}").Run();
    }

    // Table methods
    public static ILogBuilderTable Table(params string[] headers)
    {
        return LogBuilder.Table().WithHeaders(headers);
    }

    // Progress spinner methods
    public static ILogBuilderProgress Progress(string message = "")
    {
        return LogBuilder.Progress(message);
    }

    // Colored separator
    public static void Separator(char c, int length, ConsoleColor color)
    {
        LogBuilder.Message(new string(c, length)).AsColored(color).Run();
    }
    
    public static void Separator(string title, char c, int length, ConsoleColor color)
    {
        int padding = (length - title.Length - 2) / 2;
        string left = new string(c, padding);
        string right = new string(c, length - title.Length - 2 - padding);
        LogBuilder.Message($"{left} {title} {right}").AsColored(color).Run();
    }
}
