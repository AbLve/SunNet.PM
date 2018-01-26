using SunNet.PMNew.App;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunNet.PMNew.Entity.TicketModel;
using Pm2012TEST.Fakes;
using SunNet.PMNew.Core.TicketModule;

namespace Pm2012TEST
{


    /// <summary>
    ///This is a test class for TicketsApplicationTest and is intended
    ///to contain all TicketsApplicationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TicketChangeBugToRequest
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
        /// type 1: for bug->request,delete bug
        /// return code : 100 ,true
        /// <summary>
        ///A test for TicketChangeBugToRequest
        ///</summary>
        [TestMethod()]
        public void TicketChangeBugToRequestType1Test()
        {
            //FakeEmailSender email = new FakeEmailSender(); // TODO: Initialize to an appropriate value
            //FakeTicketEntity ticketE = new FakeTicketEntity(); // TODO: Initialize to an appropriate value
            //FakeTicketUsers ticketU = new FakeTicketUsers(); // TODO: Initialize to an appropriate value
            //FakeTicketsRepository res = new FakeTicketsRepository();
            //FakeTicketCache cache = new FakeTicketCache();

            //FakeTicketEntity teC = new FakeTicketEntity();

            //int userId = 0; // TODO: Initialize to an appropriate value
            //TicketsManager tm = new TicketsManager(userId, email, res, cache, new FakeGetUserInfo());

            //int tid = 1001; // TODO: Initialize to an appropriate value
            //int type = 1; // TODO: Initialize to an appropriate value
            //string descr = string.Empty; // TODO: Initialize to an appropriate value
            //TicketsEntity te = teC.CreateTicketsEntity(3); // TODO: Initialize to an appropriate value

            //bool expected = true; // TODO: Initialize to an appropriate value
            //bool actual;
            //int expectedCode = 100;

            //actual = tm.ChangeTicketStatesBugToRequest(tid, type, descr, te);
            //int recordCode = res.UpdateCode;
            //Assert.AreEqual(expectedCode, recordCode);
            //Assert.AreEqual(expected, actual);
        }
        /// type 2: for bug->request,leave bug & change bug descr
        /// return code : 200 ,true
        [TestMethod()]
        public void TicketChangeBugToRequestType2Test()
        {
            //FakeEmailSender email = new FakeEmailSender(); // TODO: Initialize to an appropriate value
            //FakeTicketEntity ticketE = new FakeTicketEntity(); // TODO: Initialize to an appropriate value
            //FakeTicketUsers ticketU = new FakeTicketUsers(); // TODO: Initialize to an appropriate value
            //FakeTicketsRepository res = new FakeTicketsRepository();
            //FakeTicketCache cache = new FakeTicketCache();

            //FakeTicketEntity teC = new FakeTicketEntity();

            //int userId = 0; // TODO: Initialize to an appropriate value
            //TicketsManager tm = new TicketsManager(userId, email, res, cache, new FakeGetUserInfo());

            //int tid = 1000; // TODO: Initialize to an appropriate value
            //int type = 2; // TODO: Initialize to an appropriate value
            //string descr = "test"; // TODO: Initialize to an appropriate value
            //TicketsEntity te = teC.CreateTicketsEntity(1); // TODO: Initialize to an appropriate value
            //int expectedCode = 200;
        
            //bool expected = true; // TODO: Initialize to an appropriate value
            //bool actual;
            //actual = tm.ChangeTicketStatesBugToRequest(tid, type, descr, te);
            //int recordCode = res.UpdateCode;
            //Assert.AreEqual(expected, actual);
            //Assert.AreEqual(expectedCode, recordCode);
        }

        /// type 3: for bug->request,change bug state to history
        /// return code : 300 ,true
        [TestMethod()]
        public void TicketChangeBugToRequestType3Test()
        {
            //FakeEmailSender email = new FakeEmailSender(); // TODO: Initialize to an appropriate value
            //FakeTicketEntity ticketE = new FakeTicketEntity(); // TODO: Initialize to an appropriate value
            //FakeTicketUsers ticketU = new FakeTicketUsers(); // TODO: Initialize to an appropriate value
            //FakeTicketsRepository res = new FakeTicketsRepository();
            //FakeTicketCache cache = new FakeTicketCache();

            //FakeTicketEntity teC = new FakeTicketEntity();

            //int userId = 0; // TODO: Initialize to an appropriate value
            ////TicketsManager tm = new TicketsManager(userId, email, res, cache, new FakeGetUserInfo());

            //int tid = 999; // TODO: Initialize to an appropriate value
            //int type = 3; // TODO: Initialize to an appropriate value
            //string descr = "test"; // TODO: Initialize to an appropriate value
            //TicketsEntity te = teC.CreateTicketsEntity(1); // TODO: Initialize to an appropriate value
            //int expectedCode = 300;
          

            //bool expected = true; // TODO: Initialize to an appropriate value
            //bool actual;
            //actual = tm.ChangeTicketStatesBugToRequest(tid, type, descr, te);
            //int recordCode = res.UpdateCode;
            //Assert.AreEqual(expected, actual);
            //Assert.AreEqual(expectedCode, recordCode);
        }
    }
}
