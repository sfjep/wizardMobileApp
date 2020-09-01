using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WizardScoreboard
{
    public class Player
    {
        public string name;
        public int score;
        public List<int> trickGuess = new List<int>();
        public List<int> trickResult = new List<int>();

        public Player(string name)
        {
            this.name = name;
            score = 0;
            trickGuess = new List<int>();
            trickResult = new List<int>();
        }
    }
}
