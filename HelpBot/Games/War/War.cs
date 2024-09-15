using System;

namespace HelpBot.Games.CardGameSystem
{
    public class War
    {
        private int[] cardNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        private string[] cardSuits = { "Clubs", "Spades", "Diamonds", "Hearts" };

        public int selectionNum { get; set; }
        public string selectedCard { get; set; }

        public War()
        {
            var random = new Random();

            int numberIndex = random.Next(0, cardNumbers.Length - 1);
            int suitIndex = random.Next(0, cardSuits.Length - 1);

            this.selectionNum = cardNumbers[numberIndex];
            this.selectedCard = $"{selectionNum} of {cardSuits[suitIndex]}";
        }
    }
}
