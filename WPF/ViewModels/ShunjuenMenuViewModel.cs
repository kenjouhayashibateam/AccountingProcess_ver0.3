using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;

namespace WPF.ViewModels
{
    /// <summary>
    /// 春秋苑データ操作メニュー画面
    /// </summary>
    public class ShunjuenMenuViewModel : BaseViewModel
    {
        public ShunjuenMenuViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            ShowCreateCondolencesCommand = new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.CreateCondolences()), () => true);
            ShowProductSalesRegistrationViewCommand=new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.ProductSalesRegistration()), () => true);
            ShowPartTimerTransPortCommand = new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.PartTimerTransportRegistration()), () => true);
            ShowMangementFeeRegistrationCommand = new DelegateCommand
                (() => CreateShowWindowCommand(ScreenTransition.ManagementFeePaymentRegistration()), () => true);
        }
        public ShunjuenMenuViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        public DelegateCommand ShowMangementFeeRegistrationCommand { get; }
        /// <summary>
        /// パート交通費登録画面表示コマンド
        /// </summary>
        public DelegateCommand ShowPartTimerTransPortCommand { get; }
        /// <summary>
        /// 物販登録画面表示コマンド
        /// </summary>
        public DelegateCommand ShowProductSalesRegistrationViewCommand { get; }
        /// <summary>
        /// 御布施一覧登録画面表示コマンド
        /// </summary>
        public DelegateCommand ShowCreateCondolencesCommand { get; }

        public override void ValidationProperty(string propertyName, object value)
        {}

        protected override void SetWindowDefaultTitle() { DefaultWindowTitle = "春秋苑経理処理メニュー"; }
    }
}
