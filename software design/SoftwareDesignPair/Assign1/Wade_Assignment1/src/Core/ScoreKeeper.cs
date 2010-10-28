using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wade_Assignment1.Core
{
    public class ScoreKeeper
    {
        int score = 0;

        public ScoreKeeper()
        {
        }

        public int GetScore()
        {
            return score;
        }

        public void AddPoints(int pointsToBeAdded)
        {
            score = score + pointsToBeAdded;
        }

        public void SubtractPoints(int pointsToBeSubtracted)
        {
            score = score - pointsToBeSubtracted;
        }
    }
}
