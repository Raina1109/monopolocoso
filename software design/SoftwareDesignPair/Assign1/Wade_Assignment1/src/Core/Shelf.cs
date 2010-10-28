using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wade_Assignment1.Core
{
    public class Shelf
    {
        char[] letterList = new char[6];

        RandomLetterGenerator randomLetterGenerator = new RandomLetterGenerator();

        public Shelf()
        {
            for (int i = 0; i <= 5; i++)
            {
                letterList[i] = randomLetterGenerator.GetNextLetter();
            }
        }

        public char[] GetLetters()
        {
            return letterList;
        }

        public bool IsEmpty()
        {
            return (letterList.Length == 0 ? true: false);
        }

        public char SelectLetterByPosition(int position)
        {
            char selectedLetter = letterList[position - 1];

            return selectedLetter;
        }

        public void DiscardLetterByPosition(int index)
        {
            letterList[index] = randomLetterGenerator.GetNextLetter();
        }
    }
}
