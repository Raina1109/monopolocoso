using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wade_Assignment1.Core;

namespace Wade_Assignment1Test
{
    [TestClass]
    public class CartTest
    {
        Cart cart;

        public CartTest()
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

        [TestInitialize]
        public void TestSetup()
        {
            cart = new Cart();
        }

        [TestMethod]
        public void TestCanary()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestCreateCart()
        {
            Assert.IsTrue(cart.IsEmpty());
        }

        [TestMethod]
        public void TestAddLetter()
        {
            cart.AddLetter('A');

            Assert.IsTrue(cart.GetLetterCount() == 1);
        }

        [TestMethod]
        public void TestAddTwoLetters()
        {
            cart.AddLetter('A');
            cart.AddLetter('B');

            Assert.IsTrue(cart.GetLetterCount() == 2);
        }


        [TestMethod]
        public void TestDiscardCart()
        {
            cart.AddLetter('A');
            cart.AddLetter('B');
            cart.AddLetter('C');

            cart.DiscardCart();

            Assert.IsTrue(cart.IsEmpty());
        }

        [TestMethod]
        public void TestDiscardEmptyCart()
        {
            cart.DiscardCart();

            Assert.IsTrue(cart.IsEmpty());
        }

        [TestMethod]
        public void TestAddToDiscardedCart()
        {
            cart.DiscardCart();

            cart.AddLetter('A');
            cart.AddLetter('B');

            Assert.IsTrue(cart.GetLetterCount() == 2);
        }

        [TestMethod]
        public void TestAddToFullCart()
        {
            try
            {
                cart.AddLetter('A');
                cart.AddLetter('B');
                cart.AddLetter('C');
                cart.AddLetter('D');
                cart.AddLetter('E');
                cart.AddLetter('F');
                cart.AddLetter('G');
                cart.AddLetter('H');

                Assert.Fail("Expected exception for adding to a full cart");
            }
            catch (CartFullException cartFullException)
            {
            }
        }

        [TestMethod]
        public void TestCollectCart()
        {
           //check spelling

            //if spelling is correct
                
                //Total letters for scoring
                
                //Empty Cart
        }
    }
}
