using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using System;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections.Concurrent;
using System.Linq;
using System.Collections.Generic;
using DSharpPlus;

namespace HelpBot.Commands
{
    public class SlashCommands : ApplicationCommandModule
    {

        private static readonly ConcurrentDictionary<ulong, int> UserNumberToGuess = new ConcurrentDictionary<ulong, int>(); // holds the number that the user is trying to guess
        private static readonly ConcurrentDictionary<ulong, int> UserGuesses = new ConcurrentDictionary<ulong, int>(); // Holds an integer value of the total guesses by the user
        private static readonly Random Rand = new Random();

        [SlashCommand("uptime", "gets uptime of bot")]
        [Cooldown(4, 6, CooldownBucketType.User)]
        public async Task Uptime(InteractionContext cmd)
        {
            TimeSpan time = DateTime.UtcNow - Program.botUpTime;
            string upTime = $"{time.Days}d {time.Hours}h {time.Minutes}m {time.Seconds}s";

            await cmd.DeferAsync();
            await cmd.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"{cmd.Client.CurrentUser.Username}'s Uptime -> {upTime}"));
        }

        [SlashCommand("binary", "gets binary of a token that is entered (whole number)")]
        [Cooldown(2, 4, CooldownBucketType.User)]
        public async Task Binary(InteractionContext cmd, [Option("token", "whole number for binary conversion")] long token)
        {
            int remainder;
            string conversion = "";

            try
            {
                while (token > 0)
                {
                    remainder = (int)(token % 2);
                    token /= 2;
                    conversion = remainder.ToString() + conversion;
                }

                await cmd.Channel.SendMessageAsync($"Decimal Conversion: {conversion}");
                Console.WriteLine("SUCCESSFUL RUN!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [SlashCommand("Decimal", "gets number from binary user entry")]
        [Cooldown(2, 4, CooldownBucketType.User)]
        public async Task Decimal(InteractionContext cmd, [Option("binary", "binary entry to convert to a whole number")] string token)
        {
            if (!token.All(v => v == '0' || v == '1')) // If ANY of the values aren't 0s and 1s, then it isn't valid for conversion
            {
                await cmd.Channel.SendMessageAsync("Binary number not valid. Please enter a valid number");
                return;
            }

            try
            {
                long dec = 0;

                for (int i = 0; i < token.Length; i++)
                {
                    if (token[token.Length - 1 - i] == '1')
                    {
                        dec += (1L << i); // 2^i (How binary calculates whole numbers)
                    }
                }
                var embed = new DiscordEmbedBuilder()
                {
                    Title = $"Decimal Conversion {dec}",
                    Color = DiscordColor.Red
                };

                await cmd.Channel.SendMessageAsync(embed: embed);
                Console.WriteLine("DECIMAL: SUCCESSFUL RUN!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [SlashCommand("rng", "random number generator")]
        [Cooldown(2, 4, CooldownBucketType.User)]
        public async Task StartGame(InteractionContext cmd)
        {
            int number = Rand.Next(1, 101); // Number between 1 and 100
            UserNumberToGuess[cmd.User.Id] = number; // Stores number in the ConcurrentDictionary UserNumberToGuess
            UserGuesses[cmd.User.Id] = 0; // Reset guess count
            await cmd.Channel.SendMessageAsync("I have selected a number between 1 and 100.");
        }

        [SlashCommand("guess", "allows the user to guess a number")]
        [Cooldown(3, 6, CooldownBucketType.User)]
        public static async Task GuessNumber(InteractionContext cmd, [Option("number", "number to guess")] long guess)
        {
            if (!UserNumberToGuess.TryGetValue(cmd.User.Id, out int number)) // if user has not entered '!rng' then there is no random number that is generated. This statement means that the game has not been init'd
            {
                await cmd.Channel.SendMessageAsync("You haven't started a game yet. Use `!rng` to start.");
                return;
            }

            if (guess < 1 || guess > 100)
            {
                await cmd.Channel.SendMessageAsync("Please guess a number between 1 and 100.");
                return;
            }

            UserGuesses[cmd.User.Id]++; // guesses of the user

            if (guess == number)
            {
                await cmd.Channel.SendMessageAsync($"Congratulations! You guessed the number in {UserGuesses[cmd.User.Id]} tries.");
                UserNumberToGuess.TryRemove(cmd.User.Id, out _); // End the game
                UserGuesses.TryRemove(cmd.User.Id, out _); // Ends guessing
            }
            else if (guess < number)
            {
                await cmd.Channel.SendMessageAsync("Too low! Try again.");
            }
            else
            {
                await cmd.Channel.SendMessageAsync("Too high! Try again.");
            }
        }

        [SlashCommand("binList", "list of all binary numbers to use for assistance")]
        [Cooldown(10, 20, CooldownBucketType.User)]
        public async Task BinList(InteractionContext cmd) // Prints values but there is a rate limit. Print is delayed by a couple seconds every 5 values
        {
            Dictionary<int, string> dict = new Dictionary<int, string>();

            for (int i = 0; i < 256; i++)
            {
                dict.Add(i, Convert.ToString(i, 2).PadLeft(8, '0')); // Takes i and converts it to a string (Convert.ToString(i,2) means it converts i to base 2 which is binary) Adding padding pads it with 0s up to 8 numbers if not a 1
            }

            foreach (var key in dict)
            {
                await cmd.Channel.SendMessageAsync($"{key.Key} : {key.Value}"); // Prints the values of the binary numbers so users can find them easily
            }
        }

        [SlashCommand("role", "role creation (if admin)")]
        [RequirePermissions(Permissions.Administrator)]
        public async Task Role(InteractionContext cmd, [Option("role", "role to create")] string role)
        {
            var roleName = await cmd.Guild.CreateRoleAsync(role);

            if (role == "admin" || role == "bot")
            {
                await roleName.ModifyAsync(Customize =>
                {
                    Customize.Color = DiscordColor.Red;
                    Customize.Permissions = Permissions.Administrator;
                });
            }

            await cmd.Channel.SendMessageAsync($"Role: '{role}' created!");
        }
    }
}
