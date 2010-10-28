using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Src
{
    public static class StringUtility
    {
        public static bool IsSpelledCorrect(string word)
        {
            bool isCorrect = false;

            using (Hunspell hunspell = new Hunspell("en_US.aff", "en_US.dic"))
            {
                isCorrect = hunspell.Spell(word);
            }

            return isCorrect;
        }

        private static string GetReversedString(string word)
        {
            string reverseWord = String.Empty;

            for (int i = (word.Length - 1); i >= 0; i--)
            {
                reverseWord = reverseWord + word[i];
            }

            return reverseWord;
        }

        public static bool IsPalindrome(string word)
        {
            if ((word.Length > 1) && (word == GetReversedString(word)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int GetAlphabetPositionByLetter(char letter)
        {
            int asciiConversionParameter = 96;
            int position = 0;

            if (char.IsLetter(letter))
            {
                position = ((int)char.ToLower(letter)) - asciiConversionParameter;
            }
            else
            {
                throw new ArgumentException("Invalid alphabet character", "letter");
            }

            return position;
        }
    }
}
