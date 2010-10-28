using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Assignment4.Core
{
    public class AlphabetDetector : IDetector
    {
        public bool IsSpecial (string word)
        {
            if (word.Length > 0)
            {
                return (word == Alphabetize(word));
            }
            else
            {
                return false;
            }
        }

        private string Alphabetize(string word)
        {
            List<char> sortedWord = word.ToCharArray().ToList<char>();

            sortedWord.Sort();

            return new string(sortedWord.ToArray<char>());
        }
    }
}
