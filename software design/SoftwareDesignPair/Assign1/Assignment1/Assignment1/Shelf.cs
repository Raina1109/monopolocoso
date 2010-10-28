using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Src
{
    public class Shelf
    {
        char [] letters;
        static Random rand = new Random();

        public Shelf()
        {
            letters = new char[6];
            for (int i = 0; i <= 5; i++)
            {
                letters[i] = randomLetter();
            }
        }
        public char getLetter(int p)
        {
            return letters[p - 1];
        }
        public bool IsEmpty()
        {
            return (letters.Length == 0 ? true : false);
        }
        public char randomLetter()
        {
            int num = rand.Next(0, 26);
            return (char)('a'+num);
        }
        public void random(int p)
        {
            letters[p - 1] = randomLetter();
        }
    }
}
