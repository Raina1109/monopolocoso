using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wade_Assignment1.Core;

namespace Wade_Assignment1Test
{
    [TestClass]
    public class GameControllerTest
    {
        GameController gameController;

        public GameControllerTest()
        {
        }

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestInitialize]
        public void Setup()
        {
            gameController = new GameController();
        }

        [TestMethod]
        public void TestCreateGameController()
        {
            Assert.IsFalse(gameController.IsGameOver());
        }

        [TestMethod]
        public void TestCheckCartForPalindrome()
        {
            string word = "level";
            Assert.IsTrue(gameController.IsPalindrome(word));
        }

        [TestMethod]
        public void TestCheckCartForPalindromeForANonPalidrome()
        {
            string word = "cat";
            Assert.IsFalse(gameController.IsPalindrome(word));
        }

        [TestMethod]
        public void TestSpellingOfCorrectlySpelledWord()
        {
            string word = "cat";
            Assert.IsTrue(gameController.IsSpelledCorrect(word));
        }

        [TestMethod]
        public void TestSpellingOfIncorrectlySpelledWord()
        {
            string word = "catd";
            Assert.IsFalse(gameController.IsSpelledCorrect(word));
        }

        [TestMethod]
        public void TestValueForALetter()
        {
            char letter = 'c';

            Assert.AreEqual(3, gameController.GetLetterPointValue(letter));
        }

        [TestMethod]
        public void TestValueForInvalidLetter()
        {
            try
            {
                char letter = '4';
                int points = gameController.GetLetterPointValue(letter);

                Assert.Fail("Expected exception for invalid character");
            }
            catch (GameControllerInvalidCharException gameControllerInvalidCharException)
            {
            }
        }

        [TestMethod]
        public void TestCollectCart()
        {
            gameController.AddToCart('d');
            gameController.AddToCart('o');
            gameController.AddToCart('g');

            Assert.IsTrue(gameController.IsSpelledCorrect());

            gameController.CollectCart();

            Assert.AreEqual(26, gameController.GetScore());

            Assert.IsTrue(gameController.IsCartEmpty());
        }

        [TestMethod]
        public void TestDiscardCart()
        {
            gameController.AddToCart('d');
            gameController.AddToCart('w');
            gameController.AddToCart('f');
            gameController.AddToCart('d');

            gameController.DiscardCart();

            Assert.AreEqual(-37, gameController.GetScore());

            Assert.IsTrue(gameController.IsCartEmpty());
        }
    }
}
