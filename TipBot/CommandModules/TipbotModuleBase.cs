using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipBot.CommandModules
{
    public class TipbotModuleBase : ModuleBase<SocketCommandContext>
    {
        public async Task<IUserMessage> EmbedReplyAsync(string title, string message, RequestOptions options = null, bool includeAvatar = false)
        {
            var embed = new EmbedBuilder()
                .WithTitle(title)
                .AddField("\u200b", message)
                .WithColor(new Color(Convert.ToUInt32("007bff", 16)));

            if(includeAvatar)
            {
                SocketUser mentionedUser = Context.Message.MentionedUsers.FirstOrDefault();
                if(mentionedUser != null)
                {
                    embed.WithThumbnailUrl(mentionedUser.GetAvatarUrl());
                }
                else
                {
                    embed.WithThumbnailUrl(Context.Message.Author.GetAvatarUrl());
                }
            }

            return await Context.Channel.SendMessageAsync("", false, embed, options).ConfigureAwait(false);
        }
    }
}
