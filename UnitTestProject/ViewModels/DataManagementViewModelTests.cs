﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities.ValueObjects;
using WPF.Views.Datas;
using Infrastructure;

namespace WPF.ViewModels.Tests
{
    [TestClass()]
    public class DataManagementViewModelTests
    {
        readonly DataManagementViewModel vm = new DataManagementViewModel(new LocalConectInfrastructure());
        [TestMethod()]
        public void シナリオ()
        {
        }
        [TestMethod()]
        public void 担当者の登録時フィールドプロパティ()
        {
            vm.IsCheckedRegistration = true;

            Assert.AreEqual(vm.IsRepNameDataEnabled, true);
            Assert.AreEqual(vm.IsRepPasswordEnabled, false);
            Assert.AreEqual(vm.IsRepNewPasswordEnabled, true);
            Assert.AreEqual(vm.IsRepReferenceMenuEnabled, false);
            Assert.AreEqual(vm.RepName, string.Empty);
            Assert.AreEqual(vm.RepCurrentPassword, string.Empty);
            Assert.AreEqual(vm.RepNewPassword, string.Empty);
            Assert.AreEqual(vm.RepDataOperationButtonContent, "登録");
            Assert.AreEqual(vm.IsRepOperationButtonEnabled, false);
        }
        [TestMethod()]
        public void 担当者の更新時フィールドプロパティ()
        {
            vm.SetDataUpdateCommand.Execute();
            SetAdminPermissionRep();

            Assert.AreEqual(vm.IsRepNameDataEnabled, false);
            Assert.AreEqual(vm.IsRepPasswordEnabled, true);
            Assert.AreEqual(vm.IsRepNewPasswordEnabled, false);
            Assert.AreEqual(vm.IsRepReferenceMenuEnabled, true);

            vm.CurrentRep = vm.RepList[0];

            Assert.AreEqual(vm.RepName, "林飛 顕誠");
            Assert.AreEqual(vm.RepCurrentPassword, string.Empty);
            Assert.AreEqual(vm.RepNewPassword, string.Empty);
            Assert.AreEqual(vm.RepDataOperationButtonContent, "更新");

            vm.RepCurrentPassword = "bbb";

            Assert.AreEqual(vm.IsRepOperationButtonEnabled, false);
            Assert.AreEqual(vm.IsRepNewPasswordEnabled, false);

            vm.RepCurrentPassword = "aaa";
            Assert.AreEqual(vm.IsRepNewPasswordEnabled, true);
            Assert.AreEqual(vm.IsRepOperationButtonEnabled, true);

            vm.RepNewPassword = "bbb";
            Assert.AreEqual(vm.IsRepOperationButtonEnabled, false);

            vm.ConfirmationPassword = "bb";
            Assert.AreEqual(vm.IsRepOperationButtonEnabled, false);
            Assert.AreEqual(vm.IsRepOperationButtonEnabled, false);

            vm.ConfirmationPassword = "bbb";
            Assert.AreEqual(vm.IsRepOperationButtonEnabled, true);
        }
        [TestMethod()]
        public void 勘定科目の登録時のフィールドプロパティ()
        {
            vm.SetDataRegistrationCommand.Execute();
            SetAdminPermissionRep();

            vm.AccountingSubjectCodeField = string.Empty;
            vm.AccountingSubjectField = string.Empty;

            Assert.AreEqual(vm.IsAccountingSubjectOperationButtonEnabled, false);

            vm.AccountingSubjectField = "aaaa";
            vm.AccountingSubjectCodeField = "111";

            Assert.AreEqual(vm.IsAccountingSubjectOperationButtonEnabled, true);

            vm.AccountingSubjectCodeField = "11";

            Assert.AreEqual(vm.IsAccountingSubjectOperationButtonEnabled, false);

            vm.AccountingSubjectCodeField = "aaa";

            Assert.AreEqual(vm.IsAccountingSubjectOperationButtonEnabled, false);

        }

        private void SetAdminPermissionRep()
        {
            LoginRep rep = LoginRep.GetInstance();
            rep.SetRep(new Rep("aaa", "bbb", "ccc", true, true));
        }
    }
}