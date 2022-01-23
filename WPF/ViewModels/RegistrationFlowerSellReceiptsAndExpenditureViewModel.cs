using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.TextHelper;
using static Domain.Entities.Helpers.DataHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 花売り出納データ登録画面ViewModel
    /// </summary>
    public class RegistrationFlowerSellReceiptsAndExpenditureViewModel :
        JustRegistraterDataViewModel
    {
        #region Properties
        #region strings
        private const string CEMETERYFLOWER = "墓地花";
        private const string HIGHFLOWER = "上花";
        private const string SPECIALFLOWER = "特花";
        private const string SAKAKI = "榊";
        private const string ANISATUM = "樒";
        private const string BASKETFLOWER = "籠花";
        private const string INCENSESTICK = "線香";
        private const string TICKET = "チケット";
        private string cemeteryFlower;
        private string cemeteryFlowerUnitPrice;
        private string cemeteryFlowerTotalAmount;
        private string highFlower;
        private string highFlowerUnitPrice;
        private string highFlowerTotalAmount;
        private string specialFlower;
        private string specialFlowerUnitPrice;
        private string specialFlowerTotalAmount;
        private string sakaki;
        private string sakakiUnitPrice;
        private string sakakiTotalAmount;
        private string anisatum;
        private string anisatumUnitPrice;
        private string anisatumTotalAmount;
        private string basketFlower;
        private string basketFlowerUnitPrice;
        private string basketFlowerTotalAmount;
        private string incenseStick;
        private string incenseStickUnitPrice;
        private string incenseStickTotalAmount;
        private string ticket;
        private string ticketUnitPrice;
        private string ticketTotalAmount;
        private string totalAmount;
        private string salesAmount;
        private string registrationButtonContent = "登録";
        private string flowerListTotalAmount;
        private string otherFlowerListTotalAmount;
        #endregion
        #region ints
        private int cemeteryFlowerCount;
        private int highFlowerCount;
        private int specialFlowerCount;
        private int sakakiCount;
        private int anisatumCount;
        private int incenseStickCount;
        private int basketFlowerCount;
        private int ticketCount;
        private int flowerListTotalCount;
        private int otherFlowerListCount;
        #endregion
        #region Dictionaries
        private Dictionary<string, KeyValuePair<int, int>> FlowerDataList =
            new Dictionary<string, KeyValuePair<int, int>>();
        private Dictionary<string, KeyValuePair<int, int>> IncenseStickDataList =
            new Dictionary<string, KeyValuePair<int, int>>();
        private Dictionary<string, KeyValuePair<int, int>> OtherFlowerDataList =
            new Dictionary<string, KeyValuePair<int, int>>();
        #endregion
        private bool isRegistrationEnabled;
        private DateTime selectedDate = DateTime.Today;
        #endregion

        public RegistrationFlowerSellReceiptsAndExpenditureViewModel
            (IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            SetProperty();
            RegistrationCommand = new DelegateCommand(() => Registration(), () => true);
        }
        public RegistrationFlowerSellReceiptsAndExpenditureViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect())
        { }
        /// <summary>
        /// 登録コマンド
        /// </summary>
        public DelegateCommand RegistrationCommand { get; }
        private void Registration()
        {
            ObservableCollection<ReceiptsAndExpenditure> list =
                DataBaseConnect.ReferenceReceiptsAndExpenditure
                    (DefaultDate, DateTime.Now, AccountingProcessLocation.Location.ToString(),
                        string.Empty, string.Empty, string.Empty, string.Empty, "811", false, true, true, true,
                        true, SelectedDate, SelectedDate, DefaultDate, DateTime.Now);

            if (list.Count != 0)
            {
                MessageBox = new MessageBoxInfo()
                {
                    Message = $"{SelectedDate.ToString("ggy年M月d日", JapanCulture)}" +
                    $"の{AccountingProcessLocation.Location}は墓地花売上データが存在します。\r\n" +
                    $"出納管理からデータの更新、追加を行ってください。",
                    Image = MessageBoxImage.Information,
                    Button = MessageBoxButton.OK,
                    Title = "墓地花売上の一括登録は1日1回です。"
                };
                CallShowMessageBox = true;
                return;
            }

            string dataContent = $"{SelectedDate.ToString("ggy年M月d日", JapanCulture)}\r\n\r\n";

            if (CemeteryFlowerCount > 0)
            { AddString(CemeteryFlower, CemeteryFlowerCount, IntAmount(CemeteryFlowerTotalAmount)); }
            if (HighFlowerCount > 0)
            { AddString(HighFlower, HighFlowerCount, IntAmount(HighFlowerTotalAmount)); }
            if (SpecialFlowerCount > 0)
            { AddString(SpecialFlower, SpecialFlowerCount, IntAmount(SpecialFlowerTotalAmount)); }
            if (TicketCount > 0) { AddString(Ticket, TicketCount, IntAmount(TicketTotalAmount)); }
            if (BasketFlowerCount > 0)
            { AddString(BasketFlower, BasketFlowerCount, IntAmount(BasketFlowerTotalAmount)); }
            if (SakakiCount > 0) { AddString(Sakaki, SakakiCount, IntAmount(SakakiTotalAmount)); }
            if (AnisatumCount > 0)
            { AddString(Anisatum, AnisatumCount, IntAmount(AnisatumTotalAmount)); }
            if (IncenseStickCount > 0)
            { AddString(IncenseStick, IncenseStickCount, IntAmount(IncenseStickTotalAmount)); }

            if (CallConfirmationDataOperation
                ($"{dataContent}\r\n登録します。よろしいですか？", "花売り") ==
                    MessageBoxResult.Cancel)
            { return; }

            RegistrationButtonContent = "登録中";
            IsRegistrationEnabled = false;
            DatasRegistration();
            IsRegistrationEnabled = true;
            RegistrationButtonContent = "登録";

            async void DatasRegistration()
            {
                await Task.Run(() => DataRegister());

                void DataRegister()
                {
                    CreditDept creditDept = DataBaseConnect.ReferenceCreditDept("香華", true, false)[0];
                    AccountingSubject accountingSubject = DataBaseConnect.ReferenceAccountingSubject
                        ("811", string.Empty, false, true)[0];
                    Content content;

                    Register(FlowerDataList, accountingSubject);

                    accountingSubject = DataBaseConnect.ReferenceAccountingSubject
                        ("813", string.Empty, false, true)[0];

                    Register(IncenseStickDataList, accountingSubject);

                    accountingSubject = DataBaseConnect.ReferenceAccountingSubject
                        ("828", "その他売上", false, true)[0];

                    Register(OtherFlowerDataList, accountingSubject);

                    CallCompletedRegistration();

                    void Register(Dictionary<string, KeyValuePair<int, int>> list,
                        AccountingSubject accountingSubject)
                    {
                        foreach (KeyValuePair<string, KeyValuePair<int, int>> pair in list)
                        {
                            content = DataBaseConnect.ReferenceContent
                                (pair.Key, accountingSubject.SubjectCode, accountingSubject.Subject, false, true)[0];
                            _ = DataBaseConnect.Registration
                                (new ReceiptsAndExpenditure
                                    (0, DateTime.Today, LoginRep.GetInstance().Rep,
                                        AccountingProcessLocation.Location.ToString(), creditDept, content,
                                        $"{pair.Value.Key}{Space}件", pair.Value.Value, true, true, SelectedDate,
                                        DefaultDate, false));
                        }
                    }
                }
            }

            void AddString(string genre, int count, int amount)
            { dataContent += $"{genre}\t{count}{Space}件\t{AmountWithUnit(amount)}\r\n"; }
        }
        private void SetProperty()
        {
            CemeteryFlower = GetContent(CEMETERYFLOWER).Text;
            CemeteryFlowerUnitPrice = AmountWithUnit(GetContent(CEMETERYFLOWER).FlatRate);
            HighFlower = GetContent(HIGHFLOWER).Text;
            HighFlowerUnitPrice = AmountWithUnit(GetContent(HIGHFLOWER).FlatRate);
            SpecialFlower = GetContent(SPECIALFLOWER).Text;
            SpecialFlowerUnitPrice = AmountWithUnit(GetContent(SPECIALFLOWER).FlatRate);
            Sakaki = GetContent(SAKAKI).Text;
            SakakiUnitPrice = AmountWithUnit(GetContent(SAKAKI).FlatRate);
            Anisatum = GetContent(ANISATUM).Text;
            AnisatumUnitPrice = AmountWithUnit(GetContent(ANISATUM).FlatRate);
            BasketFlower = GetContent(BASKETFLOWER).Text;
            BasketFlowerUnitPrice = AmountWithUnit(GetContent(BASKETFLOWER).FlatRate);
            IncenseStick = GetContent(INCENSESTICK).Text;
            IncenseStickUnitPrice = AmountWithUnit(GetContent(INCENSESTICK).FlatRate);
            Ticket = GetContent(TICKET).Text;
            TicketUnitPrice = AmountWithUnit(GetContent(TICKET).FlatRate);

            Content GetContent(string text)
            {
                ObservableCollection<Content> list = DataBaseConnect.ReferenceContent
                    (text, string.Empty, string.Empty, false, true);
                if (list.Count == 0) { return new Content(string.Empty, null, -1, string.Empty, false); }
                Content content = list[0];
                return content;
            }
        }

        /// <summary>
        /// 金額を計算します
        /// </summary>
        private void AmountCalculation()
        {
            FlowerListTotalCount = 0;
            FlowerListTotalAmount = string.Empty;
            FlowerDataList = new Dictionary<string, KeyValuePair<int, int>>();
            CemeteryFlowerTotalAmount = AmountWithUnit
                (IntAmount(CemeteryFlowerUnitPrice) * CemeteryFlowerCount);
            OperationDataList
                (ref FlowerDataList, CemeteryFlower, CemeteryFlowerCount,
                    IntAmount(CemeteryFlowerTotalAmount));
            HighFlowerTotalAmount = AmountWithUnit(IntAmount(HighFlowerUnitPrice) * HighFlowerCount);
            OperationDataList
                (ref FlowerDataList, HighFlower, HighFlowerCount, IntAmount(HighFlowerTotalAmount));
            SpecialFlowerTotalAmount = AmountWithUnit
                (IntAmount(SpecialFlowerUnitPrice) * SpecialFlowerCount);
            OperationDataList
               (ref FlowerDataList, SpecialFlower, SpecialFlowerCount, IntAmount(SpecialFlowerTotalAmount));
            BasketFlowerTotalAmount =
                AmountWithUnit(IntAmount(BasketFlowerUnitPrice) * BasketFlowerCount);
            OperationDataList
                (ref FlowerDataList, BasketFlower, BasketFlowerCount, IntAmount(BasketFlowerTotalAmount));
            TicketTotalAmount = AmountWithUnit(IntAmount(TicketUnitPrice) * TicketCount);
            OperationDataList
                (ref FlowerDataList, Ticket, TicketCount, IntAmount(TicketTotalAmount));
            foreach (KeyValuePair<string, KeyValuePair<int, int>> pair in FlowerDataList)
            {
                FlowerListTotalAmount =
                    AmountWithUnit(IntAmount(FlowerListTotalAmount) + pair.Value.Value);
                FlowerListTotalCount += pair.Value.Key;
            }

            OtherFlowerListCount = 0;
            OtherFlowerListTotalAmount = string.Empty;
            OtherFlowerDataList = new Dictionary<string, KeyValuePair<int, int>>();
            SakakiTotalAmount = AmountWithUnit(IntAmount(SakakiUnitPrice) * SakakiCount);
            OperationDataList(ref OtherFlowerDataList, Sakaki, SakakiCount, IntAmount(SakakiTotalAmount));
            AnisatumTotalAmount = AmountWithUnit(IntAmount(AnisatumUnitPrice) * AnisatumCount);
            OperationDataList
                (ref OtherFlowerDataList, Anisatum, AnisatumCount, IntAmount(AnisatumTotalAmount));
            foreach (KeyValuePair<string, KeyValuePair<int, int>> pair in OtherFlowerDataList)
            {
                OtherFlowerListCount += pair.Value.Key;
                OtherFlowerListTotalAmount =
                    AmountWithUnit(IntAmount(OtherFlowerListTotalAmount) + pair.Value.Value);
            }

            IncenseStickTotalAmount =
                AmountWithUnit(IntAmount(IncenseStickUnitPrice) * IncenseStickCount);
            OperationDataList
                (ref IncenseStickDataList, IncenseStick, IncenseStickCount,
                    IntAmount(IncenseStickTotalAmount));

            SalesAmount = AmountWithUnit(IntAmount(CemeteryFlowerTotalAmount) +
                IntAmount(HighFlowerTotalAmount) + IntAmount(SpecialFlowerTotalAmount) +
                IntAmount(SakakiTotalAmount) + IntAmount(AnisatumTotalAmount) +
                IntAmount(IncenseStickTotalAmount) + IntAmount(BasketFlowerTotalAmount));

            TotalAmount = AmountWithUnit(IntAmount(SalesAmount) + IntAmount(TicketTotalAmount));

            static void OperationDataList
                (ref Dictionary<string, KeyValuePair<int, int>> list, string genre, int count, int amount)
            {
                if (count != 0)
                {
                    _ = list.Remove(genre);
                    list.Add(genre, new KeyValuePair<int, int>(count, amount));
                }
            }
        }
        /// <summary>
        /// 墓地花
        /// </summary>
        public string CemeteryFlower
        {
            get => cemeteryFlower;
            set
            {
                cemeteryFlower = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 上花
        /// </summary>
        public string HighFlower
        {
            get => highFlower;
            set
            {
                highFlower = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 特花
        /// </summary>
        public string SpecialFlower
        {
            get => specialFlower;
            set
            {
                specialFlower = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 榊
        /// </summary>
        public string Sakaki
        {
            get => sakaki;
            set
            {
                sakaki = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 樒
        /// </summary>
        public string Anisatum
        {
            get => anisatum;
            set
            {
                anisatum = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 線香
        /// </summary>
        public string IncenseStick
        {
            get => incenseStick;
            set
            {
                incenseStick = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 籠花
        /// </summary>
        public string BasketFlower
        {
            get => basketFlower;
            set
            {
                basketFlower = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 墓地花単価表示
        /// </summary>
        public string CemeteryFlowerUnitPrice
        {
            get => cemeteryFlowerUnitPrice;
            set
            {
                cemeteryFlowerUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 上花単価表示
        /// </summary>
        public string HighFlowerUnitPrice
        {
            get => highFlowerUnitPrice;
            set
            {
                highFlowerUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 特花単価表示
        /// </summary>
        public string SpecialFlowerUnitPrice
        {
            get => specialFlowerUnitPrice;
            set
            {
                specialFlowerUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 榊単価表示
        /// </summary>
        public string SakakiUnitPrice
        {
            get => sakakiUnitPrice;
            set
            {
                sakakiUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 樒単価表示
        /// </summary>
        public string AnisatumUnitPrice
        {
            get => anisatumUnitPrice;
            set
            {
                anisatumUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 線香縦単価表示
        /// </summary>
        public string IncenseStickUnitPrice
        {
            get => incenseStickUnitPrice;
            set
            {
                incenseStickUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 籠花単価表示
        /// </summary>
        public string BasketFlowerUnitPrice
        {
            get => basketFlowerUnitPrice;
            set
            {
                basketFlowerUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 墓地花数量
        /// </summary>
        public int CemeteryFlowerCount
        {
            get => cemeteryFlowerCount;
            set
            {
                cemeteryFlowerCount = int.TryParse(value.ToString(), out int _) ? value : 0;
                CallPropertyChanged();
                AmountCalculation();
            }
        }
        /// <summary>
        /// 上花数量
        /// </summary>
        public int HighFlowerCount
        {
            get => highFlowerCount;
            set
            {
                highFlowerCount = int.TryParse(value.ToString(), out int _) ? value : 0;
                CallPropertyChanged();
                AmountCalculation();
            }
        }
        /// <summary>
        /// 特花数量
        /// </summary>
        public int SpecialFlowerCount
        {
            get => specialFlowerCount;
            set
            {
                specialFlowerCount = int.TryParse(value.ToString(), out int _) ? value : 0;
                CallPropertyChanged();
                AmountCalculation();
            }
        }
        /// <summary>
        /// 榊数量
        /// </summary>
        public int SakakiCount
        {
            get => sakakiCount;
            set
            {
                sakakiCount = int.TryParse(value.ToString(), out int _) ? value : 0;
                CallPropertyChanged();
                AmountCalculation();
            }
        }
        /// <summary>
        /// 樒数量
        /// </summary>
        public int AnisatumCount
        {
            get => anisatumCount;
            set
            {
                anisatumCount = int.TryParse(value.ToString(), out int _) ? value : 0;
                CallPropertyChanged();
                AmountCalculation();
            }
        }
        /// <summary>
        /// 線香縦数量
        /// </summary>
        public int IncenseStickCount
        {
            get => incenseStickCount;
            set
            {
                incenseStickCount = int.TryParse(value.ToString(), out int _) ? value : 0;
                CallPropertyChanged();
                AmountCalculation();
            }
        }
        /// <summary>
        /// チケット数量
        /// </summary>
        public int TicketCount
        {
            get => ticketCount;
            set
            {
                ticketCount = int.TryParse(value.ToString(), out int _) ? value : 0;
                CallPropertyChanged();
                AmountCalculation();
            }
        }
        /// <summary>
        /// 総計表示
        /// </summary>
        public string TotalAmount
        {
            get => totalAmount;
            set
            {
                totalAmount = value;
                CallPropertyChanged();
                IsRegistrationEnabled = CemeteryFlowerCount + HighFlowerCount + SpecialFlowerCount +
                    BasketFlowerCount + SakakiCount + AnisatumCount + IncenseStickCount +
                    TicketCount != 0;
            }
        }
        /// <summary>
        /// チケット単価表示
        /// </summary>
        public string TicketUnitPrice
        {
            get => ticketUnitPrice;
            set
            {
                ticketUnitPrice = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 籠花数量
        /// </summary>
        public int BasketFlowerCount
        {
            get => basketFlowerCount;
            set
            {
                basketFlowerCount = int.TryParse(value.ToString(), out int _) ? value : 0;
                CallPropertyChanged();
                AmountCalculation();
            }
        }
        /// <summary>
        /// チケット
        /// </summary>
        public string Ticket
        {
            get => ticket;
            set
            {
                ticket = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 墓地花小計
        /// </summary>
        public string CemeteryFlowerTotalAmount
        {
            get => cemeteryFlowerTotalAmount;
            set
            {
                cemeteryFlowerTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 上花小計
        /// </summary>
        public string HighFlowerTotalAmount
        {
            get => highFlowerTotalAmount;
            set
            {
                highFlowerTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 特花小計
        /// </summary>
        public string SpecialFlowerTotalAmount
        {
            get => specialFlowerTotalAmount;
            set
            {
                specialFlowerTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 榊小計
        /// </summary>
        public string SakakiTotalAmount
        {
            get => sakakiTotalAmount;
            set
            {
                sakakiTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 樒小計
        /// </summary>
        public string AnisatumTotalAmount
        {
            get => anisatumTotalAmount;
            set
            {
                anisatumTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 籠花小計
        /// </summary>
        public string BasketFlowerTotalAmount
        {
            get => basketFlowerTotalAmount;
            set
            {
                basketFlowerTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 線香縦小計
        /// </summary>
        public string IncenseStickTotalAmount
        {
            get => incenseStickTotalAmount;
            set
            {
                incenseStickTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// チケット小計
        /// </summary>
        public string TicketTotalAmount
        {
            get => ticketTotalAmount;
            set
            {
                ticketTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 売上金額
        /// </summary>
        public string SalesAmount
        {
            get => salesAmount;
            set
            {
                salesAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録ボタンのEnabled
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
        /// <summary>
        /// 登録ボタンのContent
        /// </summary>
        public string RegistrationButtonContent
        {
            get => registrationButtonContent;
            set
            {
                registrationButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された日時
        /// </summary>
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 墓地花売上の個数合計
        /// </summary>
        public int FlowerListTotalCount
        {
            get => flowerListTotalCount;
            set
            {
                flowerListTotalCount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 墓地花売上の総金額
        /// </summary>
        public string FlowerListTotalAmount
        {
            get => flowerListTotalAmount;
            set
            {
                flowerListTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// その他売上の総金額
        /// </summary>
        public string OtherFlowerListTotalAmount
        {
            get => otherFlowerListTotalAmount;
            set
            {
                otherFlowerListTotalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// その他売上の個数合計
        /// </summary>
        public int OtherFlowerListCount
        {
            get => otherFlowerListCount;
            set
            {
                otherFlowerListCount = value;
                CallPropertyChanged();
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {

        }

        protected override void SetWindowDefaultTitle()
        { DefaultWindowTitle = $"花売りデータ登録：{AccountingProcessLocation.Location}"; }
    }
}
