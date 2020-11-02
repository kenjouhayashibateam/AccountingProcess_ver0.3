using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities.ValueObjects;

namespace WPF.ViewModels.Tests
{
    [TestClass()]
    public class DataManagementViewModelTests
    {
        readonly DataManagementViewModel vm = new DataManagementViewModel();
        [TestMethod()]
        public void シナリオ()
        {

        }
        [TestMethod()]
        public void 登録時の担当者フィールドプロパティ()
        {
            vm.IsCheckedRegistration = true;

            Assert.AreEqual(vm.IsRepNameDataEnabled, true);
            Assert.AreEqual(vm.IsRepPasswordEnabled, false);
            Assert.AreEqual(vm.IsRepNewPasswordEnabled, true);
            Assert.AreEqual(vm.IsRepReferenceMenuEnabled, false);
            Assert.AreEqual(vm.RepName, null);
            Assert.AreEqual(vm.RepCurrentPassword, string.Empty);
            Assert.AreEqual(vm.RepNewPassword, string.Empty);
            Assert.AreEqual(vm.RepDataOperationButtonContent, "登録");
            Assert.AreEqual(vm.IsRepOperationButtonEnabled, false);
        }
        [TestMethod()]
        public void 更新時の担当者フィールドプロパティ()
        {
            vm.SetDataUpdateCommand.Execute();

            Assert.AreEqual(vm.IsRepNameDataEnabled, false);
            Assert.AreEqual(vm.IsRepPasswordEnabled, true);
            Assert.AreEqual(vm.IsRepNewPasswordEnabled, false);
            Assert.AreEqual(vm.IsRepReferenceMenuEnabled, true);

            vm.CurrentRep = new Rep("rep1", "a a", "aaa", false);

            Assert.AreEqual(vm.RepName, "a a");
            Assert.AreEqual(vm.RepCurrentPassword, string.Empty);
            Assert.AreEqual(vm.RepNewPassword, string.Empty);
            Assert.AreEqual(vm.RepDataOperationButtonContent, "更新");

            vm.RepCurrentPassword = "bbb";

            Assert.AreEqual(vm.IsRepOperationButtonEnabled, false);
            Assert.AreEqual(vm.IsRepNewPasswordEnabled, false);

            vm.RepCurrentPassword = "aaa";
            Assert.AreEqual(vm.IsRepNewPasswordEnabled, true);

            vm.RepNewPassword = "bbb";
            Assert.AreEqual(vm.IsRepOperationButtonEnabled, false);

            vm.ConfirmationPassword = "bb";
            Assert.AreEqual(vm.IsRepOperationButtonEnabled, false);
            Assert.AreEqual(vm.IsRepOperationButtonEnabled, false);

            vm.ConfirmationPassword = "bbb";
            Assert.AreEqual(vm.IsRepOperationButtonEnabled, true);
        }
    }
}