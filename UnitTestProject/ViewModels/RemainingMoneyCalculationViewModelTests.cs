using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPF.ViewModels.Tests
{
    [TestClass()]
    public class RemainingMoneyCalculationViewModelTests
    {
        //[TestMethod()]
        //public void RemainingMoneyCalculationViewModelTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void RemainingMoneyCalculationViewModelTest1()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void OutputTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod]
        public void シナリオ()
        {
            var vm = new RemainingMoneyCalculationViewModel
            {
                OneYenCount = 10,
                FiveYenCount = 10,
                TenYenCount = 10,
                FiftyYenCount = 10,
                OneHundredYenCount = 10,
                FiveHundredYenCount = 10,
                OneThousandYenCount = 10,
                FiveThousandYenCount = 10,
                TenThousandYenCount = 10,
                OneYenBundleCount = 10,
                FiveYenBundleCount = 10,
                TenYenBundleCount = 10,
                FiftyYenBundleCount = 10,
                OneHundredYenBundleCount = 10,
                FiveHundredYenBundleCount = 10,
                OtherMoneyAmountDisplayValue1 = "10",
                OtherMoneyAmountDisplayValue2 = "10",
                OtherMoneyAmountDisplayValue3 = "10",
                OtherMoneyAmountDisplayValue4 = "10",
                OtherMoneyAmountDisplayValue5 = "10",
                OtherMoneyAmountDisplayValue6 = "10",
                OtherMoneyAmountDisplayValue7 = "10",
                OtherMoneyAmountDisplayValue8 = "10",
            };

            Assert.AreEqual(vm.OneYenAmountWithUnit, "10 円");
            Assert.AreEqual(vm.FiveYenAmountWithUnit, "50 円");
            Assert.AreEqual(vm.TenYenAmountWithUnit, "100 円");
            Assert.AreEqual(vm.FiftyYenAmountWithUnit, "500 円");
            Assert.AreEqual(vm.OneHundredYenAmountWithUnit, "1,000 円");
            Assert.AreEqual(vm.FiveHundredYenAmountWithUnit, "5,000 円");
            Assert.AreEqual(vm.OneThousandYenAmountWithUnit, "10,000 円");
            Assert.AreEqual(vm.FiveThousandYenAmountWithUnit, "50,000 円");
            Assert.AreEqual(vm.TenThousandYenAmountWithUnit, "100,000 円");
            Assert.AreEqual(vm.OneYenBundleAmountWithUnit, "500 円");
            Assert.AreEqual(vm.FiveYenBundleAmountWithUnit, "2,500 円");
            Assert.AreEqual(vm.TenYenBundleAmountWithUnit, "5,000 円");
            Assert.AreEqual(vm.FiftyYenBundleAmountWithUnit, "25,000 円");
            Assert.AreEqual(vm.OneHundredYenBundleAmountWithUnit, "50,000 円");
            Assert.AreEqual(vm.FiveHundredYenBundleAmountWithUnit, "250,000 円");

            Assert.AreEqual(vm.TotalAmount, "499,740 円");
        }

        [TestMethod]
        public void その他金庫の金額欄が数字かどうか()
        {
            var vm = new RemainingMoneyCalculationViewModel
            {
                OtherMoneyAmountDisplayValue1 = "a",
                OtherMoneyAmountDisplayValue2 = "a",
                OtherMoneyAmountDisplayValue3 = "a",
                OtherMoneyAmountDisplayValue4 = "a",
                OtherMoneyAmountDisplayValue5 = "a",
                OtherMoneyAmountDisplayValue6 = "a",
                OtherMoneyAmountDisplayValue7 = "a",
                OtherMoneyAmountDisplayValue8 = "a",
            };

            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue1, string.Empty);
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue2, string.Empty);
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue3, string.Empty);
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue4, string.Empty);
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue5, string.Empty);
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue6, string.Empty);
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue7, string.Empty);
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue8, string.Empty);

            vm.OtherMoneyAmountDisplayValue1 = "1000";
            vm.OtherMoneyAmountDisplayValue2 = "1000";
            vm.OtherMoneyAmountDisplayValue3 = "1000";
            vm.OtherMoneyAmountDisplayValue4 = "1000";
            vm.OtherMoneyAmountDisplayValue5 = "1000";
            vm.OtherMoneyAmountDisplayValue6 = "1000";
            vm.OtherMoneyAmountDisplayValue7 = "1000";
            vm.OtherMoneyAmountDisplayValue8 = "1000";

            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue1, "1,000");
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue2, "1,000");
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue3, "1,000");
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue4, "1,000");
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue5, "1,000");
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue6, "1,000");
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue7, "1,000");
            Assert.AreEqual(vm.OtherMoneyAmountDisplayValue8, "1,000");
        }

    }
}