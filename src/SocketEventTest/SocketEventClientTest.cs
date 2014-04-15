using SocketEvent.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SocketEvent.Dto;

namespace SocketEventTest
{
    
    
    /// <summary>
    ///This is a test class for SocketEventClientTest and is intended
    ///to contain all SocketEventClientTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SocketEventClientTest
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
        ///A test for Connect
        ///</summary>
        [TestMethod()]
        public void ConnectTest()
        {
            string url = "http://192.168.122.1:2900";
            SocketEventClient actual;
            actual = SocketEventClient.Connect(url);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for Subscribe
        ///</summary>
        [TestMethod()]
        public void SubscribeTest()
        {
            string url = "http://192.168.122.1:2900";
            SocketEventClient client;
            client = SocketEventClient.Connect(url);
            string eventName = "TestEvent";
            client.Subscribe(eventName);
            Assert.AreEqual(ClientState.Connected, client.State);
        }
    }
}
