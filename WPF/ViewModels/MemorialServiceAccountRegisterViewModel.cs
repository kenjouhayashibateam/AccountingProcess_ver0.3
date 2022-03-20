using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.ViewModels.Datas;
using static Domain.Entities.Helpers.DataHelper;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 法事計算書登録画面ViewModel
    /// </summary>
    public class MemorialServiceAccountRegisterViewModel : JustRegistraterDataViewModel
    {
        #region Properties
        #region Ints
        private int noukotuAmount;
        private int memorialServiceDonationAmount;
        private int serviceFee;
        private int guestsNumber;
        private int burningIncenseTableAmount;
        private int shunjuenSubTotal;
        private int stupaStandAmount;
        private int parasolAmount;
        private int parasolCount;
        private int boneEnshrinedAmount;
        private int stupaCount;
        private int stupaAmount;
        #endregion
        #region Strings
        private string customerName;
        private string selectedVariousChange;
        private string variousChangeAmountDisplayValue;
        private string boneVaseAmount;
        private string offeringAmount;
        private string returnGiftAmount;
        private string blancAmount;
        private string boneWashingAmount;
        private string boneBagAmount;
        private string tagPlateAddingSculptureAmount;
        private string graveCleaningAmount;
        #endregion
        #region Bools
        private bool isNoukotuInput;
        private bool isBurningIncenseTableInput;
        private bool isStupaStandInput;
        private bool isParasolInput;
        private bool isBoneEnshrinedInput;
        private bool boneEnshrinedAmountToggle;
        private bool noukotuAccountToggle;
        #endregion
        #region ReceiptsAndExpenditures
        private ReceiptsAndExpenditure noukotuData;
        private ReceiptsAndExpenditure memorialServiceDonationData;
        private ReceiptsAndExpenditure burningIncenseTableData;
        private ReceiptsAndExpenditure stupaStandData;
        private ReceiptsAndExpenditure parasolData;
        private ReceiptsAndExpenditure variousChangeData;
        private ReceiptsAndExpenditure boneEnshrinedData;
        private ReceiptsAndExpenditure stupaData;
        private ReceiptsAndExpenditure boneVaseData;
        private ReceiptsAndExpenditure offeringData;
        private ReceiptsAndExpenditure returnGiftData;
        private ReceiptsAndExpenditure blancData;
        private ReceiptsAndExpenditure boneWashingData;
        private ReceiptsAndExpenditure boneBagData;
        private ReceiptsAndExpenditure tagPlateAddingSculptureData;
        private ReceiptsAndExpenditure graveCleaningData;
        #endregion
        private DateTime accountActivityDate;
        private Dictionary<string,int> places = new Dictionary<string, int>() 
        { { string.Empty, 0 }, { "礼拝堂", 50000 }, { "白蓮華堂", 50000 }, { "青蓮堂", 50000 }, { "特別参拝室", 10000 } };
        private List<string> variousChanges = new List<string>() { string.Empty, "名義変更", "再発行" };
        private ObservableCollection<Content> graveCleaningContents;
        private ObservableCollection<ReceiptsAndExpenditure> ShunjuenData =
            new ObservableCollection<ReceiptsAndExpenditure>();
        private KeyValuePair<string, int> selectedPlace;
        private Content selectedGraveCleaningContent;
        private readonly int[] BoneEnshrinedAmounts = new int[2] { 3000, 30000 };
        #endregion

        public MemorialServiceAccountRegisterViewModel(IDataBaseConnect dataBaseConnect) : base(dataBaseConnect)
        {
            AccountActivityDate = DateTime.Now;
            SetReceiptsAndExpenditure();
            NoukotuAccountToggle = true;
        }
        public MemorialServiceAccountRegisterViewModel() : this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        /// <summary>
        /// 春秋苑小計を設定します
        /// </summary>
        private void SetShunjuenSubTotal()
        {
            ShunjuenSubTotal = 0;
            SetReceiptsAndExpenditure();
            foreach (ReceiptsAndExpenditure rae in ShunjuenData) { ShunjuenSubTotal += rae.Price; }
        }
        /// <summary>
        /// 各出納データを設定します
        /// </summary>
        private void SetReceiptsAndExpenditure()
        {
            Rep rep = LoginRep.GetInstance().Rep;
            string location=AccountingProcessLocation.Location.ToString();
            CreditDept shunjuenDept = DataBaseConnect.ReferenceCreditDept("春秋苑", true, true)[0];
            ShunjuenData = new ObservableCollection<ReceiptsAndExpenditure>();

            if (NoukotuData == null)
            {
                NoukotuData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("納骨", "811", string.Empty, true, true)[0], CustomerName,
                    NoukotuAmount, true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                NoukotuData.AccountActivityDate = AccountActivityDate;
                NoukotuData.Price = NoukotuAmount;
                NoukotuData.Detail = CustomerName;
            }
            ShunjuenData.Add(NoukotuData);

            if (MemorialServiceDonationData == null)
            {
                MemorialServiceDonationData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("法事冥加", "813", string.Empty, true, true)[0], string.Empty,
                    MemorialServiceDonationAmount + ServiceFee, true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                MemorialServiceDonationData.AccountActivityDate = AccountActivityDate;
                MemorialServiceDonationData.Price = MemorialServiceDonationAmount + ServiceFee;
                MemorialServiceDonationData.Detail = CustomerName;
            }
            ShunjuenData.Add(MemorialServiceDonationData);

            if (BurningIncenseTableData == null)
            {
                BurningIncenseTableData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("焼香台", "812", string.Empty, true, true)[0], string.Empty,
                    BurningIncenseTableAmount, true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                BurningIncenseTableData.AccountActivityDate = AccountActivityDate;
                BurningIncenseTableData.Price = BurningIncenseTableAmount;
                BurningIncenseTableData.Detail = CustomerName;
            }
            ShunjuenData.Add(BurningIncenseTableData);

            if (StupaStandData == null)
            {
                StupaStandData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("簡易塔婆立", "812", string.Empty, true, true)[0], string.Empty,
                    StupaStandAmount, true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                StupaStandData.AccountActivityDate = AccountActivityDate;
                StupaStandData.Price = StupaStandAmount;
                StupaStandData.Detail = CustomerName;
            }
            ShunjuenData.Add(StupaStandData);

            if (parasolData == null)
            {
                ParasolData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("パラソル", "812", string.Empty, true, true)[0], string.Empty,
                    ParasolAmount, true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                parasolData.AccountActivityDate = AccountActivityDate;
                parasolData.Price = ParasolAmount;
                parasolData.Detail = CustomerName;
            }
            ShunjuenData.Add(ParasolData);

            SelectedVariousChange = string.IsNullOrEmpty(SelectedVariousChange) ? string.Empty : SelectedVariousChange;
            VariousChangeData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                DataBaseConnect.ReferenceContent(SelectedVariousChange, "814", string.Empty, true, true)[0], string.Empty,
                IntAmount(VariousChangeAmountDisplayValue), true, true, AccountActivityDate, DefaultDate, false);
            VariousChangeData.AccountActivityDate = AccountActivityDate;
            VariousChangeData.Price = IntAmount(VariousChangeAmountDisplayValue);
            VariousChangeData.Detail = CustomerName;
            ShunjuenData.Add(VariousChangeData);

            if (BoneEnshrinedData == null)
            {
                BoneEnshrinedData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("ご遺骨預かり", "817", string.Empty, true, true)[0], string.Empty,
                    BoneEnshrinedAmount, true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                BoneEnshrinedData.AccountActivityDate = AccountActivityDate;
                BoneEnshrinedData.Price = BoneEnshrinedAmount;
                BoneEnshrinedData.Detail = CustomerName;
            }
            ShunjuenData.Add(BoneEnshrinedData);

            if(StupaData==null)
            { 
            StupaData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                DataBaseConnect.ReferenceContent("卒塔婆", "822", string.Empty, true, true)[0], string.Empty,
                StupaAmount, true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                StupaData.AccountActivityDate = AccountActivityDate;
                StupaData.Price = StupaAmount;
                StupaData.Detail = CustomerName;
            }
            ShunjuenData.Add(StupaData);

            if (BoneVaseData == null)
            {
                BoneVaseData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("骨壺", "822", string.Empty, true, true)[0], string.Empty,
                    IntAmount(BoneVaseAmount), true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                BoneVaseData.AccountActivityDate = AccountActivityDate;
                BoneVaseData.Price = IntAmount(BoneVaseAmount);
                BoneVaseData.Detail = CustomerName;
            }
            ShunjuenData.Add(BoneVaseData);

            if (OfferingData == null)
            {
                OfferingData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("供物", "871", string.Empty, true, true)[0], string.Empty,
                    IntAmount(OfferingAmount), true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                OfferingData.AccountActivityDate = AccountActivityDate;
                OfferingData.Price = IntAmount(OfferingAmount);
                OfferingData.Detail = CustomerName;
            }
            ShunjuenData.Add(OfferingData);

            if (ReturnGiftData == null)
            {
                ReturnGiftData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("返礼品", "871", string.Empty, true, true)[0], string.Empty,
                    IntAmount(ReturnGiftAmount), true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                ReturnGiftData.AccountActivityDate = AccountActivityDate;
                ReturnGiftData.Price = IntAmount(ReturnGiftAmount);
                ReturnGiftData.Detail = CustomerName;
            }
            ShunjuenData.Add(ReturnGiftData);

            if (BlancData == null)
            {
                BlancData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("ブラン", "871", string.Empty, true, true)[0], string.Empty,
                    IntAmount(BlancAmount), true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                BlancData.AccountActivityDate = AccountActivityDate;
                BlancData.Price = IntAmount(BlancAmount);
                BlancData.Detail = CustomerName;
            }
            ShunjuenData.Add(BlancData);

            if (BoneWashingData == null)
            {
                BoneWashingData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("洗骨料", "852", string.Empty, true, true)[0], string.Empty,
                    IntAmount(BoneWashingAmount), true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                BoneWashingData.AccountActivityDate = AccountActivityDate;
                BoneWashingData.Price = IntAmount(BoneWashingAmount);
                BoneWashingData.Detail = CustomerName;
            }
            ShunjuenData.Add(BoneWashingData);

            if (BoneBagData == null)
            {
                BoneBagData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("納骨袋", "852", string.Empty, true, true)[0], string.Empty,
                    IntAmount(BoneBagAmount), true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                BoneBagData.AccountActivityDate = AccountActivityDate;
                BoneBagData.Price = IntAmount(BoneBagAmount);
                BoneBagData.Detail = CustomerName;
            }
            ShunjuenData.Add(BoneBagData);

            if (TagPlateAddingSculptureData == null)
            {
                TagPlateAddingSculptureData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                    DataBaseConnect.ReferenceContent("銘板追加彫刻", "882", string.Empty, true, true)[0], string.Empty,
                    IntAmount(TagPlateAddingSculptureAmount), true, true, AccountActivityDate, DefaultDate, false);
            }
            else
            {
                TagPlateAddingSculptureData.AccountActivityDate = AccountActivityDate;
                TagPlateAddingSculptureData.Price = IntAmount(TagPlateAddingSculptureAmount);
                TagPlateAddingSculptureData.Detail = CustomerName;
            }
            ShunjuenData.Add(TagPlateAddingSculptureData);

            GraveCleaningContents = DataBaseConnect.ReferenceContent(string.Empty, "821", "墓域清掃料", true, true);
            SelectedGraveCleaningContent ??= GraveCleaningContents[0];
            GraveCleaningData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                SelectedGraveCleaningContent, string.Empty, IntAmount(GraveCleaningAmount), true, true,
                AccountActivityDate, DefaultDate, false);
            ShunjuenData.Add(GraveCleaningData);
        }
        /// <summary>
        /// 納骨冥加データ
        /// </summary>
        public ReceiptsAndExpenditure NoukotuData
        {
            get => noukotuData;
            set
            {
                noukotuData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 入金日
        /// </summary>
        public DateTime AccountActivityDate
        {
            get => accountActivityDate;
            set
            {
                accountActivityDate = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 施主名
        /// </summary>
        public string CustomerName
        {
            get => customerName;
            set
            {
                customerName = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 納骨冥加金額
        /// </summary>
        public int NoukotuAmount
        {
            get => noukotuAmount;
            set
            {
                noukotuAmount = value;
                CallPropertyChanged();
                NoukotuData.Price = value;
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 納骨冥加を一覧に加算するか
        /// </summary>
        public bool IsNoukotuInput
        {
            get => isNoukotuInput;
            set
            {
                isNoukotuInput = value;
                CallPropertyChanged();
                NoukotuAmount = value ? NoukotuData.Content.FlatRate : 0;
            }
        }
        /// <summary>
        /// 法事冥加データ
        /// </summary>
        public ReceiptsAndExpenditure MemorialServiceDonationData
        {
            get => memorialServiceDonationData;
            set
            {
                memorialServiceDonationData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 会場リスト
        /// </summary>
        public Dictionary<string, int> Places
        {
            get => places;
            set
            {
                places = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 法事冥加金額
        /// </summary>
        public int MemorialServiceDonationAmount
        {
            get => memorialServiceDonationAmount;
            set
            {
                memorialServiceDonationAmount = value;
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// サービス料金額
        /// </summary>
        public int ServiceFee
        {
            get => serviceFee;
            set
            {
                serviceFee = value;
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 選択された会場
        /// </summary>
        public KeyValuePair<string, int> SelectedPlace
        {
            get => selectedPlace;
            set
            {
                selectedPlace = value;
                CallPropertyChanged();
                MemorialServiceDonationAmount = value.Value;
                MemorialServiceDonationData.Price = value.Value + ServiceFee;
            }
        }
        /// <summary>
        /// 客数
        /// </summary>
        public int GuestsNumber
        {
            get => guestsNumber;
            set
            {
                guestsNumber = value;
                CallPropertyChanged();
                ServiceFee = 500 * value;
            }
        }
        /// <summary>
        ///  焼香台
        /// </summary>
        public ReceiptsAndExpenditure BurningIncenseTableData
        {
            get => burningIncenseTableData;
            set
            {
                burningIncenseTableData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 焼香台使用料
        /// </summary>
        public int BurningIncenseTableAmount
        {
            get => burningIncenseTableAmount;
            set
            {
                burningIncenseTableAmount = value;
                CallPropertyChanged();
                SetShunjuenSubTotal() ;
            }
        }
        /// <summary>
        /// 焼香台使用料を一覧に加算するか
        /// </summary>
        public bool IsBurningIncenseTableInput
        {
            get => isBurningIncenseTableInput;
            set
            {
                isBurningIncenseTableInput = value;
                CallPropertyChanged();
                BurningIncenseTableAmount = value ? BurningIncenseTableData.Content.FlatRate : 0;
            }
        }
        /// <summary>
        /// 春秋苑会計の小計
        /// </summary>
        public int ShunjuenSubTotal
        {
            get => shunjuenSubTotal;
            set
            {
                shunjuenSubTotal = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 簡易塔婆立
        /// </summary>
        public ReceiptsAndExpenditure StupaStandData
        {
            get => stupaStandData;
            set
            {
                stupaStandData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 簡易塔婆立使用料を加算するか
        /// </summary>
        public bool IsStupaStandInput
        {
            get => isStupaStandInput;
            set
            {
                isStupaStandInput = value;
                CallPropertyChanged();
                StupaStandAmount = value ? StupaStandData.Content.FlatRate : 0;
            }
        }
        /// <summary>
        /// 簡易塔婆立使用料
        /// </summary>
        public int StupaStandAmount
        {
            get => stupaStandAmount;
            set
            {
                stupaStandAmount = value;
                CallPropertyChanged();
                StupaStandData.Price = value;
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// パラソル
        /// </summary>
        public ReceiptsAndExpenditure ParasolData
        {
            get => parasolData;
            set
            {
                parasolData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// パラソル使用料を加算するか
        /// </summary>
        public bool IsParasolInput
        {
            get => isParasolInput;
            set
            {
                isParasolInput = value;
                CallPropertyChanged();
                ParasolCount = value ? 1 : 0;
            }
        }
        /// <summary>
        /// パラソル使用料
        /// </summary>
        public int ParasolAmount
        {
            get => parasolAmount;
            set
            {
                parasolAmount = value;
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 各種変更
        /// </summary>
        public ReceiptsAndExpenditure VariousChangeData
        {
            get => variousChangeData;
            set
            {
                variousChangeData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 各種変更の内容リスト
        /// </summary>
        public List<string> VariousChanges
        {
            get => variousChanges;
            set
            {
                variousChanges = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された各種変更
        /// </summary>
        public string SelectedVariousChange
        {
            get => selectedVariousChange;
            set
            {
                selectedVariousChange = value;
                CallPropertyChanged();
                if (string.IsNullOrEmpty(value)) { return; }

                CreditDept creditDept = DataBaseConnect.ReferenceCreditDept("春秋苑", true, true)[0];
                Content content = DataBaseConnect.ReferenceContent(value, "814", string.Empty, true, true)[0];

                VariousChangeData = new ReceiptsAndExpenditure(0, DateTime.Today, LoginRep.GetInstance().Rep, 
                    AccountingProcessLocation.Location.ToString(), creditDept, content, CustomerName, 0, true, true, 
                    AccountActivityDate, DefaultDate, false);
            }
        }
        /// <summary>
        /// 表示用各種変更冥加金額
        /// </summary>
        public string VariousChangeAmountDisplayValue
        {
            get => variousChangeAmountDisplayValue;
            set
            {
                variousChangeAmountDisplayValue = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 御骨安置
        /// </summary>
        public ReceiptsAndExpenditure BoneEnshrinedData
        {
            get => boneEnshrinedData;
            set
            {
                boneEnshrinedData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 御骨安置を一覧に加算するか
        /// </summary>
        public bool IsBoneEnshrinedInput
        {
            get => isBoneEnshrinedInput;
            set
            {
                isBoneEnshrinedInput = value;
                CallPropertyChanged();
                int i = BoneEnshrinedAmountToggle ? 0 : 1;
                BoneEnshrinedAmount = value ? BoneEnshrinedAmounts[i] : 0;
            }
        }
        /// <summary>
        /// ご遺骨預かり金額
        /// </summary>
        public int BoneEnshrinedAmount
        {
            get => boneEnshrinedAmount;
            set
            {
                boneEnshrinedAmount = value;
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 卒塔婆
        /// </summary>
        public ReceiptsAndExpenditure StupaData
        {
            get => stupaData;
            set
            {
                stupaData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 卒塔婆の本数
        /// </summary>
        public int StupaCount
        {
            get => stupaCount;
            set
            {
                stupaCount = value;
                CallPropertyChanged();
                StupaAmount = 3000 * value;
            }
        }
        /// <summary>
        /// 卒塔婆金額
        /// </summary>
        public int StupaAmount
        {
            get => stupaAmount;
            set
            {
                stupaAmount = value;
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 骨壺
        /// </summary>
        public ReceiptsAndExpenditure BoneVaseData
        {
            get => boneVaseData;
            set
            {
                boneVaseData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 骨壺料金
        /// </summary>
        public string BoneVaseAmount
        {
            get => boneVaseAmount;
            set
            {
                boneVaseAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 供物
        /// </summary>
        public ReceiptsAndExpenditure OfferingData
        {
            get => offeringData;
            set
            {
                offeringData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 供物金額
        /// </summary>
        public string OfferingAmount
        {
            get => offeringAmount;
            set
            {
                offeringAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 返礼品
        /// </summary>
        public ReceiptsAndExpenditure ReturnGiftData
        {
            get => returnGiftData;
            set
            {
                returnGiftData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 返礼品金額
        /// </summary>
        public string ReturnGiftAmount
        {
            get => returnGiftAmount;
            set
            {
                returnGiftAmount = value;
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// ブラン
        /// </summary>
        public ReceiptsAndExpenditure BlancData
        {
            get => blancData;
            set
            {
                blancData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ブラン金額
        /// </summary>
        public string BlancAmount
        {
            get => blancAmount;
            set
            {
                blancAmount = value;
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 洗骨
        /// </summary>
        public ReceiptsAndExpenditure BoneWashingData
        {
            get => boneWashingData;
            set
            {
                boneWashingData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 洗骨金額
        /// </summary>
        public string BoneWashingAmount
        {
            get => boneWashingAmount;
            set
            {
                boneWashingAmount = value;
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 納骨袋
        /// </summary>
        public ReceiptsAndExpenditure BoneBagData
        {
            get => boneBagData;
            set
            {
                boneBagData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 納骨袋代
        /// </summary>
        public string BoneBagAmount
        {
            get => boneBagAmount;
            set
            {
                boneBagAmount = value;
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 銘板追加彫刻
        /// </summary>
        public ReceiptsAndExpenditure TagPlateAddingSculptureData
        {
            get => tagPlateAddingSculptureData;
            set
            {
                tagPlateAddingSculptureData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 銘板追加彫刻金額
        /// </summary>
        public string TagPlateAddingSculptureAmount
        {
            get => tagPlateAddingSculptureAmount;
            set
            {
                tagPlateAddingSculptureAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// パラソルの本数
        /// </summary>
        public int ParasolCount
        {
            get => parasolCount;
            set
            {
                parasolCount = value;
                CallPropertyChanged();
                ParasolAmount = ParasolData.Content.FlatRate * value;
            }
        }
        /// <summary>
        /// 墓域清掃
        /// </summary>
        public ReceiptsAndExpenditure GraveCleaningData
        {
            get => graveCleaningData;
            set
            {
                graveCleaningData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 墓域清掃料
        /// </summary>
        public string GraveCleaningAmount
        {
            get => graveCleaningAmount;
            set
            {
                graveCleaningAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 墓域清掃料のContents
        /// </summary>
        public ObservableCollection<Content> GraveCleaningContents
        {
            get => graveCleaningContents;
            set
            {
                graveCleaningContents = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された墓域清掃料のContent
        /// </summary>
        public Content SelectedGraveCleaningContent
        {
            get => selectedGraveCleaningContent;
            set
            {
                selectedGraveCleaningContent = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 遺骨預かりの金額のトグルボタン
        /// </summary>
        public bool BoneEnshrinedAmountToggle
        {
            get => boneEnshrinedAmountToggle;
            set
            {
                boneEnshrinedAmountToggle = value;
                CallPropertyChanged();
                BoneEnshrinedAmount = value ? BoneEnshrinedAmounts[0] : BoneEnshrinedAmounts[1];
            }
        }
        /// <summary>
        /// 納骨、出骨のトグル
        /// </summary>
        public bool NoukotuAccountToggle
        {
            get => noukotuAccountToggle;
            set
            {
                noukotuAccountToggle = value;
                CallPropertyChanged();
                int i = value ? 0 : 1;
                NoukotuData.Content = 
                    DataBaseConnect.ReferenceContent(string.Empty, "811", string.Empty, true, true)[i];
            }
        }

        public override void ValidationProperty(string propertyName, object value) { }

        protected override void SetWindowDefaultTitle()
        { DefaultWindowTitle = $"法事計算書登録 : {AccountingProcessLocation.Location}"; }
    }
}
