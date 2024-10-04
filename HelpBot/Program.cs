using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using HelpBot.Config;
using HelpBot.Commands;
using DSharpPlus.EventArgs;
using System.Collections.Generic;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;

namespace HelpBot
{
    public class Program
    {
        private static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }
        public static DateTime botUpTime = DateTime.UtcNow;
        static async Task Main(string[] args)
        {
            var jsonreader = new jsonreader();
            await jsonreader.readjson();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jsonreader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            Client = new DiscordClient(discordConfig);

            // Event handlers
            Client.Ready += Client_Ready;
            Client.VoiceStateUpdated += VoiceChannelHandler;

            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonreader.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            var slashCommands = Client.UseSlashCommands();
            Commands.CommandErrored += CommandHandler;

            Commands.RegisterCommands<TestCommands>();
            slashCommands.RegisterCommands<SlashCommands>();

            await Client.ConnectAsync();
            await Task.Delay(-1); // Keeps bot running indefinitely 
        }

        private static async Task CommandHandler(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            if(e.Exception is ChecksFailedException exception)
            {
                string time = string.Empty;

                foreach(var check in exception.FailedChecks)
                {
                    var coolDown = (CooldownAttribute)check;
                    time = coolDown.GetRemainingCooldown(e.Context).ToString(@"hh\:mm\:ss");
                }

                var coolDownMessage = new DiscordEmbedBuilder
                {
                    Color = DiscordColor.Red,
                    Title = "You are on a cooldown. Please wait for it to end.",
                    Description = $"Time -> {time}"
                };
                await e.Context.Channel.SendMessageAsync(embed: coolDownMessage);
            }
        }

        // Event handler methods

        private static async Task VoiceChannelHandler(DiscordClient sender, VoiceStateUpdateEventArgs e)
        {
            HashSet<string> voiceChannels = new HashSet<string> { "General", "Call-of-Duty" }; // if you want to add channels, add the names into this HashSet
            if (e.Before == null && voiceChannels.Contains(e.Channel.Name))
            {
                await e.Channel.SendMessageAsync($"{e.User.Username} has joined the voice channel! There are currently {e.Guild.MemberCount} members in this VC!");
            }
        }

        private static Task Client_Ready(DiscordClient sender, DSharpPlus.EventArgs.ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
        
    }
}
