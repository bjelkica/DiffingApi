using DiffingApi.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiffUnitTests
{
    [TestClass]
    public class Base64Test
    {
        [TestMethod]
        public void DecodeBase64 ()
        {
            //Testing Data
            string encoded1 = "AAAAAA==";
            string encoded2 = "AAA=";
            string encoded3 = "AQABAQ==";

            //Expected Result
            string expected1 = "0000";
            string expected2 = "00";
            string expected3 = "1011"; 

            //Actual Result
            string actual1 = Base64String.DecodeBase64String(encoded1);
            string actual2 = Base64String.DecodeBase64String(encoded2);
            string actual3 = Base64String.DecodeBase64String(encoded3);

            //Assert
            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
            Assert.AreEqual(expected3, actual3);
        }
    }
}
