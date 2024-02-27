namespace SharpCord.Core.Extensions;

using Discord.WebSocket;

internal static class DiscordExtensions
{
    public static SocketGuild? GetGuild(this ISocketMessageChannel channel)
        => channel is SocketGuildChannel guildChannel
            ? guildChannel.Guild
            : null;

    public static SocketGuild? GetGuild(this SocketMessage message)
        => message.Channel.GetGuild();
}
