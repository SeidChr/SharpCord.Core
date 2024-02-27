namespace SharpCord.Core.Internal;

using Microsoft.Extensions.Logging;

internal static class LogExtensions
{
    public static void Log(this ILogger logger, Action<ILogger, Exception?> action)
        => action(logger, null);

    public static void Log(this ILogger logger, Action<ILogger, Exception?> action, Exception exception)
        => action(logger, exception);

    public static void Log<T0>(this ILogger logger, Action<ILogger, T0, Exception?> action, T0 argument)
        => action(logger, argument, null);

    public static void Log<T0>(this ILogger logger, Action<ILogger, T0, Exception?> action, T0 argument, Exception exception)
        => action(logger, argument, exception);
}
