using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Src
{
    public class ScoreKeeper
    {
        private int score;
        public ScoreKeeper()
        {
            score = 0;
        }
        public int getScore()
        {
            return score;
        }
        public void addPoint(int pointDiff)
        {
            score = score + pointDiff;
            if (score < 0)
                score = 0;
        }
    }
}
