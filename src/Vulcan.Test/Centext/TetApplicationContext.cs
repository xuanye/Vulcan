using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using Vulcan;
namespace Vulcan.Test.Centext
{
    [TestFixture]
    public class TetApplicationContext
    {

        [Test]
        public void TestSetGetData()
        {
            ApplicationContext.LocalContext.Add("TEST_1","1");

            bool hasValue= ApplicationContext.LocalContext.Contains("TEST_1");

            Assert.True(hasValue);

            string value = ApplicationContext.LocalContext["TEST_1"].ToString();

            Assert.AreEqual("1", value);
        }
    }
}
