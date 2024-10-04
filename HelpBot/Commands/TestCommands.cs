using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using HelpBot.Games.CardGameSystem;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using HelpBot.Games.Jokes;
using HelpBot.Games.RockPaperScissors;
using HelpBot.Games.RandomFact;
namespace HelpBot.Commands
{
    public class TestCommands : BaseCommandModule
    {
        private static readonly Random Rand = new Random();
        private bool gameOver;

        [Command("greet")]
        [Cooldown(5, 10, CooldownBucketType.User)]
        public async Task Greet(CommandContext cmd)
        {
            string[] randGreet = new string[] { "Hello,", "Hi,", "Hey,", "Greetings," };
            int randIndex = Rand.Next(randGreet.Length);
            await cmd.Channel.SendMessageAsync($"{randGreet[randIndex]} {cmd.User.Username}!");
        }

        [Command("cards")]
        public async Task CardGame(CommandContext cmd)
        {
            var userCard = new War();
            var embeddedUser = new DiscordEmbedBuilder
            {
                Title = $"{cmd.User.Username}'s card -> {userCard.selectedCard}",
                Color = DiscordColor.Red
            };

            await cmd.Channel.SendMessageAsync(embed: embeddedUser);

            var botCard = new War();
            var embeddedBot = new DiscordEmbedBuilder
            {
                Title = $"{cmd.Client.CurrentUser.Username}'s card -> {botCard.selectedCard}",
                Color = DiscordColor.Red
            };

            await cmd.Channel.SendMessageAsync(embed: embeddedBot);

            var result = userCard.selectionNum > botCard.selectionNum
                ? $"Congratulations, {cmd.User.Username}! You've won!"
                : "You lost!";

            var victory = new DiscordEmbedBuilder
            {
                Title = result,
                Color = DiscordColor.Red
            };
            var tie = new DiscordEmbedBuilder()
            {
                Title = "It's a tie!",
                Color = DiscordColor.Red
            };

            await cmd.Channel.SendMessageAsync(embed: victory);

            if (userCard.selectionNum == botCard.selectionNum)
                await cmd.Channel.SendMessageAsync(embed: tie);
        }

        [Command("date")]
        [Cooldown(5, 10, CooldownBucketType.User)]
        public async Task Date(CommandContext cmd)
        {
            DateTime today = DateTime.Now;
            await cmd.RespondAsync($"Today is {today:yyyy-MM-dd}");
        }

        [Command("joke")]
        [Cooldown(1, 2, CooldownBucketType.User)]
        public async Task Joke(CommandContext cmd)
        {
            DadJokes jokes = new DadJokes();

            try
            {
                string jokeData = await jokes.GetJoke();
                var embedJoke = new DiscordEmbedBuilder()
                {
                    Title = jokeData,
                    Color = DiscordColor.Red
                };
                await cmd.Channel.SendMessageAsync(embed: embedJoke);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                await cmd.Channel.SendMessageAsync("ERROR FETCHING JOKE!");
            }


        } // Works flawlessly!

        [Command("rps")]
        public async Task RockPaperScissors(CommandContext cmd)
        {
            var userRPS = new RPS();
            gameOver = false;
            var embedUser = new DiscordEmbedBuilder()
            {
                Title = $"{cmd.User.Username}'s choice -> {userRPS.shoot}",
                Color = DiscordColor.Orange
            };

            await cmd.RespondAsync(embed: embedUser);

            var botRPS = new RPS();
            var embedBot = new DiscordEmbedBuilder()
            {
                Title = $"{cmd.Client.CurrentUser.Username}'s choice -> {botRPS.shoot}",
                Color = DiscordColor.Orange
            };

            await cmd.RespondAsync(embed: embedBot);

            if (gameOver == false)
            {
                // Tie
                if (userRPS.shoot.Contains(botRPS.shoot))
                {
                    var tie = new DiscordEmbedBuilder()
                    {
                        Title = "It's a tie!",
                        Color = DiscordColor.Orange
                    };

                    await cmd.RespondAsync(embed: tie);
                    gameOver = true;
                }

                // User win
                if (userRPS.shoot.Contains("rock") && botRPS.shoot.Contains("scissors"))
                {
                    var userWin = new DiscordEmbedBuilder()
                    {
                        Title = $"Congratulations {cmd.User.Username}! {userRPS.shoot} trumps {botRPS.shoot}. You win!",
                        Color = DiscordColor.Orange
                    };

                    await cmd.RespondAsync(embed: userWin);
                    gameOver = true;
                }
                else if (userRPS.shoot.Contains("paper") && botRPS.shoot.Contains("rock"))
                {
                    var userWin = new DiscordEmbedBuilder()
                    {
                        Title = $"Congratulations {cmd.User.Username}! {userRPS.shoot} trumps {botRPS.shoot}. You win!",
                        Color = DiscordColor.Orange
                    };

                    await cmd.RespondAsync(embed: userWin);
                    gameOver = true;
                }
                else if (userRPS.shoot.Contains("scissors") && botRPS.shoot.Contains("paper"))
                {
                    var userWin = new DiscordEmbedBuilder()
                    {
                        Title = $"Congratulations {cmd.User.Username}! {userRPS.shoot} trumps {botRPS.shoot}. You win!",
                        Color = DiscordColor.Orange
                    };

                    await cmd.RespondAsync(embed: userWin);
                    gameOver = true;
                }

                // User loss
                else if (!userRPS.shoot.Contains(botRPS.shoot))
                {
                    var userLoss = new DiscordEmbedBuilder()
                    {
                        Title = $"You lost {cmd.User.Username}! {botRPS.shoot} trumps {userRPS.shoot}.",
                        Color = DiscordColor.Orange
                    };

                    await cmd.RespondAsync(embed: userLoss);
                    gameOver = true;
                }
            }
        }

        [Command("fact")]
        [Cooldown(3, 6, CooldownBucketType.User)]
        public async Task Facts(CommandContext cmd)
        {
            Fact fact = new Fact();

            try
            {
                string factdata = await fact.GetFact();
                Console.WriteLine($"FACT GENERATED: {factdata}");

                var embed = new DiscordEmbedBuilder()
                {
                    Title = factdata,
                    Color = DiscordColor.Orange
                };

                await cmd.RespondAsync(embed: embed);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}

