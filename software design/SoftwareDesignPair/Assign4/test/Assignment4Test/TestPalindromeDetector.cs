using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Assignment4.Core;

namespace Assignment4Test
{
    [TestFixture]
    public class TestPalindromeDetector
    {
        PalindromeDetector palindromeDetector;

        [SetUp]
        public void Initialize()
        {
            palindromeDetector = new PalindromeDetector();
        }

        [Test]
        public void TestPalindromeDetectorWithPalindromeWord()
        {
            Assert.IsTrue(palindromeDetector.IsSpecial("rotor"));
        }

        [Test]
        public void TestPalindromeDetectorWithNonPalindromeWord()
        {
            Assert.IsFalse(palindromeDetector.IsSpecial("bike"));
        }

        [Test]
        public void TestPalindromeDetectorWithEmptyWord()
        {
            Assert.IsFalse(palindromeDetector.IsSpecial(String.Empty));
        }
    }
}
