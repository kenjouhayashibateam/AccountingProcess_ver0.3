using Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WPF.ViewModels.Tests
{
    [TestClass()]
    public class ReceiptsAndExpenditureMangementViewModelTests
    {
        private readonly ReceiptsAndExpenditureMangementViewModel vm = new ReceiptsAndExpenditureMangementViewModel() { DataBaseConnect = new LocalConnectInfrastructure() };

        [TestMethod()]
        public void データ登録時のフィールドプロパティ()
        {
            Assert.AreEqual(vm.IsValidity, true);
            Assert.AreEqual(vm.ComboContentText, string.Empty);
            Assert.AreEqual(vm.ComboAccountingSubjectText, string.Empty);
            Assert.AreEqual(vm.ComboAccountingSubjectCode, string.Empty);
            Assert.AreEqual(vm.ComboCreditAccountText, "春秋苑");
            Assert.AreEqual(vm.DetailText, string.Empty);
            Assert.AreEqual(vm.Price, string.Empty);
            Assert.AreEqual(vm.AccountActivityDate, DateTime.Today);
            Assert.AreEqual(vm.IsDataOperationButtonEnabled, false);

            vm.ComboContentText = vm.ComboContents[0].Text;
            Assert.AreEqual(vm.IsDataOperationButtonEnabled, false);

            vm.ComboAccountingSubjectText = vm.ComboAccountingSubjects[0].Subject;
            Assert.AreEqual(vm.IsDataOperationButtonEnabled, false);

            vm.ComboAccountingSubjectCode = vm.ComboAccountingSubjects[0].SubjectCode;
            Assert.AreEqual(vm.IsDataOperationButtonEnabled, false);

            vm.Price = "1000";
            Assert.AreEqual(vm.IsDataOperationButtonEnabled, true);            
        }
    }
}