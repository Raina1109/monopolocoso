using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CityTemperature.Core;

namespace CityTemperature.Test
{
    [TestClass]
    public class TemperatureFinderTest
    {
        TemperatureFinder temperatureFinder;

        public TemperatureFinderTest()
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
        public void MyTestInitialize() 
        {
            temperatureFinder = new TemperatureFinder();
        }

        [TestMethod]
        public void TestCreateTemperatureFinder()
        {
            Assert.IsTrue(temperatureFinder.ProviderCount == 0);
        }

        [TestMethod]
        public void TestGetTemperature()
        {
            ProviderMock provider1 = new ProviderMock("Provider1", Reliability.Low);
            ProviderMock provider2 = new ProviderMock("Provider2", Reliability.High);

            temperatureFinder.AddProvider(provider1);
            temperatureFinder.AddProvider(provider2);


            Assert.AreNotEqual(temperatureFinder.GetTemperture(1234), -999 );
        }

        [TestMethod]
        public void TestAddProviderToTemperatureProviderList()
        {
            ProviderMock provider1 = new ProviderMock("Provider1", Reliability.Low);

            temperatureFinder.AddProvider(provider1);
            
            Assert.AreEqual(temperatureFinder.ProviderCount, 1);
        }

        [TestMethod]
        public void TestAddMultipleProvidersToTemperatureProviderList()
        {
            ProviderMock provider1 = new ProviderMock("Provider1", Reliability.Low);

            ProviderMock provider2 = new ProviderMock("Provider2", Reliability.Moderate);

            temperatureFinder.AddProvider(provider1);

            temperatureFinder.AddProvider(provider2);

            Assert.AreEqual(temperatureFinder.ProviderCount, 2);
        }

        [TestMethod]
        public void TestAddAndRemoveProviderFromTemperatureProviderList()
        {
            ProviderMock provider1 = new ProviderMock("Provider1", Reliability.Low);

            ProviderMock provider2 = new ProviderMock("Provider2", Reliability.Moderate);

            temperatureFinder.AddProvider(provider1);

            temperatureFinder.AddProvider(provider2);

            temperatureFinder.RemoveProvider(provider1);

            Assert.AreEqual(temperatureFinder.ProviderCount, 1);
        }

        [TestMethod]
        public void TestAddAndRemoveAllProviderFromTemperatureProviderList()
        {
            ProviderMock provider1 = new ProviderMock("Provider1", Reliability.Low);

            ProviderMock provider2 = new ProviderMock("Provider2", Reliability.Moderate);

            temperatureFinder.AddProvider(provider1);

            temperatureFinder.AddProvider(provider2);

            temperatureFinder.RemoveProvider(provider1);

            temperatureFinder.RemoveProvider(provider2);

            Assert.AreEqual(temperatureFinder.ProviderCount, 0);
        }

        [TestMethod]
        public void TestAddAndRemoveProviderAndVerifyExistingProviderInTemperatureProviderList()
        {
            ProviderMock provider1 = new ProviderMock("Provider1", Reliability.Low);

            ProviderMock provider2 = new ProviderMock("Provider2", Reliability.Moderate);

            temperatureFinder.AddProvider(provider1);

            temperatureFinder.AddProvider(provider2);

            temperatureFinder.RemoveProvider(provider1);

            Assert.IsTrue(temperatureFinder.ContainsProvider(provider2));
        }

        [TestMethod]
        public void TestRemoveAllFromEmptyTemperatureProviderList()
        {
            try
            {
                temperatureFinder.RemoveAllProviders();

                Assert.Fail("Expected exception to be thrown");
            }
            catch (TemperatureFinderProviderListException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void TestDoneLetSameProvidersBeAddedTwiceToTemperatureProviderList()
        {
            ProviderMock provider1 = new ProviderMock("Provider1", Reliability.Low);

            try
            {
                temperatureFinder.AddProvider(provider1);

                temperatureFinder.AddProvider(provider1);

                Assert.Fail("Expected to throw an exception");
            }
            catch (TemperatureFinderProviderListException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void TestRemoveAllFromTemperatureProviderList()
        {
            ProviderMock provider1 = new ProviderMock("Provider1", Reliability.Low);

            ProviderMock provider2 = new ProviderMock("Provider2", Reliability.Moderate);

            temperatureFinder.AddProvider(provider1);

            temperatureFinder.AddProvider(provider2);

            temperatureFinder.RemoveAllProviders();

            Assert.IsFalse(temperatureFinder.ProviderCount > 0);
        }

        [TestMethod]
        public void TestIFTemperatureProviderListIsSorted()
        {
            ProviderMock provider1 = new ProviderMock("Provider1", Reliability.Low);

            ProviderMock provider2 = new ProviderMock("Provider2", Reliability.Moderate);

            ProviderMock provider3 = new ProviderMock("Provider3", Reliability.High);

            temperatureFinder.AddProvider(provider1);

            temperatureFinder.AddProvider(provider3);

            temperatureFinder.AddProvider(provider2);

            Assert.IsTrue(temperatureFinder.IsProviderListSorted());
        }
    }
}
