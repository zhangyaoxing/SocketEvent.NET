using SocketEvent.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SocketEvent.Dto;
using SocketEvent;
using System.Threading;

namespace SocketEventTest
{


    /// <summary>
    ///This is a test class for SocketEventClientTest and is intended
    ///to contain all SocketEventClientTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SocketEventClientTest
    {
        public const string URL = "http://192.168.122.1:2900";

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
            ISocketEventClient client;
            client = SocketEventClientFactory.CreateInstance(URL);
            Assert.IsNotNull(client);
        }

        /// <summary>
        ///A test for Subscribe
        ///</summary>
        [TestMethod()]
        public void SubscribeTest()
        {
            ISocketEventClient client;
            client = SocketEventClientFactory.CreateInstance("SubscriberID", URL);
            string eventName = "TestEvent";
            ISocketEventResponse serverResponse = null;
            ISocketEventRequest serverRequest = null;
            Semaphore s = new Semaphore(0, 1);
            client.Subscribe(eventName,
                new Func<ISocketEventRequest,RequestResult>((request) =>
                {
                    serverRequest = request;
                    s.Release();
                    return RequestResult.Success;
                }),
                (response) =>
                {
                    serverResponse = response;
                });
            client.Enqueue(eventName);
            s.WaitOne();

            Assert.AreEqual(RequestResult.Success, serverResponse.Status);
            Assert.AreEqual(ClientState.Connected, client.State);
            Assert.AreEqual(eventName, serverRequest.EventName);
            Assert.IsNotNull(serverRequest.RequestId);
        }

        [TestMethod()]
        public void EnqueueTest()
        {
            ISocketEventClient client;
            client = SocketEventClientFactory.CreateInstance(URL);
            string eventName = "TestEvent";
            ISocketEventResponse response = null;
            Semaphore s = new Semaphore(0, 1);

            client.Enqueue(eventName, (data) =>
                {
                    response = data;
                    s.Release();
                });
            s.WaitOne(3000);

            Assert.AreEqual(RequestResult.Success, response.Status);
            Assert.AreEqual(ClientState.Connected, client.State);
        }

        [TestMethod()]
        public void StateChangeTest()
        {

        }
    }
}
