using SunNet.PMNew.Core.UserModule;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunNet.PMNew.Framework.Utils;
using SunNet.PMNew.Entity.UserModel;
using Pm2012TEST.Fakes;
using NUnit.Framework;
using System;

namespace Pm2012TEST
{


    /// <summary>
    ///This is a test class for UserManagerTest and is intended
    ///to contain all UserManagerTest Unit Tests
    ///</summary>
    //[TestClass()]
    [TestFixture]
    public class UserManagerTest
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
        ///A test for AddUser
        ///</summary>
        //[TestMethod()]
        [Test]
        public void AddUserTest_Add_A_Client()
        {
            //int userID = 0; // TODO: Initialize to an appropriate value
            //EmailSenderFake emailSender = new EmailSenderFake(); // TODO: Initialize to an appropriate value
            //ICache<UserManager> cache = null; // TODO: Initialize to an appropriate value
            //IUsersRepository userRepository = new UserRepositoryFake(); // TODO: Initialize to an appropriate value
            //IRolesRepository roleRepository = null; // TODO: Initialize to an appropriate value
            //IModulesRepository moduleRepository = null; // TODO: Initialize to an appropriate value
            //IRoleModulesRepository rmRespository = null; // TODO: Initialize to an appropriate value
            //UserManager target = new UserManager(userID, emailSender, cache, userRepository, roleRepository, moduleRepository, rmRespository); // TODO: Initialize to an appropriate value
            //UsersEntity model = new UsersEntity();

            //model.CreatedBy = 0;
            //model.CreatedOn = DateTime.Now;
            //model.ID = 0;
            //model.ModifiedBy = 0;
            //model.ModifiedOn = DateTime.Now;

            //model.UserID = 0;
            //model.ID = 0;
            //model.CompanyName = "test";
            //model.CompanyID = 0;
            //model.RoleID = 0;
            //model.Role = RolesEnum.Roles.CLIENT;
            //model.FirstName = "test";
            //model.LastName = "test";
            //model.UserName = "test";
            //model.Email = "Sunnet@sunnet.us";
            //model.PassWord = "test";
            //model.Title = "test";
            //model.Phone = "test";
            //model.EmergencyContactFirstName = "test";
            //model.EmergencyContactLastName = "test";
            //model.EmergencyContactPhone = "test";
            //model.EmergencyContactEmail = "Sunnet@sunnet.us";
            //model.MaintenancePlanOption = "test";
            //model.CreatedOn = DateTime.Now;
            //model.AccountStatus = 0;
            //model.ForgotPassword = 0;
            //model.IsDelete = false;
            //model.Status = "test";
            //model.UserType = "test";
            //model.Skype = "test";
            //model.Office = "US";

            //string to = "Pm1,Pm2,Admin";
            //int expected = 1; // TODO: Initialize to an appropriate value
            //int actual;
            //actual = target.AddUser(model);
            //Assert.AreEqual(expected, actual);
            //Assert.AreEqual(to, emailSender.SendTo);
        }
        /// <summary>
        ///A test for AddUser
        ///</summary>
        //[TestMethod()]
        [Test]
        public void AddUserTest_Add_A_Sunnet()
        {
            //int userID = 0; // TODO: Initialize to an appropriate value
            //EmailSenderFake emailSender = new EmailSenderFake(); // TODO: Initialize to an appropriate value
            //ICache<UserManager> cache = null; // TODO: Initialize to an appropriate value
            //IUsersRepository userRepository = new UserRepositoryFake(); // TODO: Initialize to an appropriate value
            //IRolesRepository roleRepository = null; // TODO: Initialize to an appropriate value
            //IModulesRepository moduleRepository = null; // TODO: Initialize to an appropriate value
            //IRoleModulesRepository rmRespository = null; // TODO: Initialize to an appropriate value
            //UserManager target = new UserManager(userID, emailSender, cache, userRepository, roleRepository, moduleRepository, rmRespository); // TODO: Initialize to an appropriate value
            //UsersEntity model = new UsersEntity();

            //model.CreatedBy = 0;
            //model.CreatedOn = DateTime.Now;
            //model.ID = 0;
            //model.ModifiedBy = 0;
            //model.ModifiedOn = DateTime.Now;

            //model.UserID = 0;
            //model.ID = 0;
            //model.CompanyName = "test";
            //model.CompanyID = 0;
            //model.RoleID = 0;
            //model.Role = RolesEnum.Roles.PM;
            //model.FirstName = "test";
            //model.LastName = "test";
            //model.UserName = "test";
            //model.Email = "Sunnet@sunnet.us";
            //model.PassWord = "test";
            //model.Title = "test";
            //model.Phone = "test";
            //model.EmergencyContactFirstName = "test";
            //model.EmergencyContactLastName = "test";
            //model.EmergencyContactPhone = "test";
            //model.EmergencyContactEmail = "Sunnet@sunnet.us";
            //model.MaintenancePlanOption = "test";
            //model.CreatedOn = DateTime.Now;
            //model.AccountStatus = 0;
            //model.ForgotPassword = 0;
            //model.IsDelete = false;
            //model.Status = "test";
            //model.UserType = "test";
            //model.Skype = "test";
            //model.Office = "US";

            //string to = "Pm1,Pm2,Admin,Lee,Sam,Susan";
            //int expected = 1; // TODO: Initialize to an appropriate value
            //int actual;
            //actual = target.AddUser(model);
            //Assert.AreEqual(expected, actual);
            //Assert.AreEqual(to, emailSender.SendTo);
        }
    }
}
