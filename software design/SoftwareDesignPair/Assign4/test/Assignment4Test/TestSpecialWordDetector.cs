using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Assignment4.Core;

namespace Assignment4Test
{
    [TestFixture]
    public class TestSpecialWordDetector
    {
        SpecialWordDetector specialWordDetector;

        [SetUp]
        public void Initialize()
        {
            specialWordDetector = new SpecialWordDetector();
        }

        [Test]
        public void TestSpecialWordDetectorWithNoDetectors()
        {
            string testWord = "madam";

            specialWordDetector.RemoveDetectors();

            Assert.IsTrue(specialWordDetector.IsSpecialWorld(testWord));
        }

        [Test]
        public void TestSpecialWordDetectorWithPalindromeDetectorWithPalindromeWord()
        {
            string testWord = "madam";

            Assert.IsTrue(specialWordDetector.IsSpecialWorld(testWord));
        }

        [Test]
        public void TestSpecialWordDetectorWithPalindromeDetectorWithNonPalindromeWord()
        {
            string testWord = "almost";

            Assert.IsTrue(specialWordDetector.IsSpecialWorld(testWord));
        }

        [Test]
        public void TestSpecialWordDetectorWithAlphabetDetectorWithAlphabetOrderWord()
        {
            string testWord = "almost";

            Assert.IsTrue(specialWordDetector.IsSpecialWorld(testWord));
        }

        [Test]
        public void TestSpecialWordDetectorWithAlphabetDetectorWithNonAlphabetOrderWord()
        {
            string testWord = "madam";

            Assert.IsTrue(specialWordDetector.IsSpecialWorld(testWord));
        }

        [Test]
        public void TestSpecialWordDetectorWithPalindromeAndAlphabetOrderDetectorsWithPalindromeWord()
        {
            string testWord = "madam";

            Assert.IsTrue(specialWordDetector.IsSpecialWorld(testWord));
        }

        [Test]
        public void TestSpecialWordDetectorWithPalindromeAndAlphabetOrderDetectorsWithAlphabeticallyOrderedWord()
        {
            string testWord = "almost";

            Assert.IsTrue(specialWordDetector.IsSpecialWorld(testWord));
        }
    }
}
