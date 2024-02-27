namespace SharpCord.Core;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class BotHost
{
    public static IHostBuilder CreateDefaultBuilder<TBot>(
        string[]? args)
        where TBot : class, IBot
        => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(x => x.AddUserSecrets<TBot>())
            .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Debug))
            .ConfigureServices((builder, services) =>
            {
                services.AddSingleton(provider => new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Debug,
                    AlwaysDownloadUsers = true,
                    GatewayIntents
                        = (GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers)
                        & ~(GatewayIntents.GuildScheduledEvents | GatewayIntents.GuildInvites),
                });

                services.AddSingleton(provider => new DiscordSocketClient(provider.GetRequiredService<DiscordSocketConfig>()));
                services.AddSingleton<TBot>();
                services.AddOptions<BotOptions>()
                    .Bind(builder.Configuration.GetSection(nameof(BotOptions)));
                services.AddHostedService<BotRuntime<TBot>>();
            });
}
