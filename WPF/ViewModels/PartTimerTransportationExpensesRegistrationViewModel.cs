using Domain.Entities;
using Domain.Entities.Helpers;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// パート交通費データ登録画面ViewModel
    /// </summary>
    public class PartTimerTransportationExpensesRegistrationViewModel : BaseViewModel
    {
        private ObservableCollection<PartData> dataList = new ObservableCollection<PartData>();
        private string listTitle = TITLE;
        private const string TITLE = "パート交通費リスト";
        private PartData selectedPartData;
        private bool isRegistrationEnabled;

        public PartTimerTransportationExpensesRegistrationViewModel(IDataBaseConnect dataBaseConnect) : 
            base(dataBaseConnect)
        {
            DataPasteCommand = new DelegateCommand(() => DataPaste(), () => true);
            RegistrationCommand = new DelegateCommand(() => Registration(), () => true);
        }
        public PartTimerTransportationExpensesRegistrationViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// 出納データ登録コマンド
        /// </summary>
        public DelegateCommand RegistrationCommand { get; }
        private void Registration()
        {
            int registerCount = default;
            string countInfo;
            LoginRep loginRep = LoginRep.GetInstance();
            ReceiptsAndExpenditure templateReceiptsAndExpenditure =
                new ReceiptsAndExpenditure(0, DateTime.Today, loginRep.Rep, AccountingProcessLocation.Location,
                    DataBaseConnect.CallCreditDept("credit_dept0"), DataBaseConnect.CallContent("content1"),
                    string.Empty, 0, false, true, DateTime.Today, DefaultDate, false);
            //pdListにPartDataを格納して、登録する件数を計算する
            List<PartData> pdList = new List<PartData>();
            foreach (PartData pd in DataList)
            {
                pdList.Add(pd);
                if (!pd.IsExclusion) registerCount++;
            }
            //登録件数を文字列に格納する
            if (registerCount == DataList.Count) countInfo = $"{DataList.Count}{Space}件";
            else countInfo = $"{registerCount}/{DataList.Count}{Space}件";
            //登録件数が0ならメソッドを終了する
            if(registerCount==0)
            {
                NoRegistrationInfo();
                return;
            }
            //登録確認
            if (ConfirmationRegistration(countInfo) == MessageBoxResult.Cancel) return;
            //出納データを登録してリストから削除する
            ReceiptsAndExpenditure rae;
            foreach(PartData pd in pdList)
            {
                if (pd.IsExclusion) continue;

                rae = templateReceiptsAndExpenditure;
                rae.Detail = pd.Name;
                rae.Price = pd.TransportationExpenses;
                DataBaseConnect.Registration(rae);
                DataList.Remove(pd);
            }
            SetListTitle();
            ValidationProperty(nameof(DataList), DataList);
            SetIsRegistrationEnabled();
            CompleteRegistration();
        }
        private void CompleteRegistration()
        {
            MessageBox = new MessageBoxInfo()
            {
                Title = "登録完了",
                Message = "リストのアイテムを出納データに登録しました。出納管理を確認してください。",
                Image = MessageBoxImage.Information,
                Button = MessageBoxButton.OK
            };
        }
        private void NoRegistrationInfo()
        {
            MessageBox = new MessageBoxInfo()
            {
                Title = "登録データがありません",
                Message = "すべてのアイテムに除外チェックがついていますので、登録せずに処理を終了します。",
                Image = MessageBoxImage.Information,
                Button = MessageBoxButton.OK
            };
        }
        private MessageBoxResult ConfirmationRegistration(string countInfo)
        {
            MessageBox = new MessageBoxInfo()
            {
                Message = $"{countInfo}{Space}を登録します。よろしいですか？",
                Title = "登録確認",
                Button = MessageBoxButton.OKCancel,
                Image = MessageBoxImage.Question
            };
            return MessageBox.Result;
        }
        /// <summary>
        /// 交通費データをリストに貼り付けるコマンド
        /// </summary>
        public DelegateCommand DataPasteCommand { get; }
        private void DataPaste()
        {
            string[] PasteDataArray = Clipboard.GetText().Replace("\r\n", "\n").Split(new[] { '\n', '\r' });
            string[] subArray;
            PartData partData;

            DataList = new ObservableCollection<PartData>();
            for(int i =0; i<PasteDataArray.Length;i++)
            {
                subArray = PasteDataArray[i].Split('\t');
                if(!CopyDataErrorCheck(subArray))
                {
                    WarningCopyDataMessage();
                    return;
                }
                partData = new PartData(subArray[0], int.Parse(subArray[1]));
                DataList.Add(partData);
            }
            SetListTitle();
            ValidationProperty(nameof(DataList), DataList);
          
            SetIsRegistrationEnabled();
        }
        private void SetListTitle() =>
            ListTitle = DataList.Count == 0 ? TITLE : $"{TITLE}{Space}:{Space}{DataList.Count}{Space}件";

        private bool CopyDataErrorCheck(string[] data)
        {
            int i;
            bool b;
            b = data.Length == 2;
            if (b)
            {
                i = int.TryParse(data[1], out int j) ? j : 0;
                b = i > 0;
            }

            return b;
        }

        private void WarningCopyDataMessage()
        {
            MessageBox = new MessageBoxInfo()
            {
                Message = "コピーデータの長さが正しくありません。",
                Image = MessageBoxImage.Warning,
                Title = "コピーエラー",
                Button = MessageBoxButton.OK
            };
        }
        /// <summary>
        /// リスト表示するパートデータ
        /// </summary>
        public ObservableCollection<PartData> DataList
        {
            get => dataList;
            set
            {
                dataList = value;
                ValidationProperty(nameof(DataList), value);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// パートデータリストのタイトル
        /// </summary>
        public string ListTitle
        {
            get => listTitle;
            set
            {
                listTitle = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択されたパートデータ
        /// </summary>
        public PartData SelectedPartData
        {
            get => selectedPartData;
            set
            {
                selectedPartData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録ボタンEnabled
        /// </summary>
        public bool IsRegistrationEnabled
        {
            get => isRegistrationEnabled;
            set
            {
                isRegistrationEnabled = value;
                CallPropertyChanged();
            }
        }

        private void SetIsRegistrationEnabled() => IsRegistrationEnabled = !HasErrors & DataList.Count > 0;

        public override void SetRep(Rep rep)
        {
            {
                if (rep == null || string.IsNullOrEmpty(rep.Name)) WindowTitle = DefaultWindowTitle;
                else
                {
                    IsAdminPermisson = rep.IsAdminPermisson;
                    WindowTitle =
                        $"{DefaultWindowTitle}（ログイン : {TextHelper.GetFirstName(rep.Name)}）";
                }
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            switch(propertyName)
            {
                case nameof(DataList):
                    ErrorsListOperation(DataList.Count == 0, propertyName, "アイテムがありません");
                    break;
            }
        }

        protected override void SetWindowDefaultTitle() => DefaultWindowTitle = "パート交通費データ登録";

        /// <summary>
        /// パートデータクラス
        /// </summary>
        public class PartData
        {
            /// <summary>
            /// 氏名
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 交通費
            /// </summary>
            public int TransportationExpenses { get; set; }
            /// <summary>
            /// 表示用交通費
            /// </summary>
            public string TransportationExpensesDisplayValue 
            { get => TextHelper.AmountWithUnit(TransportationExpenses); }
            /// <summary>
            /// 登録除外チェック
            /// </summary>
            public bool IsExclusion { get; set; }

            public PartData(string name, int transportationExpenses)
            {
                Name = name;
                TransportationExpenses = transportationExpenses;
            }
        }
    }
}
