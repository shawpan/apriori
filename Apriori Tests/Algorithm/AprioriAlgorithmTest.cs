using Apriori.Algorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Apriori.DAL.Gateway.Interface;
using Apriori.DAL.Gateway;

namespace Apriori_Tests
{
    
    
    /// <summary>
    ///This is a test class for AprioriAlgorithmTest and is intended
    ///to contain all AprioriAlgorithmTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AprioriAlgorithmTest
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
        ///A test for GenerateFrequentItemSets
        ///</summary>
        [TestMethod()]
        public void GenerateFrequentItemSetsTest()
        {
            IInputDatabaseHelper _inDatabaseHelper = new FileInputDatabaseHelper("mushroom"); // TODO: Initialize to an appropriate value
            IOutputDatabaseHelper _outDatabaseHelper = new FileOutputDatabaseHelper(@"D:\Data_Mining_Assignment\AprioriTests\Result\");; // TODO: Initialize to an appropriate value
            AprioriAlgorithm target = new AprioriAlgorithm(_inDatabaseHelper, _outDatabaseHelper,0.5f); // TODO: Initialize to an appropriate value
            int expected = 153; // expected 153 itemsets for mushroom.dat minSup 0.5
            int actual;
            actual = target.GenerateFrequentItemSets();
            if (_inDatabaseHelper.DatabaseName == "mushroom" && target.MinimumSupport == 0.5f)
                expected = 153;  // expected 153 itemsets for mushroom.dat minSup 0.5
            else
                expected = actual;
            
            Assert.AreEqual(expected, actual);
        }
    }
}
