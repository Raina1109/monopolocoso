using Src;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Test
{
    
    
    /// <summary>
    ///This is a test class for ScoreKeeperTest and is intended
    ///to contain all ScoreKeeperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ScoreKeeperTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for getScore
        ///</summary>
        [TestMethod()]
        public void getScoreTest()
        {
            ScoreKeeper target = new ScoreKeeper(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.getScore();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for addPoint
        ///</summary>
        [TestMethod()]
        public void addPointTest()
        {
            ScoreKeeper target = new ScoreKeeper(); // TODO: Initialize to an appropriate value
            int pointDiff = 20; // TODO: Initialize to an appropriate value
            target.addPoint(pointDiff);
            int expected = 20;
            Assert.AreEqual(expected, target.getScore());
            
        }

        /// <summary>
        ///A test for ScoreKeeper Constructor
        ///</summary>
        [TestMethod()]
        public void ScoreKeeperConstructorTest()
        {
            ScoreKeeper target = new ScoreKeeper();
            Assert.IsNotNull(target);
            
        }
        [TestMethod]
        public void ScoreKeeperDeductBiggerThanTotalTest()
        {
            ScoreKeeper target = new ScoreKeeper();
            target.addPoint(20);
            target.addPoint(-30);
            int expected = 0;
            int actual = target.getScore();
            Assert.AreEqual(expected, actual);
        }
    }
}
