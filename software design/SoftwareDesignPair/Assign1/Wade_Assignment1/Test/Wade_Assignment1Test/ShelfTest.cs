using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wade_Assignment1.Core;

namespace Wade_Assignment1Test
{
    [TestClass]
    public class ShelfTest
    {
        Shelf shelf;

        public ShelfTest()
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

        [TestInitialize()]
        public void TestSetup() 
        {
            shelf = new Shelf();
        }

        [TestMethod]
        public void TestCanary()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestCreateShelf()
        {
            Assert.IsFalse(shelf.IsEmpty());
        }

        [TestMethod]
        public void TestSelectLetter()
        {
            char firstLetter = shelf.SelectLetterByPosition(1);

            Assert.AreEqual(firstLetter, shelf.SelectLetterByPosition(1));
        }



    }
}
