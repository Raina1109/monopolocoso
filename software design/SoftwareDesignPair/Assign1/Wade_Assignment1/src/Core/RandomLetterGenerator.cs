using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wade_Assignment1.Core
{
    public class RandomLetterGenerator
    {
        const int asciiParameter = 96;

        Random randomNumberGenerator = new Random();

        public RandomLetterGenerator()
        { 
        }

        public char GetNextLetter()
        {
            return (char)(randomNumberGenerator.Next(1, 26) + asciiParameter);
        }
    }
}
