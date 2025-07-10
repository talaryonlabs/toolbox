namespace Talaryon.Toolbox;

public static class TalaryonLogger
{
    public static void Debug<T>(string message) where T : class
    {
        Console.WriteLine($"[{typeof(T).Name}][DEBUG]: {message}");
    }
    
    public static void Error<T>(string message) where T : class
    {
        Console.WriteLine($"[{typeof(T).Name}][ERROR]: {message}");
    }
}