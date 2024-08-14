using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGameGame.Constrollers
{
    public class ScoreManager
    {
        private static ScoreManager instance = null;  // Statische instantie van de klasse
        private int score;

        // Privé constructor om instantiatie van buitenaf te voorkomen
        private ScoreManager()
        {
            score = 0;
        }

        // Publieke statische methode om de enige instantie te verkrijgen
        public static ScoreManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScoreManager();  // Instantie wordt voor de eerste keer aangemaakt
                }
                return instance;
            }
        }

        // Methodes om de score te beheren
        public void AddPoints(int points)
        {
            score += points;
        }

        public int GetScore()
        {
            return score;
        }

        public void Reset()
        {
            score = 0;
        }
    }
}

