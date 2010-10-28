using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Assignment3.Core;


namespace Assignment3Test
{
    [TestFixture]
    public class FileWriterTest
    {
        FileWriter _fileWriter;
        string _fileFullPath = @"Test.txt";
        
		[SetUp]
        public void TestInitialize()
        {
            _fileWriter = new FileWriter(_fileFullPath);
        }

        [TearDown]
        public void TestFinalize()
        {
            _fileWriter = null;
        }

        private string ReadLineFromFile(string _fileFullPath)
        {
            System.IO.StreamReader streamReader = new System.IO.StreamReader(_fileFullPath);
            string line = String.Empty;

            line = streamReader.ReadLine();
            streamReader.Close();

            return line;
        }

        [Test]
        public void TestCreateFileWriter()
        {
            Assert.IsFalse(_fileWriter.IsClosed);
        }

        [Test]
        public void TestFileWriterWriteToFile()
        {
            string testValue = "Testing...";

            _fileWriter.Write(testValue);

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(testValue, line);
        }

        [Test]
        public void TestFileWriterWriteToNullString()
        {
                string testValue = null;

                _fileWriter.Write(testValue);

                string line = ReadLineFromFile(_fileFullPath);

                Assert.AreEqual(testValue, line);
        }

        [Test]
        public void TestFileWriterWithLowerCaseOperator()
        {
            string uppercaseTestString = "TESTING IS GREAT";

            _fileWriter = new Assignment3.Core.FileWriter(new LowerCaseOperator(), _fileFullPath);

            _fileWriter.Write(uppercaseTestString);

            

            string line = ReadLineFromFile(_fileFullPath);

            

            Assert.AreEqual(line, uppercaseTestString.ToLower());
        }

        [Test]
        public void TestFileWriterWithUpperCaseOperator()
        {
            string lowercaseTestString = "testing is great";

            _fileWriter = new Assignment3.Core.FileWriter(new UpperCaseOperator(), _fileFullPath);

            _fileWriter.Write(lowercaseTestString);

            

            string line = ReadLineFromFile(_fileFullPath);

            

            Assert.AreEqual(line, lowercaseTestString.ToUpper(), _fileFullPath);
        }

        [Test]
        public void TestFileWriterWithStupidRemoverOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new StupidRemoverOperator(), _fileFullPath);
            _fileWriter.Write("not testing is stupid");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "not testing is s*****");
        }

        [Test]
        public void TestFileWriterWithDuplicateRemoverOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new DuplicateRemoverOperator(), _fileFullPath);
            _fileWriter.Write("not testing is really really bad");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "not testing is really bad");
        }

        [Test]
        public void TestFileWriterWithLowerCaseOperatorThenUpperCaseOperator()
        {
            string uppercaseTestString = "TESTING IS GREAT";

            _fileWriter = new Assignment3.Core.FileWriter(new LowerCaseOperator(new UpperCaseOperator()), _fileFullPath);
            _fileWriter.Write(uppercaseTestString);

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, uppercaseTestString.ToLower());
        }

        [Test]
        public void TestFileWriterWithLowerCaseOperatorAfterStupidRemoverOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new LowerCaseOperator(new StupidRemoverOperator()), _fileFullPath);
            _fileWriter.Write("NOT TESTING IS stupid");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "not testing is s*****");
        }

        [Test]
        public void TestFileWriterWithLowerCaseOperatorAfterDuplicateRemoverOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new LowerCaseOperator(new DuplicateRemoverOperator()), _fileFullPath);
            _fileWriter.Write("NOT TESTING IS IS BAD");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "not testing is bad");
        }

        [Test]
        public void TestFileWriterWithLowerCaseOperatorAfterLowerCaseOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new LowerCaseOperator(new LowerCaseOperator()), _fileFullPath);
            _fileWriter.Write("NOT TESTING IS BAD");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "not testing is bad");
        }

        [Test]
        public void TestFileWriterWithUpperCaseOperatorAfterLowerCaseOperator()
        {
            string uppercaseTestString = "TESTING IS GREAT";

            _fileWriter = new Assignment3.Core.FileWriter(new UpperCaseOperator(new LowerCaseOperator()), _fileFullPath);
            _fileWriter.Write(uppercaseTestString);

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, uppercaseTestString);
        }

        [Test]
        public void TestFileWriterWithUpperCaseOperatorAfterStupidRemoverOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new UpperCaseOperator(new StupidRemoverOperator()), _fileFullPath);
            _fileWriter.Write("not testing is stupid");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "NOT TESTING IS S*****");
        }

        [Test]
        public void TestFileWriterWithUpperCaseOperatorAfterDuplicateRemoverOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new UpperCaseOperator(new DuplicateRemoverOperator()), _fileFullPath);
            _fileWriter.Write("NOT TESTING IS IS BAD");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "NOT TESTING IS BAD");
        }

        [Test]
        public void TestFileWriterWithUpperCaseOperatorAfterUpperCaseOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new UpperCaseOperator(new UpperCaseOperator()), _fileFullPath);
            _fileWriter.Write("NOT TESTING IS BAD");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "NOT TESTING IS BAD");
        }

        [Test]
        public void TestFileWriterWithStupidRemoverOperatorAfterUpperCaseOperator()
        {

            _fileWriter = new Assignment3.Core.FileWriter(new StupidRemoverOperator(new UpperCaseOperator()), _fileFullPath);
            _fileWriter.Write("not testing is stupid");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "NOT TESTING IS STUPID");
        }

        [Test]
        public void TestFileWriterWithStupidRemoverOperatorAfterStupidRemoverOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new StupidRemoverOperator(new StupidRemoverOperator()), _fileFullPath);
            _fileWriter.Write("not testing is stupid");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "not testing is s*****");
        }

        [Test]
        public void TestFileWriterWithStupidRemoverOperatorAfterDuplicateRemoverOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new StupidRemoverOperator(new DuplicateRemoverOperator()), _fileFullPath);
            _fileWriter.Write("not testing is is bad");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "not testing is bad");
        }

        [Test]
        public void TestFileWriterWithStupidRemoverOperatorAfterLowerCaseOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new StupidRemoverOperator(new LowerCaseOperator()), _fileFullPath);
            _fileWriter.Write("NOT TESTING IS stupid");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "not testing is s*****");
        }

        [Test]
        public void TestFileWriterWithDuplicateRemoverOperatorAfterUpperCaseOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new DuplicateRemoverOperator(new UpperCaseOperator()), _fileFullPath);
            _fileWriter.Write("not testing is is stupid");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "NOT TESTING IS STUPID");
        }

        [Test]
        public void TestFileWriterWithDuplicateRemoverOperatorAfterStupidRemoverOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new DuplicateRemoverOperator(new StupidRemoverOperator()), _fileFullPath);
            _fileWriter.Write("not testing is is stupid");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "not testing is s*****");
        }

        [Test]
        public void TestFileWriterWithDuplicateRemoverOperatorAfterDuplicateRemoverOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new DuplicateRemoverOperator(new DuplicateRemoverOperator()), _fileFullPath);
            _fileWriter.Write("not testing is is bad");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "not testing is bad");
        }

        [Test]
        public void TestFileWriterWithDuplicateRemoverOperatorAfterLowerCaseOperator()
        {
            _fileWriter = new Assignment3.Core.FileWriter(new DuplicateRemoverOperator(new LowerCaseOperator()), _fileFullPath);
            _fileWriter.Write("NOT TESTING IS IS bad");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreEqual(line, "not testing is bad");
        }

        [Test]
        public void TestFileWriterClose()
        {
            _fileWriter = new Assignment3.Core.FileWriter(_fileFullPath);
            _fileWriter.Write("hello");

            _fileWriter.Close();

            Assert.AreEqual(_fileWriter.IsClosed, true);
        }

        [Test]
        public void TestFileWriterWriteAfterClose()
        {
            _fileWriter = new Assignment3.Core.FileWriter(_fileFullPath);
            _fileWriter.Write("hello");

            _fileWriter.Close();

            _fileWriter.Write("hello world");

            string line = ReadLineFromFile(_fileFullPath);

            Assert.AreNotEqual(line, "hello world");
        }
    }
}
