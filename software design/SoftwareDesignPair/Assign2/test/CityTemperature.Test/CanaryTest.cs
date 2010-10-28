using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CityTemperature.Test
{
    [TestClass]
    public class CanaryTest
    {
        public CanaryTest()
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

        [TestMethod]
        public void TestCanary()
        {
            Assert.IsTrue(true);
        }
    }
}
