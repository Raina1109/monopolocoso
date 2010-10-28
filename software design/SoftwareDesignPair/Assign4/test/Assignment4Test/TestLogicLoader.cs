using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Assignment4.Core;

namespace Assignment4Test
{
    [TestFixture]
    public class TestLogicLoader
    {
        [Test]
        public void TestCreateLogicLoader()
        {
            LogicLoader logicLoader = LogicLoader.Instance;

            Assert.IsTrue(logicLoader.Detectors.Count > 0);
        }
    }
}
