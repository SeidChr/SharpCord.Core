namespace SharpCord.Core;

using System;

using Discord;

using Microsoft.Extensions.Logging;

internal sealed class LogMessages
{
    public static readonly Action<ILogger, string, Exception?> Connecting
        = LoggerMessage.Define<string>(LogLevel.Information, 16001, "Trying to connect using token '{Token}'");

    public static readonly Action<ILogger, string, Exception?> Error
        = LoggerMessage.Define<string>(LogLevel.Error, 17001, "ErrorMessage: {Message}");

    public static readonly Action<ILogger, string, Exception?> System
        = LoggerMessage.Define<string>(LogLevel.Information, 17002, "SystemMessage: {Message}");

    public static readonly Action<ILogger, Exception?> ClientStarting
        = LoggerMessage.Define(LogLevel.Information, 18001, "Discord Client Starting");

    public static readonly Action<ILogger, UserStatus, Exception?> ClientStatus
        = LoggerMessage.Define<UserStatus>(LogLevel.Information, 18002, "Discord Client Status: {Status}");

    public static readonly Action<ILogger, Exception?> ClientStopping
        = LoggerMessage.Define(LogLevel.Information, 18003, "Discord Client Stopping");

    public static readonly Action<ILogger, Exception?> ClientStopped
        = LoggerMessage.Define(LogLevel.Information, 18004, "Discord Client Stopped");

    public static readonly Action<ILogger, Exception?> UnawaitedTaskError
        = LoggerMessage.Define(LogLevel.Error, 19001, "Error in non-awaited Task");
}
