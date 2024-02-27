namespace SharpCord.Core;

using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
                this.runtimeLogger.LogError(m.Exception, "ErrorMessage: {message}", m.Message);
            }
            else
            {
                this.runtimeLogger.LogInformation("SystemMessage: {message}", m.Message);
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
        this.runtimeLogger.LogInformation("Discord Client Starting");
        
        var token = this.Options.Value.Token;

        this.runtimeLogger.LogInformation(
            "Trying to connect using token '{token}'", 
            token != null
                ? token[..Math.Min(token.Length, 5)] + "..." 
                : "null"
        );

        await this.Client.LoginAsync(TokenType.Bot, token);
        await this.Client.StartAsync();

        this.runtimeLogger.LogInformation("Discord Client Status: {Status}", this.Client.Status);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        this.runtimeLogger.LogInformation("Discord Client Stopping");

        await this.Client.LogoutAsync();
        await this.Client.StopAsync();

        this.runtimeLogger.LogInformation("Discord Client Stopped");
    }
}