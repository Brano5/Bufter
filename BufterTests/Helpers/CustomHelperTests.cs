using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bufter.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bufter.Models;

namespace Bufter.Helpers.Tests
{
    [TestClass()]
    public class CustomHelperTests
    {
        [TestMethod()]
        public void HashPasswordTest()
        {
            //Arrange
            string password = "pass";
            string salt = "salt";

            //Act
            var pass1 = CustomHelper.HashPassword(password, salt);
            var pass2 = CustomHelper.HashPassword(password, salt);

            //Assert
            Assert.AreEqual(pass1, pass2);
        }

        [TestMethod()]
        public void HashPasswordTestEmpty()
        {
            //Act
            string result = CustomHelper.HashPassword("", "");

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void CreateSaltTest()
        {
            //Act
            string result = CustomHelper.CreateSalt();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Length > 10);
        }
    }
}