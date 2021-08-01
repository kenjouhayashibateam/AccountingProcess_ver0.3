using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    public class ReceiptsAndExpenditureRegistrationHelperViewModel : BaseViewModel
    {
        private readonly ReceiptsAndExpenditureOperation OperationData;
        private ObservableCollection<Content> contents;
        private Content selectedContent;
        private string searchContentText = string.Empty;
        private string searchAccountingSubject = string.Empty;
        private bool windowCloseSwich;

        public ReceiptsAndExpenditureRegistrationHelperViewModel
            (IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            OperationData = ReceiptsAndExpenditureOperation.GetInstance();

            CreditDept cd = new CreditDept(string.Empty, string.Empty, true, true);
            AccountingSubject subject = new AccountingSubject(string.Empty, string.Empty, string.Empty, true);
            Content c = new Content(string.Empty, subject, -1, string.Empty, true);

            OperationData.SetData
                (new ReceiptsAndExpenditure
                    (0, DateTime.Today, LoginRep.GetInstance().Rep, AccountingProcessLocation.Location, cd, c, 
                        string.Empty, 0, true, true, DateTime.Today, DefaultDate, false));

            InputContentCommand = new DelegateCommand(() => InputContent(), () => true);
        }
        public ReceiptsAndExpenditureRegistrationHelperViewModel() : 
            this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// OperationDataを更新してウィンドウを閉じるコマンド
        /// </summary>
        public DelegateCommand InputContentCommand { get; }
        private async void InputContent()
        {
            await Task.Delay(1);

            OperationData.Data.Content = SelectedContent;
            WindowCloseSwich = true;
        }
        /// <summary>
        /// 検索する伝票内容
        /// </summary>
        public string SearchContentText
        {
            get => searchContentText;
            set
            {
                searchContentText = value;
                if (!string.IsNullOrEmpty(value)) { SetList(); }
                CallPropertyChanged();
            }
        }
        private void SetList()
        {
            Contents = DataBaseConnect.ReferenceContent
                    (SearchContentText, string.Empty, SearchAccountingSubject, true);
        }
        /// <summary>
        /// 伝票内容リスト
        /// </summary>
        public ObservableCollection<Content> Contents
        {
            get => contents;
            set
            {
                contents = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された伝票内容
        /// </summary>
        public Content SelectedContent
        {
            get => selectedContent;
            set
            {
                selectedContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ウィンドウを閉じるスウィッチ
        /// </summary>
        public bool WindowCloseSwich
        {
            get => windowCloseSwich;
            set
            {
                windowCloseSwich = value;
                CallPropertyChanged();
                windowCloseSwich = false;
            }
        }
        /// <summary>
        /// 検索する勘定科目
        /// </summary>
        public string SearchAccountingSubject
        {
            get => searchAccountingSubject;
            set
            {
                searchAccountingSubject = value;
                if (!string.IsNullOrEmpty(value)) { SetList(); }
                CallPropertyChanged();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        { }

        protected override void SetWindowDefaultTitle()
        {
            DefaultWindowTitle = $"伝票内容逆引き入力{Space}:{AccountingProcessLocation.Location}";
        }
    }
}
