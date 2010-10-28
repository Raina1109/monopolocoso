using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wade_Assignment1.Core;

namespace Wade_Assignment1Test
{
    [TestClass]
    public class ScoreKeeperTest
    {
        ScoreKeeper scoreKeeper;

        public ScoreKeeperTest()
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
            scoreKeeper = new ScoreKeeper();
        }

        [TestMethod]
        public void TestCanary()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestCreateScoreKeeper()
        {
            Assert.IsTrue(scoreKeeper.GetScore() == 0);
        }

        [TestMethod]
        public void TestAddPointsToScore()
        {
            scoreKeeper.AddPoints(50);
            Assert.IsTrue(scoreKeeper.GetScore() == 50);
        }

        [TestMethod]
        public void TestAddPointsToScoreTheSubtractPoints()
        {
            scoreKeeper.AddPoints(50);
            scoreKeeper.SubtractPoints(30);

            Assert.IsTrue(scoreKeeper.GetScore() == 20);
        }

        [TestMethod]
        public void TestSubtractPointsFromAZeroScore()
        {
            scoreKeeper.SubtractPoints(30);
            Assert.IsTrue(scoreKeeper.GetScore() == -30);
        }

        [TestMethod]
        public void TestAddNegativePointsToScore()
        {
            scoreKeeper.AddPoints(-5);
            Assert.IsTrue(scoreKeeper.GetScore() == -5);
        }

        [TestMethod]
        public void TestSubtractNegativePointsScore()
        {
            scoreKeeper.SubtractPoints(-30);
            Assert.IsTrue(scoreKeeper.GetScore() == 30);
        }
    }
}
