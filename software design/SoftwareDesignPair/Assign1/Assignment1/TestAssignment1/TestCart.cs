using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Src;
using NHunspell;

namespace Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class TestCart
    {
        Cart cart;
        public TestCart()
        {
        }
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
        [TestInitialize]
        public void TestInitialize()
        {
            cart = new Cart();
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            cart = null;
        }  
        [TestMethod]
        public void TestCanary()
        {
            Assert.IsTrue(true);
          
        }

        [TestMethod]
        public void TestCreateCart()
        {
            Assert.IsNotNull(cart);
            Assert.AreEqual("-------", cart.getString());
        }
        
        [TestMethod]
        public void TestAddLetter()
        {
            cart.addCharacter('a',3);
            Assert.AreEqual('a',cart.getChar(3));
            
        }
        [TestMethod]
        public void TestAddLetterInOccupiedCell()
        {
            try
            {
                cart.addCharacter('a', 1);
                cart.addCharacter('b', 1);
                Assert.Fail("fail to catch ovewriting");
            }
            catch (LetterCartException ex)
            {

                //success
            }
        }
        [TestMethod]
        public void TestAddLetterOutsideRange()
        {
            try
            {
                cart.addCharacter('a', 10);
                Assert.Fail("fail to catch out of bound adding");
            }
            catch (IndexOutOfRangeException)
            {
                //success
            }
        }
        [TestMethod]
        public void TestCollectCartSuccess()
        {
            cart.addCharacter('c', 1);
            cart.addCharacter('a', 2);
            cart.addCharacter('t', 3);
            Assert.AreEqual(18, cart.collect());
        }
        [TestMethod]
        public void TestCollectDoubleWords()
        {
            cart.addCharacter('c', 1);
            cart.addCharacter('a', 2);
            cart.addCharacter('t', 3);
            cart.addCharacter('d', 5);
            cart.addCharacter('o', 6);
            cart.addCharacter('g', 7);
            Assert.AreEqual(0, cart.collect());
        }
        [TestMethod]
        public void TestCollectEmptyCart()
        {
            Assert.AreEqual(0,cart.collect());
        }
        [TestMethod]
        public void TestCollectWrongSpelling()
        {
            cart.addCharacter('c', 1);
            cart.addCharacter('a', 2);
            cart.addCharacter('a', 3);
            Assert.AreEqual(0, cart.collect());
        }
        [TestMethod]
        public void TestDiscardCart()
        {
            Assert.IsTrue(cart.discard());
            Assert.AreEqual("-------",cart.getString());
        }
        [TestMethod]
        public void getLetterOutOfRange()
        {
            try
            {
                cart.getChar(9);
                Assert.Fail("fail to catch getting letter not in range");
            }
            catch(LetterCartException ex)
            {
                
            }
        }
        [TestMethod]
        public void getEmptyLetter()
        {
            Assert.AreEqual('-', cart.getChar(1));
        }
    }
}
