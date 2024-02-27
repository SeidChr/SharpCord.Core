namespace SharpCord.Core;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using SharpCord.Core.Internal;

public sealed class BotRuntime<TBot> : IHostedService
    where TBot : IBot
{
    private readonly TBot bot;

    private readonly ILogger<BotRuntime<TBot>> runtimeLogger;

    public BotRuntime(
        TBot bot,
        ILogger<BotRuntime<TBot>> runtimeLogger,
        DiscordSocketClient client,
        IOptions<BotOptions> options)
    {
        this.bot = bot;
        this.runtimeLogger = runtimeLogger;

        client.Ready += () => this.bot.OnClientConnected();

        client.Log += m =>
        {
            if (m.Exception != null)
            {
                this.runtimeLogger.Log(LogMessages.Error, m.Message, m.Exception);
            }
            else
            {
                this.runtimeLogger.Log(LogMessages.System, m.Message);
            }

            return Task.CompletedTask;
        };

        this.Client = client;
        this.Options = options;
    }

    public DiscordSocketClient Client { get; }

    public IOptions<BotOptions> Options { get; }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        this.runtimeLogger.Log(LogMessages.ClientStarting);

        var token = this.Options.Value.Token;

        this.runtimeLogger.Log(LogMessages.Connecting, token != null ? token[..Math.Min(token.Length, 5)] + "..." : "null");

        await this.Client.LoginAsync(TokenType.Bot, token);
        await this.Client.StartAsync();

        this.runtimeLogger.Log(LogMessages.ClientStatus, this.Client.Status);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        this.runtimeLogger.Log(LogMessages.ClientStopping);

        await this.Client.LogoutAsync();
        await this.Client.StopAsync();

        this.runtimeLogger.Log(LogMessages.ClientStopped);
    }
}
