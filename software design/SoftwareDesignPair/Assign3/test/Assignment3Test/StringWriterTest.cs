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
    public class StringWriterTest 
    {
        Assignment3.Core.StringWriter stringWriter;

        public StringWriterTest()
        {
        }

        [SetUp]
        public void TestInitialize()
        {
            stringWriter = new Assignment3.Core.StringWriter();
        }

        [TearDown]
        public void TestFinalize()
        {
            stringWriter = null;
        }

        [Test]
        public void TestCreateStringWriter()
        {
            Assert.IsFalse(stringWriter.IsClosed);
        }

        [Test]
        public void TestWriteToString()
        {
            string testString = "testing is great";

            stringWriter.Write(testString);

            Assert.AreEqual(stringWriter.Contents, testString);
        }

        [Test]
        public void TestStringWriterWithLowerCaseOperator()
        {
            string uppercaseTestString = "TESTING IS GREAT";

            stringWriter = new Assignment3.Core.StringWriter(new LowerCaseOperator());

            stringWriter.Write(uppercaseTestString);

            Assert.AreEqual(stringWriter.Contents, uppercaseTestString.ToLower());
        }

        [Test]
        public void TestStringWriterWithUpperCaseOperator()
        {
            string lowercaseTestString = "testing is great";

            stringWriter = new Assignment3.Core.StringWriter(new UpperCaseOperator());

            stringWriter.Write(lowercaseTestString);

            Assert.AreEqual(stringWriter.Contents, lowercaseTestString.ToUpper());
        }

        [Test]
        public void TestStringWriterWithStupidRemoverOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new StupidRemoverOperator());

            stringWriter.Write("not testing is stupid");

            Assert.AreEqual(stringWriter.Contents, "not testing is s*****");
        }

        [Test]
        public void TestStringWriterWithDuplicateRemoverOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new DuplicateRemoverOperator());

            stringWriter.Write("not testing is really really bad");

            Assert.AreEqual(stringWriter.Contents, "not testing is really bad");
        }

        [Test]
        public void TestStringWriterWithLowerCaseOperatorAfterUpperCaseOperator()
        {
            string uppercaseTestString = "TESTING IS GREAT";

            stringWriter = new Assignment3.Core.StringWriter(new LowerCaseOperator(new UpperCaseOperator()));

            stringWriter.Write(uppercaseTestString);

            Assert.AreEqual(stringWriter.Contents, uppercaseTestString.ToLower());
        }

        [Test]
        public void TestStringWriterWithLowerCaseOperatorAfterStupidRemoverOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new LowerCaseOperator(new  StupidRemoverOperator()));

            stringWriter.Write("NOT TESTING IS stupid");

            Assert.AreEqual(stringWriter.Contents, "not testing is s*****");
        }

        [Test]
        public void TestStringWriterWithLowerCaseOperatorAfterDuplicateRemoverOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new LowerCaseOperator(new DuplicateRemoverOperator()));

            stringWriter.Write("NOT TESTING IS IS BAD");

            Assert.AreEqual(stringWriter.Contents, "not testing is bad");
        }

        [Test]
        public void TestStringWriterWithLowerCaseOperatorAfterLowerCaseOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new LowerCaseOperator(new LowerCaseOperator()));

            stringWriter.Write("NOT TESTING IS BAD");

            Assert.AreEqual(stringWriter.Contents, "not testing is bad");
        }

        [Test]
        public void TestStringWriterWithUpperCaseOperatorAfterLowerCaseOperator()
        {
            string uppercaseTestString = "TESTING IS GREAT";

            stringWriter = new Assignment3.Core.StringWriter(new UpperCaseOperator(new LowerCaseOperator()));

            stringWriter.Write(uppercaseTestString);

            Assert.AreEqual(stringWriter.Contents, uppercaseTestString);
        }

        [Test]
        public void TestStringWriterWithUpperCaseOperatorAfterStupidRemoverOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new UpperCaseOperator(new StupidRemoverOperator()));

            stringWriter.Write("not testing is stupid");

            Assert.AreEqual(stringWriter.Contents, "NOT TESTING IS S*****");
        }

        [Test]
        public void TestStringWriterWithUpperCaseOperatorAfterDuplicateRemoverOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new UpperCaseOperator(new DuplicateRemoverOperator()));

            stringWriter.Write("NOT TESTING IS IS BAD");

            Assert.AreEqual(stringWriter.Contents, "NOT TESTING IS BAD");
        }

        [Test]
        public void TestStringWriterWithUpperCaseOperatorAfterUpperCaseOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new UpperCaseOperator(new UpperCaseOperator()));

            stringWriter.Write("NOT TESTING IS BAD");

            Assert.AreEqual(stringWriter.Contents, "NOT TESTING IS BAD");
        }

        [Test]
        public void TestStringWriterWithStupidRemoverOperatorAfterUpperCaseOperator()
        {

            stringWriter = new Assignment3.Core.StringWriter(new StupidRemoverOperator(new UpperCaseOperator()));

            stringWriter.Write("not testing is stupid");

            Assert.AreEqual(stringWriter.Contents, "NOT TESTING IS STUPID");
        }

        [Test]
        public void TestStringWriterWithStupidRemoverOperatorAfterStupidRemoverOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new StupidRemoverOperator(new StupidRemoverOperator()));

            stringWriter.Write("not testing is stupid");

            Assert.AreEqual(stringWriter.Contents, "not testing is s*****");
        }

        [Test]
        public void TestStringWriterWithStupidRemoverOperatorAfterDuplicateRemoverOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new StupidRemoverOperator(new DuplicateRemoverOperator()));

            stringWriter.Write("not testing is is bad");

            Assert.AreEqual(stringWriter.Contents, "not testing is bad");
        }

        [Test]
        public void TestStringWriterWithStupidRemoverOperatorAfterLowerCaseOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new StupidRemoverOperator(new LowerCaseOperator()));

            stringWriter.Write("NOT TESTING IS stupid");

            Assert.AreEqual(stringWriter.Contents, "not testing is s*****");
        }

        [Test]
        public void TestStringWriterWithDuplicateRemoverOperatorAfterUpperCaseOperator()
        {

            stringWriter = new Assignment3.Core.StringWriter(new DuplicateRemoverOperator(new UpperCaseOperator()));

            stringWriter.Write("not testing is is stupid");

            Assert.AreEqual(stringWriter.Contents, "NOT TESTING IS STUPID");
        }

        [Test]
        public void TestStringWriterWithDuplicateRemoverOperatorAfterStupidRemoverOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new DuplicateRemoverOperator(new StupidRemoverOperator()));

            stringWriter.Write("not testing is is stupid");

            Assert.AreEqual(stringWriter.Contents, "not testing is s*****");
        }

        [Test]
        public void TestStringWriterWithDuplicateRemoverOperatorAfterDuplicateRemoverOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new DuplicateRemoverOperator(new DuplicateRemoverOperator()));

            stringWriter.Write("not testing is is bad");

            Assert.AreEqual(stringWriter.Contents, "not testing is bad");
        }

        [Test]
        public void TestStringWriterWithDuplicateRemoverOperatorAfterLowerCaseOperator()
        {
            stringWriter = new Assignment3.Core.StringWriter(new DuplicateRemoverOperator(new LowerCaseOperator()));

            stringWriter.Write("NOT TESTING IS IS bad");

            Assert.AreEqual(stringWriter.Contents, "not testing is bad");
        }

        [Test]
        public void TestStringWriterClose()
        {
            stringWriter = new Assignment3.Core.StringWriter();

            stringWriter.Write("hello");

            stringWriter.Close();

            Assert.AreEqual(stringWriter.IsClosed, true);
        }

        [Test]
        public void TestStringWriterWriteAfterClose()
        {
            stringWriter = new Assignment3.Core.StringWriter();

            stringWriter.Write("hello");

            stringWriter.Close();

            stringWriter.Write("hello world");

            Assert.AreNotEqual(stringWriter.Contents, "hello world");
        }
    }
}
