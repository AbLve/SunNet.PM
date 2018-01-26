using SunNet.PMNew.Core.TicketModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunNet.PMNew.Framework.Utils;
using System.Collections.Generic;
using Pm2012TEST.Fakes;
using SunNet.PMNew.Entity.TicketModel;

namespace Pm2012TEST
{


    /// <summary>
    ///This is a test class for TicketsManagerTest and is intended
    ///to contain all TicketsManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UpdateTicketsTest
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
        ///A test for UpdateTicketStateAndSendEmail
        ///</summary>
        [TestMethod()]
        public void UpdateTicketStateAndSendEmailTest()
        {
            int userId = 0; // TODO: Initialize to an appropriate value

            FakeEmailSender email = new FakeEmailSender(); // TODO: Initialize to an appropriate value
            FakeTicketEntity ticketE = new FakeTicketEntity(); // TODO: Initialize to an appropriate value
            FakeTicketUsers ticketU = new FakeTicketUsers(); // TODO: Initialize to an appropriate value
            FakeTicketsRepository res = new FakeTicketsRepository();
            FakeTicketCache cache = new FakeTicketCache();

           // TicketsManager tm = new TicketsManager(userId, email, res, cache, new FakeGetUserInfo());
            //bool expected = true; // TODO: Initialize to an appropriate value
            //bool actual;
            //Assert.AreEqual(res.SendSuc, false);
        }

        /// <summary>
        ///A test for UpdateTicket/email
        ///</summary>
        [TestMethod()]
        public void UpdateTicketTest()
        {
            int userId = 0; // TODO: Initialize to an appropriate value
            FakeEmailSender emailSender = new FakeEmailSender(); // TODO: Initialize to an appropriate value
            FakeTicketEntity ticketE = new FakeTicketEntity(); // TODO: Initialize to an appropriate value
            FakeTicketUsers ticketU = new FakeTicketUsers(); // TODO: Initialize to an appropriate value
            FakeTicketsRepository ticketsRespository = new FakeTicketsRepository();
            FakeTicketCache cache = new FakeTicketCache();
            FakeUsersRepository fur = new FakeUsersRepository();
            //TicketsManager target = new TicketsManager(userId, emailSender, ticketsRespository,
            //                                           cache, new FakeGetUserInfo(), fur); // TODO: Initialize to an appropriate value
            TicketsEntity te = ticketE.CreateTicketsEntity(1); // TODO: Initialize to an appropriate value
            te.Status = TicketsState.PMReviewed;
            int expected = 1; // TODO: Initialize to an appropriate value
            bool actual;

            //sactual = target.UpdateTicket(te);
            int ReturnCode = emailSender.Count;
            Assert.AreEqual(expected, ReturnCode);
        }
    }
}
