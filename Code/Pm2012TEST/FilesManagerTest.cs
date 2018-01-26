using SunNet.PMNew.Core.FileModule;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunNet.PMNew.Entity.FileModel;
using Pm2012TEST.Fakes;

namespace Pm2012TEST
{


    /// <summary>
    ///This is a test class for FilesManagerTest and is intended
    ///to contain all FilesManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class FilesManagerTest
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
        ///A test for CheckValidityOfFile
        ///</summary>
        ///not pass , file type not match
        [TestMethod()]
        public void CheckValidityOfFileTest()
        {
            //int userID = 0; // TODO: Initialize to an appropriate value
            //IFilesRepository Repository = null; // TODO: Initialize to an appropriate value
            //// FilesManager target = new FilesManager(userID, Repository); // TODO: Initialize to an appropriate value
            //FakeFile ff = new FakeFile();

            //FilesEntity fe = ff.Create(1); // TODO: Initialize to an appropriate value
            //bool expected = false; // TODO: Initialize to an appropriate value
            //bool actual;
            //actual = target.CheckValidityOfFile(fe);
            //Assert.AreEqual(expected, actual);
        }
        /// <summary>
        /// pass.file type match
        /// </summary>
        [TestMethod()]
        public void CheckValidityOfFileTest01()
        {
            //int userID = 0; // TODO: Initialize to an appropriate value
            //IFilesRepository Repository = null; // TODO: Initialize to an appropriate value
            ////  FilesManager target = new FilesManager(userID, Repository); // TODO: Initialize to an appropriate value
            //FakeFile ff = new FakeFile();

            //FilesEntity fe = ff.Create(2); // TODO: Initialize to an appropriate value
            //bool expected = true; // TODO: Initialize to an appropriate value
            //bool actual;
            //actual = target.CheckValidityOfFile(fe);
            //Assert.AreEqual(expected, actual);
        }
    }
}
