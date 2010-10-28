using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHunspell;
using Utilities;

namespace Src
{
    public class Cart
    {
        private char[] slot;
        
        public Cart()
        {
            slot = new char[7];
            for (int i = 0; i < 7; i++)
            {
                slot[i] = '-';
            }
        }

        public string getString()
        {
            string str = new string(slot);
            return str;
        }
        public char getChar(int place)
        {
            if (place > 7 || place < 1)
                throw new LetterCartException("getting letter out of range");
            else
                return slot[place-1];
        }
        public void addCharacter(char A, int place)
        {
            if (slot[place - 1] != '-')
                throw new LetterCartException("overwriting old letter");
            else
                slot[place-1] = A;
        }
        public int collect()
        {
            string word = new string(slot);
            word.Replace('-', ' ');
            int value = 0;
            if (StringUtility.IsSpelledCorrect(word))
            {
                foreach (char letter in slot)
                    value += StringUtility.GetAlphabetPositionByLetter(letter);
            }
            if (StringUtility.IsPalindrome(word))

            return 0;
        }
        public Boolean discard()
        {
            for (int i = 0; i < 7; i++)
            {
                slot[i] = '-';
            }
            return true;
        }
    }
}
