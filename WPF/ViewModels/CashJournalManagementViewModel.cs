using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 出納帳出力画面ViewModel
    /// </summary>
    public class CashJournalManagementViewModel : BaseViewModel
    {
        private string yearString;
        private string monthString = "1";
        private string outputButtonContent = "出力";
        private bool outputButtonEnabled;
        private readonly IDataOutput DataOutput;

        public CashJournalManagementViewModel(IDataBaseConnect dataBaseConnect, 
            IDataOutput dataOutput) :
            base(dataBaseConnect)
        {
            DataOutput = dataOutput;
            YearString = DateTime.Now.Year.ToString();
            MonthString = DateTime.Now.Month.ToString();
            OutputCommand = new DelegateCommand(() => Output(), () => true);
        }
        public CashJournalManagementViewModel() :
            this(DefaultInfrastructure.GetDefaultDataBaseConnect(),
                DefaultInfrastructure.GetDefaultDataOutput())
        { }
        /// <summary>
        /// 出納帳出力コマンド
        /// </summary>
        public DelegateCommand OutputCommand { get; }
        private async void Output()
        {
            DateTime searchDateStart = new DateTime(IntAmount(YearString), IntAmount(MonthString), 1);
            DateTime searchDateEnd =
                searchDateStart == DateTime.Today.AddDays(-1 * (DateTime.Today.Day - 1)) ? DateTime.Today :
                                                searchDateStart.AddMonths(1).AddDays(-1);

            if (searchDateStart < new DateTime(2021, 4, 1))
            {
                CallOutputBlockingMessage();
                return;
            }

            OutputButtonContent = "出力中";
            await Task.Run(
                () => DataOutput.ReceiptsAndExpenditureData
                    (
                        DataBaseConnect.ReferenceReceiptsAndExpenditure
                            (DefaultDate, DateTime.Now, string.Empty, string.Empty, string.Empty, string.Empty,
                                string.Empty, string.Empty, false, true, true, true, DefaultDate, DateTime.Now, 
                                searchDateStart,searchDateEnd)
                    )
                );
            OutputButtonContent = "出力";

            void CallOutputBlockingMessage()
            {
                MessageBox = new Views.Datas.MessageBoxInfo()
                {
                    Message = "2020年度以前の出納帳は、データ不足のため出力できません。",
                    Image = System.Windows.MessageBoxImage.Information,
                    Button = System.Windows.MessageBoxButton.OK,
                    Title = "期間外出力"
                };
                CallPropertyChanged(nameof(MessageBox));
            }
        }
        /// <summary>
        /// 出力する年
        /// </summary>
        public string YearString
        {
            get => yearString;
            set
            {
                yearString = int.TryParse(value, out int i) ? i.ToString() : string.Empty;
                ValidationProperty(nameof(YearString), yearString);
                CallPropertyChanged();
                ValidationProperty(nameof(MonthString), MonthString);
            }
        }
        /// <summary>
        /// 出力する月
        /// </summary>
        public string MonthString
        {
            get => monthString;
            set
            {
                monthString = int.TryParse(value, out int i) ? i.ToString() : string.Empty;
                ValidationProperty(nameof(MonthString), monthString);
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出力ボタンのContent
        /// </summary>
        public string OutputButtonContent
        {
            get => outputButtonContent;
            set
            {
                outputButtonContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 出力ボタンのEnabled
        /// </summary>
        public bool OutputButtonEnabled
        {
            get => outputButtonEnabled;
            set
            {
                outputButtonEnabled = value;
                CallPropertyChanged();
            }
        }

        private void SetOutputButtonEnabled() =>
            OutputButtonEnabled =
                !HasErrors && !string.IsNullOrEmpty(YearString) && !string.IsNullOrEmpty(MonthString);

        public override void ValidationProperty(string propertyName, object value)
        {
            switch (propertyName)
            {
                case nameof(YearString):
                    {
                        SetNullOrEmptyError(propertyName, (string)value);
                        ErrorsListOperation
                            (int.Parse((string)value) > DateTime.Now.Year, propertyName,
                                "未来の出納帳は出せません");
                        break;
                    }
                case nameof(MonthString):
                    {
                        SetNullOrEmptyError(propertyName, (string)value);
                        int y = IntAmount(YearString);
                        int m = IntAmount((string)value);
                        ErrorsListOperation(!Enumerable.Range(1, 12).Contains(m), propertyName, "月が無効です");
                        if (GetErrors(propertyName) == null) ErrorsListOperation
                               (new DateTime(y, m, 1) > DateTime.Now, propertyName, "未来の出納帳は出せません");
                        break;
                    }
                default:
                    break;
            }
            SetOutputButtonEnabled();
        }

        protected override void SetWindowDefaultTitle() => DefaultWindowTitle =
            $"出納帳出力{Space}:{Space}{AccountingProcessLocation.Location}";
    }
}