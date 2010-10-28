using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHunspell;
using System.Reflection;
using System.Resources; 
using System.IO;
using Utilities;

namespace Wade_Assignment1.Core
{
    public class GameController
    {
        Cart cart = null;
        Shelf shelf = null;
        ScoreKeeper scoreKeeper = null;
        bool gameOver = false;
        public GameController()
        {
            cart = new Cart();
            shelf = new Shelf();
            scoreKeeper = new ScoreKeeper();
            gameOver = false;
        }

        public string GetRandomLetters()
        {
            string letters = String.Empty;

            foreach(char letter in shelf.GetLetters())
            {
                letters = letters + letter;
            }

            return letters;
        }

        public char GetShelfLetterByPosition(int position)
        {
            return shelf.SelectLetterByPosition(position);
        }

        public bool IsGameOver()
        {
            return gameOver;
        }

        public bool IsPalindrome(string word)
        {
            return StringUtility.IsPalindrome(word);
        }

        public bool IsSpelledCorrect(string word)
        {
            return StringUtility.IsSpelledCorrect(word);
        }

        public int GetLetterPointValue(char letter)
        {
            int pointValue = 0;

            try
            {
                pointValue = StringUtility.GetAlphabetPositionByLetter(letter);
            }
            catch(Exception ex)
            {
                throw new GameControllerInvalidCharException();
            }

            return pointValue;
        }

        public void AddToCart(char letter)
        {
            cart.AddLetter(letter);
        }

        public bool IsSpelledCorrect()
        {
            return IsSpelledCorrect(cart.GetWord());
        }

        public string GetCartLetters()
        {
            return cart.GetWord();
        }

        public bool IsPalindrome()
        {
            return IsPalindrome(cart.GetWord());
        }

        public int GetScore()
        {
            return scoreKeeper.GetScore();
        }

        public void CollectCart()
        {
            int cartValue = GetWordValue();

            scoreKeeper.AddPoints(cartValue);

            if (IsPalindrome())
            {
                scoreKeeper.AddPoints(50);
            }

            cart.DiscardCart();
        }

        private int GetWordValue()
        {
            int wordValue = 0;

            string word = cart.GetWord();

            foreach (char letter in word)
            {
                wordValue = wordValue + GetLetterPointValue(letter);
                
            }

            return wordValue;
        }

        public bool IsCartEmpty()
        {
            return cart.IsEmpty();
        }

        public void DiscardCart()
        {
            int cartValue = GetWordValue();

            scoreKeeper.SubtractPoints(cartValue);

            cart.DiscardCart();
        }

        public void SubtractFromScore(int pointValue)
        {
            scoreKeeper.SubtractPoints(pointValue);
        }

        public void GetNextRandomLetterForPosition(int position)
        {
            shelf.DiscardLetterByPosition(position - 1);
        }
    }
}
