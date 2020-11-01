using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Helpers.Tests
{
    [TestClass()]
    public class AmountHelperTests
    {
        [TestMethod()]
        public void シナリオ()
        {
            Assert.AreEqual(AmountHelper.AmountWithUnit(1000),"1,000 円");
            Assert.AreEqual(AmountHelper.CommaDelimitedAmount(1000), "1,000");
        }
    }
}