﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ValueObjects.Tests
{
    [TestClass()]
    public class OtherMoneyTests
    {
        [TestMethod()]
        public void シナリオ()
        {
            OtherMoney otherMoney = new OtherMoney() { Amount = 1000 };
            Assert.AreEqual(otherMoney.AmountWithUnit(), "1,000 円");
        }
    }
}