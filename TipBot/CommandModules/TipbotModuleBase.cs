using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TipBot.CommandModules
{
    public class TipbotModuleBase : ModuleBase<SocketCommandContext>
    {
        /// <inheritdoc cref="Settings"/>
        /// <remarks>Set by DI.</remarks>
        public Settings Settings { get; set; }

        public async Task<IUserMessage> EmbedReplyAsync(string title, string message)
        {
            var embed = new EmbedBuilder()
                .WithTitle(title)
                .AddField("\u200b", message)
                .WithColor(new Color(Convert.ToUInt32(Settings.EmbedColorHex, 16)));

            return await Context.Channel.SendMessageAsync("", false, embed, null);
        }
    }
}
