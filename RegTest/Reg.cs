using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Controls;
using Пр12;
using Пр12.Pages;

namespace RegTest
{
    [TestClass]
    public class UnitTest2
    {
        Reg page = new Reg();

        [TestMethod]
        public void RegTestSuccess()
        {
            Assert.IsTrue(page.Regist("", ""));
        }

        [TestMethod]
        public void RegTestFail()
        {
            Assert.IsFalse(page.Regist("", ""));
            Assert.IsFalse(page.Regist("abc", ""));
            Assert.IsFalse(page.Regist("", "123"));
            Assert.IsFalse(page.Regist("abc", "123"));
        }
    }
}
