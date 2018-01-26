using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pm2012TEST.Fakes;
using SunNet.PMNew.Core.TicketModule;
using SunNet.PMNew.Entity.TicketModel;

namespace Pm2012TEST
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class AddTicketsMethodAndSendEmailTest
    {
        public AddTicketsMethodAndSendEmailTest()
        {
            //
            // TODO: Add constructor logic here
            //
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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestAddTicketMethod()
        {
            //
            // TODO: Add test logic	here
            //
            //FakeEmailSender emailSender = new FakeEmailSender(); // TODO: Initialize to an appropriate value
            //FakeTicketEntity ticketE = new FakeTicketEntity(); // TODO: Initialize to an appropriate value
            //FakeTicketUsers ticketU = new FakeTicketUsers(); // TODO: Initialize to an appropriate value

            //FakeUsersRepository fu = new FakeUsersRepository();

            //FakeTicketsRepository Repository = new FakeTicketsRepository();
            //FakeTicketCache cache = new FakeTicketCache();
            //// TicketsManager tm = new TicketsManager(0, emailSender, Repository, cache, new FakeGetUserInfo());
            //TicketsEntity te = ticketE.CreateTicketsEntity(1); // TODO: Initialize to an appropriate value
            //// int id = tm.AddTicket(te);
            //int count = emailSender.Count;
            //Assert.AreEqual(id, 100);
            //Assert.AreEqual(count, 1);
        }
    }
}
