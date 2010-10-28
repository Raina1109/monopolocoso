using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Assignment4.Core;

namespace Assignment4Test
{
    [TestFixture]
    public class TestAlphabetDetector
    {
        AlphabetDetector alphabetDetector;

        [SetUp]
        public void Initialize()
        {
            alphabetDetector = new AlphabetDetector();
        }

        [Test]
        public void TestAlphabetDetectorRunSuccess()
        {
            Assert.IsTrue(alphabetDetector.IsSpecial("almost"));
        }

        [Test]
        public void TestAlphabetDetectorNegative()
        {
            Assert.IsFalse(alphabetDetector.IsSpecial("hello"));
        }
    }
}
