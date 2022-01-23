using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Entities.ValueObjects;
using Infrastructure;
using Domain.Entities;
using WPF.ViewModels;

namespace WPF.ViewModels.Tests
{
    [TestClass()]
    public class DataManagementViewModelTests
    {
        readonly DataManagementViewModel vm = new DataManagementViewModel
            (new LocalConnectInfrastructure()) ;

        [TestMethod()]
        public void 担当者の登録時フィールドプロパティ()
        {
            LoginRep.GetInstance().SetRep(new Rep("aaa", "aaa", "aaa", true, true));

            vm.IsCheckedRegistration = true;
            vm.DataBaseConnect = new LocalConnectInfrastructure();
            LoginRep.GetInstance().SetRep(new Rep("aaa", "aaa", "aaa", true, true));
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
            Assert.AreEqual(vm.IsRepOperationButtonEnabled, false);

            vm.CurrentRep = vm.RepList[0];

            Assert.AreEqual(vm.RepName, "林飛 顕誠");
            Assert.AreEqual(vm.RepCurrentPassword, string.Empty);
            Assert.AreEqual(vm.RepNewPassword, string.Empty);
            Assert.AreEqual(vm.ConfirmationPassword, string.Empty);
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

            vm.AccountingSubjectCodeField = "aaa";

            Assert.AreEqual(vm.IsAccountingSubjectOperationButtonEnabled, false);

        }
        [TestMethod()]
        public void 勘定科目の更新時のフィールドプロパティ()
        {
            LoginRep.GetInstance().SetRep(new Rep("aaa", "aaa", "aaa", true, true));
            vm.SetDataUpdateCommand.Execute();

            Assert.AreEqual(vm.IsAccountingSubjectReferenceMenuEnabled, true);
            Assert.AreEqual(vm.IsAccountingSubjectOperationButtonEnabled, false);

            vm.CurrentAccountingSubject = vm.AccountingSubjects[0];

            Assert.AreEqual(vm.IsAccountingSubjectOperationButtonEnabled, true);

            Assert.AreEqual(vm.IsAccountingSubjectCodeFieldEnabled, false);
            Assert.AreEqual(vm.IsAccountingSubjectFieldEnabled, false);
        }
        [TestMethod()]
        public void 貸方勘定の登録時のフィールドプロパティ()
        {
            vm.SetDataRegistrationCommand.Execute();

            Assert.AreEqual(vm.IsCreditDeptOperationButtonEnabled, false);
            Assert.AreEqual(vm.CreditDeptField, string.Empty);
            Assert.AreEqual(vm.IsCreditDeptEnabled, true);

            vm.CreditDeptField = "aaa";

            Assert.AreEqual(vm.IsCreditDeptOperationButtonEnabled, true);
        }
        [TestMethod()]
        public void 貸方勘定の更新時のフィールドプロパティ()
        {
            vm.SetDataUpdateCommand.Execute();

            Assert.AreEqual(vm.IsCreditDeptReferenceMenuEnabled, true);
            Assert.AreEqual(vm.IsCreditDeptOperationButtonEnabled, false);

            vm.CurrentCreditDept = vm.CreditDepts[0];

            Assert.AreEqual(vm.IsCreditDeptEnabled, false);
            Assert.AreEqual(vm.IsCreditDeptOperationButtonEnabled, true);
        }
        [TestMethod()]
        public void 伝票内容の登録時のフィールドプロパティ()
        {
            vm.SetDataRegistrationCommand.Execute();

            Assert.AreEqual(vm.IsContentOperationEnabled, false);
            Assert.AreEqual(vm.IsAffiliationAccountingSubjectEnabled, true);
            Assert.AreEqual(vm.IsContentFieldEnabled, true);

            vm.AffiliationAccountingSubject = vm.AffiliationAccountingSubjects[0];

            Assert.AreEqual(vm.IsContentOperationEnabled, false);

            vm.ContentField = "aaa";

            Assert.AreEqual(vm.IsContentOperationEnabled, true);
        }
        [TestMethod()]
        public void 伝票内容の更新時のフィールドプロパティ()
        {
            vm.SetDataUpdateCommand.Execute();

            Assert.AreEqual(vm.IsContentOperationEnabled, false);
            Assert.AreEqual(vm.IsAffiliationAccountingSubjectEnabled, false);
            Assert.AreEqual(vm.IsContentFieldEnabled, false);

            vm.CurrentContent = vm.Contents[0];

            Assert.AreEqual(vm.IsContentOperationEnabled, true);
        }
        private void SetAdminPermissionRep()
        {
            LoginRep rep = LoginRep.GetInstance();
            rep.SetRep(new Rep("aaa", "bbb", "ccc", true, true));
        }
    }
}