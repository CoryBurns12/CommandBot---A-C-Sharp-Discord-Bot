using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using System;
using HelpBot.Config;
using HelpBot.Games.RandomFact;
using System.Security.Cryptography;

namespace HelpBot.Commands
{
    public class SlashCommands : ApplicationCommandModule
    {
        [SlashCommand("uptime", "gets uptime of bot")]
        public async Task Uptime(InteractionContext cmd)
        {
            TimeSpan time = DateTime.UtcNow - Program.botUpTime;
            string upTime = $"{time.Days}d {time.Hours}h {time.Minutes}m {time.Seconds}s";

            await cmd.DeferAsync();
            await cmd.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"{cmd.Client.CurrentUser.Username}'s Uptime -> {upTime}"));
        }

        [SlashCommand("translate", "translates a word to another language")]
        public async Task Translate(InteractionContext cmd)
        {
            await cmd.DeferAsync();
            await cmd.EditResponseAsync(new DiscordWebhookBuilder().WithContent("WIP"));
        } // WIP Method (Waiting for API key)
    }
}
