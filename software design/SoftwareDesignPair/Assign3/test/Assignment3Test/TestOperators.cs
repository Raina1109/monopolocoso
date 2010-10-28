using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Assignment3.Core;
using System.IO;

namespace Assignment3Test
{
    [TestFixture]
    public class TestOperators
    {
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
        }

        [Test]
        public void TestLowerCaseOperator()
        {
            String testString = "TESTING";

            IOperation operation = new LowerCaseOperator();
            String result = operation.Transform(testString);

            Assert.AreEqual(result, "testing");
        }

        [Test]
        public void TestLowerCaseOperatorWithEmptyString()
        {
            String testString = String.Empty;

            IOperation operation = new LowerCaseOperator();
            String result = operation.Transform(testString);

            Assert.AreEqual(result, String.Empty);
        }

        [Test]
        public void TestUpperCaseOperator()
        {
            String testString = "testing";

            IOperation operation = new UpperCaseOperator();
            String result = operation.Transform(testString);

            Assert.AreEqual(result, "TESTING");
        }

        [Test]
        public void TestStupidRemoverOperator()
        {
            String testString = "This is really stupid!!!";

            IOperation operation = new StupidRemoverOperator();
            String result = operation.Transform(testString);

            Assert.AreEqual(result, "This is really s*****!!!");
        }

        [Test]
        public void TestStupidRemoverOperatorWithTwoStupidStrings()
        {
            String testString = "This is really stupid stupid!!!";

            IOperation operation = new StupidRemoverOperator();
            String result = operation.Transform(testString);

            Assert.AreEqual(result, "This is really s***** s*****!!!");
        }

        [Test]
        public void TestDuplicateRemover()
        {
            String testString = "This is is it";

            IOperation operation = new DuplicateRemoverOperator();
            String result = operation.Transform(testString);

            Assert.AreEqual(result, "This is it");
        }

        [Test]
        public void TestDuplicateRemoverWithMutipleDuplicates()
        {
            String testString = "This is is it really really";

            IOperation operation = new DuplicateRemoverOperator();
            String result = operation.Transform(testString);

            Assert.AreEqual(result, "This is it really");
        }
    }
}
