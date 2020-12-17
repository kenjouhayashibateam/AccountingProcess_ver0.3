using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities.ValueObjects.Tests
{
    [TestClass()]
    public class MoneyCategoryTests
    {
        //[TestMethod()]
        //public void MoneyCategoryTest()
        //{
        //    Assert.Fail();
        //}
        [TestMethod()]
        public void シナリオ()
        {
            MoneyCategory money = new MoneyCategory(100) { Count = 10 };

            Assert.AreEqual(money.Amount, 1000);
        }
    }
}