using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Controls;
using Пр12;
using Пр12.Pages;

namespace AuthTest
{
    [TestClass]
    public class UnitTest1
    {
        AccountPage page = new AccountPage();

        [TestMethod]
        public void AuthTestSuccess()
        {
            Assert.IsTrue(page.Auth("admin", "12345"));
            Assert.IsTrue(page.Auth("maria", "niconiconiii"));
        }

        [TestMethod]
        public void AuthTestFail()
        {
            Assert.IsFalse(page.Auth("", ""));
            Assert.IsFalse(page.Auth("admin", ""));
            Assert.IsFalse(page.Auth("", "admin"));
            Assert.IsFalse(page.Auth("adm", "12345"));
            Assert.IsFalse(page.Auth("admin", "123"));
        }
    }
}
