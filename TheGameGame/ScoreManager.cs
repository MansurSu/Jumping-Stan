using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameGame
{
    public class ScoreManager
    {
        public int Score { get; private set; }

        public ScoreManager()
        {
            Score = 0;
        }

        public void AddScore(int amount)
        {
            Score += amount;
        }
    }
}

