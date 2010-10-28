using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wade_Assignment1.Core
{
    public class Cart
    {
        char[] letters = new char[7]; 

        public Cart()
        {
            InitializeCart();
        }

        private void InitializeCart()
        {
            for (int i = 0; i <= 6; i++)
            {
                letters[i] = '-';
            }
        }

        public bool IsEmpty()
        {
            return (GetLetterCount() == 0 ? true : false);
        }

        public int GetLetterCount()
        {
            int count = 0;

            for (int i = 0; i <= 6; i++)
            {
                if (letters[i] != '-')
                { 
                    count++;
                }
            }

            return count;
        }

        public void AddLetter(char letterToBeAdded)
        {
            int nextOpenIndexSlot = FindNextOpenSlot();

            if (nextOpenIndexSlot != -1)
            {
                letters[nextOpenIndexSlot] = letterToBeAdded;
            }
            else
            {
                throw new CartFullException();
            }
        }

        private int FindNextOpenSlot()
        {
            for (int i = 0; i <= 6; i++)
            {
                if (letters[i] == '-')
                {
                    return i;
                }
            }

            return -1;
        }

        public void DiscardCart()
        {
            InitializeCart();
        }

        public string GetWord()
        {
            string word = String.Empty;

            foreach (char letter in letters)
            {
                if (letter != '-')
                {
                    word = word + letter;
                }
            }

            return word;
        }
    }
}
