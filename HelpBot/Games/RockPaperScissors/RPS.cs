using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpBot.Games.RockPaperScissors
{
    public class RPS
    {
        private string[] rps = { "rock", "paper", "scissors" };
        private Random rand = new Random();
        public string shoot { get; set; }

        public RPS()
        {
            int randIndex = rand.Next(rps.Length);

            this.shoot = rps[randIndex];
        }
    }
}
