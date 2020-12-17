using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.ValueObjects;

namespace WPF.ViewModels.Tests
{
    [TestClass()]
    public class LoginViewModelTests
    {
        
        readonly LoginViewModel vm=new LoginViewModel();
        [TestMethod()]
        public void シナリオ()
        {
            vm.SetRep(new Rep("aaa", "bbb", "ccc", true,true));
            Assert.AreEqual(vm.WindowTitle, "担当者ログイン（ログイン : bbb）");
        }
    }
}