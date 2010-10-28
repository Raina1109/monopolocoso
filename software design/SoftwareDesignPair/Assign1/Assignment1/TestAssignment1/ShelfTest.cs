using Src;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Test
{
    [TestClass()]
    public class ShelfTest
    {

        private Shelf shelf;
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
        public void TestSetup()
        {
            shelf = new Shelf();
        }
        [TestCleanup]
        public void TestCleanup()
        {
            shelf = null;
        }
        
        [TestMethod]
        public void ShelfConstructorTest()
        {
            Assert.IsFalse(shelf.IsEmpty());
        }
        [TestMethod]
        public void TestSelectLetter()
        {
            char firstLetter = shelf.getLetter(1);

            Assert.AreEqual(firstLetter, shelf.getLetter(1));
        }
        [TestMethod]
        public void RandomLetter()
        {
            char letter = shelf.getLetter(2);
            shelf.random(2);
            Assert.AreNotEqual(letter, shelf.getLetter(2));
        }
    }
}
