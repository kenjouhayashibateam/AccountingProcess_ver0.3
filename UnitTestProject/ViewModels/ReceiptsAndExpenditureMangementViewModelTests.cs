using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WPF.ViewModels.Tests
{
    [TestClass()]
    public class ReceiptsAndExpenditureMangementViewModelTests
    {
        private readonly ReceiptsAndExpenditureMangementViewModel vm = new ReceiptsAndExpenditureMangementViewModel();

        [TestMethod()]
        public void データ登録時のフィールドプロパティ()
        {
            vm.SetDataRegistrationCommand.Execute();

            Assert.AreEqual(vm.IsValidity, true);
            Assert.AreEqual(vm.ComboContentText, string.Empty);
            Assert.AreEqual(vm.ComboAccountingSubjectText, string.Empty);
            Assert.AreEqual(vm.ComboAccountingSubjectCode, string.Empty);
            Assert.AreEqual(vm.ComboCreditAccountText, "春秋苑");
            Assert.AreEqual(vm.DetailText, string.Empty);
            Assert.AreEqual(vm.Price, string.Empty);
            Assert.AreEqual(vm.AccountActivityDate, DateTime.Today);
            Assert.AreEqual(vm.IsDataOperationButtonEnabled, false);

            
        }
    }
}