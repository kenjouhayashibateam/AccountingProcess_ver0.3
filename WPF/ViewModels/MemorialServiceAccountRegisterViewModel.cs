using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WPF.ViewModels.Commands;
using WPF.ViewModels.Datas;
using WPF.Views.Datas;
using static Domain.Entities.Helpers.DataHelper;
using static Domain.Entities.Helpers.TextHelper;

namespace WPF.ViewModels
{
    /// <summary>
    /// 法事計算書登録画面ViewModel
    /// </summary>
    public class MemorialServiceAccountRegisterViewModel : JustRegistraterDataViewModel, IClosing
    {
        #region Properties
        #region Ints
        private int noukotuAmount;
        private int memorialServiceDonationAmount;
        private int serviceFee;
        private int guestsNumber;
        private int burningIncenseTableAmount;
        private int shunjuenSubTotal;
        private int wizeCoreSubTotal;
        private int stupaStandAmount;
        private int parasolAmount;
        private int parasolCount;
        private int boneEnshrinedAmount;
        private int stupaCount;
        private int stupaAmount;
        private int flowerOfferingCount;
        private int flowerOfferingAmount;
        private int houmuSubTotal;
        private int totalAmount;
        #endregion
        #region Strings
        private string customerName;
        private string selectedVariousChange;
        private string variousChangeAmount;
        private string boneVaseAmount;
        private string offeringAmount;
        private string returnGiftAmount;
        private string blancAmount;
        private string boneWashingAmount;
        private string boneBagAmount;
        private string tagPlateAddingSculptureAmount;
        private string graveCleaningAmount;
        private string floralTributeAmount;
        private string cemeteryFlowerAmount;
        private string incenseStickAmount;
        private string flowerOfferingFlatRate;
        private string shunjuanReturnGiftAmount;
        private string shunjuanMealSalesAmount;
        private string rengeanSalesAmount;
        private string rengeanMealSalesAmount;
        private string rengeanBeverageSalesAmount;
        private string buddhistChantDonationAmount;
        private string almsgivingAmount;
        private string carTipAmount;
        private string mealTipAmount;
        private string carAndMealTipAmount;
        private string stoneWorkPartAmount;
        #endregion
        #region Bools
        private bool isNoukotuInput;
        private bool isBurningIncenseTableInput;
        private bool isStupaStandInput;
        private bool isParasolInput;
        private bool isBoneEnshrinedInput;
        private bool boneEnshrinedAmountToggle;
        private bool noukotuAccountToggle;
        private bool isSundry;
        private bool isStoneWorkPartInput;
        private bool isCloseCancel = false;
        private bool canOperation;
        private bool isIndoorCemeteryCineration;
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
        private ReceiptsAndExpenditure floralTributeData;
        private ReceiptsAndExpenditure cemeteryFlowerData;
        private ReceiptsAndExpenditure incenseStickData;
        private ReceiptsAndExpenditure flowerOfferingData;
        private ReceiptsAndExpenditure shunjuanReturnGiftData;
        private ReceiptsAndExpenditure shunjuanMealSalesData;
        private ReceiptsAndExpenditure rengeanSalesData;
        private ReceiptsAndExpenditure buddhistChantDonationData;
        private ReceiptsAndExpenditure almsgivingData;
        private ReceiptsAndExpenditure carTipData;
        private ReceiptsAndExpenditure mealTipData;
        private ReceiptsAndExpenditure carAndMealTipData;
        private ReceiptsAndExpenditure stoneWorkPartData;
        #endregion
        #region ObservableCollections
        private ObservableCollection<Content> graveCleaningContents;
        private ObservableCollection<ReceiptsAndExpenditure> ShunjuenData;
        private ObservableCollection<ReceiptsAndExpenditure> WizeCoreData;
        private ObservableCollection<ReceiptsAndExpenditure> HoumuData;
        private ObservableCollection<TransferReceiptsAndExpenditure> WizeCoreTransferData;
        private ObservableCollection<Content> buddhistChantDonationContents;
        private ObservableCollection<Content> stoneWorkPartContents;
        private ObservableCollection<Content> floralTributeCotents;
        #endregion
        #region Contents
        private Content selectedGraveCleaningContent;
        private Content selectedStoneWorkPartContent;
        private Content selectedBuddhistChantDonationContent;
        private Content selectedFloralTributeContent;
        #endregion
        private DateTime accountActivityDate;
        private Dictionary<string,int> places = new Dictionary<string, int>() 
        { { string.Empty, 0 }, { "礼拝堂", 50000 }, { "白蓮華堂", 50000 }, { "青蓮堂", 50000 },
            { "特別参拝室", 10000 } };
        private KeyValuePair<string, int> selectedPlace;
        private List<string> variousChanges = new List<string>() 
        { string.Empty, "名義変更", "再発行" };
        private readonly int[] BoneEnshrinedAmounts = new int[2] { 3000, 30000 };
        private TransferReceiptsAndExpenditure rengeanMealSalesData;
        private TransferReceiptsAndExpenditure rengeanBeverageSalesData;
        #endregion

        public MemorialServiceAccountRegisterViewModel(IDataBaseConnect dataBaseConnect) :
            base(dataBaseConnect)
        {
            AccountActivityDate = DateTime.Now;
            SetReceiptsAndExpenditure();
            NoukotuAccountToggle = true;
            BuddhistChantDonationContents = 
                DataBaseConnect.ReferenceContent(string.Empty, "831", "懇志読経料", true, true);
            StoneWorkPartContents = 
                DataBaseConnect.ReferenceContent(string.Empty, "254", "石材工事部勘定", true, true);
            DataRegistrationCommand = new DelegateCommand(() => DataRegistration(), () => true);
            FieldClear();
        }
        public MemorialServiceAccountRegisterViewModel() : 
            this(DefaultInfrastructure.GetDefaultDataBaseConnect()) { }
        private void FieldClear()
        {
            GuestsNumber = 0;
            ParasolCount= 0;
            BoneEnshrinedAmount = 0;
            StupaCount = 0;
            FlowerOfferingCount = 0;
            CustomerName = string.Empty;
            SelectedVariousChange = string.Empty;
            VariousChangeAmount = "0";
            BoneVaseAmount = "0";
            OfferingAmount = "0";
            ReturnGiftAmount = "0";
            BlancAmount = "0";
            BoneWashingAmount = "0";
            BoneBagAmount = "0";
            TagPlateAddingSculptureAmount = "0";
            GraveCleaningAmount = "0";
            FloralTributeAmount = "0";
            CemeteryFlowerAmount = "0";
            IncenseStickAmount = "0";
            FlowerOfferingFlatRate = string.Empty;
            ShunjuanReturnGiftAmount = "0";
            RengeanSalesAmount = "0";
            BuddhistChantDonationAmount = "0";
            AlmsgivingAmount = "0";
            CarTipAmount = "0";
            MealTipAmount = "0";
            CarAndMealTipAmount = "0";
            StoneWorkPartAmount = "0";
            ShunjuanMealSalesAmount = "0";
            IsNoukotuInput = false;
            IsBurningIncenseTableInput = false;
            IsStupaStandInput = false;
            IsParasolInput = false;
            IsBoneEnshrinedInput = false;
            NoukotuAccountToggle = true;
            IsSundry = false;
            IsStoneWorkPartInput = false;
            IsIndoorCemeteryCineration = false;
            SelectedBuddhistChantDonationContent = BuddhistChantDonationContents[0];
        }
        /// <summary>
        /// 一括登録コマンド
        /// </summary>    
        public DelegateCommand DataRegistrationCommand { get; }
        private async void DataRegistration()
        {
            if (CallConfirmationDataOperation("一括登録しますか？", "一括登録") == 
                System.Windows.MessageBoxResult.Cancel)
            { return; }

            IsCloseCancel = true;

            await Task.Run(() => Registration());

            CallCompletedRegistration();
            FieldClear();

            IsCloseCancel = false;

            void Registration()
            {
                ListRegistration(HoumuData);
                ListRegistration(ShunjuenData);
                ListRegistration(WizeCoreData);

                if (IsSundry)
                {
                    foreach (TransferReceiptsAndExpenditure trae in WizeCoreTransferData)
                    { _ = DataBaseConnect.Registration(trae); }
                }

                void ListRegistration(ObservableCollection<ReceiptsAndExpenditure> list)
                {
                    foreach (ReceiptsAndExpenditure rae in list)
                    {
                        if (rae.Price == 0) { continue; }
                        _ = DataBaseConnect.Registration(rae);
                    }
                }
            }
        }
        /// <summary>
        /// 総計を設定します
        /// </summary>
        private void SetTotalAmount()
        { TotalAmount = ShunjuenSubTotal + HoumuSubTotal + WizeCoreSubTotal; }
        /// <summary>
        /// ワイズコア小計を設定します
        /// </summary>
        private void SetWizeCoreSubTotal()
        {
            WizeCoreSubTotal = 0;
            SetReceiptsAndExpenditure();
            foreach (ReceiptsAndExpenditure rae in WizeCoreData) { WizeCoreSubTotal += rae.Price; }
            SetTotalAmount();
        }
        /// <summary>
        /// 春秋苑小計を設定します
        /// </summary>
        private void SetShunjuenSubTotal()
        {
            ShunjuenSubTotal = 0;
            SetReceiptsAndExpenditure();
            foreach (ReceiptsAndExpenditure rae in ShunjuenData) { ShunjuenSubTotal += rae.Price; }
            SetTotalAmount();
        }
        /// <summary>
        /// 法務小計を設定します
        /// </summary>
        private void SetHoumuSubTotal()
        {
            HoumuSubTotal = 0;
            SetReceiptsAndExpenditure();
            foreach (ReceiptsAndExpenditure rae in HoumuData) { HoumuSubTotal += rae.Price; }
            SetTotalAmount();
        }
        /// <summary>
        /// 各出納データを設定します
        /// </summary>
        private void SetReceiptsAndExpenditure()
        {
            Rep rep = LoginRep.GetInstance().Rep;
            string location=AccountingProcessLocation.Location.ToString();
            CreditDept shunjuenDept = DataBaseConnect.ReferenceCreditDept("春秋苑", true, true)[0];
            CreditDept wizeCoreDept;
            
            SetShunjuenData();
            SetWizeCoreData();
            SetHoumuData();

            if (StoneWorkPartData == null)
            {
                StoneWorkPartData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                    shunjuenDept, 
                    DataBaseConnect.ReferenceContent(string.Empty, "254", "石材工事部勘定", true, true)[0], 
                    CustomerName, IntAmount(StoneWorkPartAmount), true, true, AccountActivityDate, 
                    DefaultDate, false);
            }
            else
            {
                StoneWorkPartData.AccountActivityDate = AccountActivityDate;
                StoneWorkPartData.Price = IntAmount(StoneWorkPartAmount);
                StoneWorkPartData.Detail = CustomerName;
            }
            ShunjuenData.Add(StoneWorkPartData);

            void SetHoumuData()
            {
                HoumuData = new ObservableCollection<ReceiptsAndExpenditure>();

                if (AlmsgivingData == null)
                {
                    AlmsgivingData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location,
                        shunjuenDept, 
                        DataBaseConnect.ReferenceContent("御布施", "815", string.Empty, true, true)[0],
                        CustomerName, IntAmount(AlmsgivingAmount), true, true, AccountActivityDate,
                        DefaultDate, false);
                }
                else
                {
                    AlmsgivingData.AccountActivityDate = AccountActivityDate;
                    AlmsgivingData.Price = IntAmount(AlmsgivingAmount);
                    AlmsgivingData.Detail = CustomerName;
                }
                HoumuData.Add(AlmsgivingData);

                if (CarTipData == null)
                {
                    CarTipData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                        DataBaseConnect.ReferenceContent("御車代", "832", string.Empty, true, true)[0], 
                        CustomerName, IntAmount(CarTipAmount), true, true, AccountActivityDate, DefaultDate, 
                        false);
                }
                else
                {
                    CarTipData.AccountActivityDate = AccountActivityDate;
                    CarTipData.Price = IntAmount(CarTipAmount);
                    CarTipData.Detail = CustomerName;
                }
                HoumuData.Add(CarTipData);

                if (MealTipData == null)
                {
                    MealTipData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                        shunjuenDept,
                        DataBaseConnect.ReferenceContent("御膳料", "832", string.Empty, true, true)[0], 
                        CustomerName, IntAmount(MealTipAmount), true, true, AccountActivityDate, 
                        DefaultDate, false);
                }
                else
                {
                    MealTipData.AccountActivityDate = AccountActivityDate;
                    MealTipData.Price = IntAmount(MealTipAmount);
                    MealTipData.Detail = CustomerName;
                }
                HoumuData.Add(MealTipData);

                if (CarAndMealTipData == null)
                {
                    CarAndMealTipData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                        shunjuenDept,
                        DataBaseConnect.ReferenceContent("御車代御膳料", "832", string.Empty, true, true)[0], 
                        CustomerName, IntAmount(CarAndMealTipAmount), true, true, AccountActivityDate,
                        DefaultDate, false);
                }
                else
                {
                    CarAndMealTipData.AccountActivityDate = AccountActivityDate;
                    CarAndMealTipData.Price = IntAmount(CarAndMealTipAmount);
                    CarAndMealTipData.Detail = CustomerName;
                }
                HoumuData.Add(CarAndMealTipData);
            }
            void SetShunjuenData()
            {
                ShunjuenData = new ObservableCollection<ReceiptsAndExpenditure>();

                if (BuddhistChantDonationData == null)
                {
                    BuddhistChantDonationData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location,
                        shunjuenDept,
                        DataBaseConnect.ReferenceContent(string.Empty, "831", "懇志読経料", true, true)[0],
                        CustomerName, IntAmount(BuddhistChantDonationAmount), true, true, 
                        AccountActivityDate, DefaultDate, false);
                }
                else
                {
                    BuddhistChantDonationData.AccountActivityDate = AccountActivityDate;
                    BuddhistChantDonationData.Price = IntAmount(BuddhistChantDonationAmount);
                    BuddhistChantDonationData.Detail = CustomerName;
                    BuddhistChantDonationData.Content = SelectedBuddhistChantDonationContent;
                }
                ShunjuenData.Add(BuddhistChantDonationData);

                if (NoukotuData == null)
                {
                    NoukotuData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                        shunjuenDept,
                        DataBaseConnect.ReferenceContent("納骨", "811", string.Empty, true, true)[0],
                        CustomerName, NoukotuAmount, true, true, AccountActivityDate, DefaultDate, false);
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
                    MemorialServiceDonationData = new ReceiptsAndExpenditure(0, DateTime.Today, rep,
                        location, shunjuenDept,
                        DataBaseConnect.ReferenceContent("法事冥加", "813", string.Empty, true, true)[0], 
                        string.Empty, MemorialServiceDonationAmount + ServiceFee, true, true, 
                        AccountActivityDate, DefaultDate, false);
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
                    BurningIncenseTableData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, 
                        location, shunjuenDept, 
                        DataBaseConnect.ReferenceContent("焼香台", "812", string.Empty, true, true)[0],
                        string.Empty, BurningIncenseTableAmount, true, true, AccountActivityDate, 
                        DefaultDate, false);
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
                    StupaStandData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                        shunjuenDept,
                        DataBaseConnect.ReferenceContent("簡易塔婆立", "812", string.Empty, true, true)[0], 
                        string.Empty, StupaStandAmount, true, true, AccountActivityDate, DefaultDate, false);
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
                        DataBaseConnect.ReferenceContent("パラソル", "812", string.Empty, true, true)[0], 
                        string.Empty, ParasolAmount, true, true, AccountActivityDate, DefaultDate, false);
                }
                else
                {
                    parasolData.AccountActivityDate = AccountActivityDate;
                    parasolData.Price = ParasolAmount;
                    parasolData.Detail = CustomerName;
                }
                ShunjuenData.Add(ParasolData);

                SelectedVariousChange = string.IsNullOrEmpty(SelectedVariousChange) ? 
                    string.Empty : SelectedVariousChange;
                VariousChangeData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                    shunjuenDept,
                    DataBaseConnect.ReferenceContent
                        (SelectedVariousChange, "814", string.Empty, true, true)[0],
                    CustomerName, IntAmount(VariousChangeAmount), true, true, AccountActivityDate, 
                    DefaultDate, false);
                ShunjuenData.Add(VariousChangeData);

                if (BoneEnshrinedData == null)
                {
                    BoneEnshrinedData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                        shunjuenDept,
                        DataBaseConnect.ReferenceContent("ご遺骨預かり", "817", string.Empty, true, true)[0], 
                        string.Empty, BoneEnshrinedAmount, true, true, AccountActivityDate, DefaultDate, false);
                }
                else
                {
                    BoneEnshrinedData.AccountActivityDate = AccountActivityDate;
                    BoneEnshrinedData.Price = BoneEnshrinedAmount;
                    BoneEnshrinedData.Detail = CustomerName;
                }
                ShunjuenData.Add(BoneEnshrinedData);

                if (StupaData == null)
                {
                    StupaData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, shunjuenDept,
                        DataBaseConnect.ReferenceContent("卒塔婆", "822", string.Empty, true, true)[0], 
                        string.Empty, StupaAmount, true, true, AccountActivityDate, DefaultDate, false);
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
                        DataBaseConnect.ReferenceContent("骨壺", "822", string.Empty, true, true)[0], 
                        string.Empty, IntAmount(BoneVaseAmount), true, true, AccountActivityDate, 
                        DefaultDate, false);
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
                        DataBaseConnect.ReferenceContent("供物", "871", string.Empty, true, true)[0], 
                        string.Empty, IntAmount(OfferingAmount), true, true, AccountActivityDate, DefaultDate,
                        false);
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
                    ReturnGiftData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location,
                        shunjuenDept,
                        DataBaseConnect.ReferenceContent("返礼品", "871", string.Empty, true, true)[0], 
                        string.Empty, IntAmount(ReturnGiftAmount), true, true, AccountActivityDate, 
                        DefaultDate, false);
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
                        DataBaseConnect.ReferenceContent("ブラン", "871", string.Empty, true, true)[0], 
                        string.Empty, IntAmount(BlancAmount), true, true, AccountActivityDate, DefaultDate, false);
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
                    BoneWashingData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                        shunjuenDept,
                        DataBaseConnect.ReferenceContent("洗骨料", "852", string.Empty, true, true)[0], 
                        string.Empty, IntAmount(BoneWashingAmount), true, true, AccountActivityDate, 
                        DefaultDate, false);
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
                    BoneBagData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                        shunjuenDept,
                        DataBaseConnect.ReferenceContent("納骨袋", "852", string.Empty, true, true)[0], 
                        string.Empty, IntAmount(BoneBagAmount), true, true, AccountActivityDate,
                        DefaultDate, false);
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
                    TagPlateAddingSculptureData = new ReceiptsAndExpenditure(0, DateTime.Today, rep,
                        location, shunjuenDept,
                        DataBaseConnect.ReferenceContent("銘板追加彫刻", "882", string.Empty, true, true)[0],
                        string.Empty, IntAmount(TagPlateAddingSculptureAmount), true, true, 
                        AccountActivityDate, DefaultDate, false);
                }
                else
                {
                    TagPlateAddingSculptureData.AccountActivityDate = AccountActivityDate;
                    TagPlateAddingSculptureData.Price = IntAmount(TagPlateAddingSculptureAmount);
                    TagPlateAddingSculptureData.Detail = CustomerName;
                }
                ShunjuenData.Add(TagPlateAddingSculptureData);

                GraveCleaningContents = DataBaseConnect.ReferenceContent
                    (string.Empty, "821", "墓域清掃料", true, true);
                SelectedGraveCleaningContent ??= GraveCleaningContents[0];
                GraveCleaningData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                    shunjuenDept,
                    SelectedGraveCleaningContent, CustomerName, IntAmount(GraveCleaningAmount), true, true,
                    AccountActivityDate, DefaultDate, false);
                ShunjuenData.Add(GraveCleaningData);
            }
            void SetWizeCoreData()
            {
                WizeCoreData = new ObservableCollection<ReceiptsAndExpenditure>();
                WizeCoreTransferData = new ObservableCollection<TransferReceiptsAndExpenditure>();
                wizeCoreDept = DataBaseConnect.ReferenceCreditDept("香華", true, false)[0];

                if (FloralTributeData == null)
                {
                    FloralTributeCotents = DataBaseConnect.ReferenceContent
                        (string.Empty, "812", string.Empty, false, true);
                    SelectedFloralTributeContent ??= FloralTributeCotents[0];
                    FloralTributeData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location,
                        wizeCoreDept, SelectedFloralTributeContent, CustomerName, 
                        IntAmount(FloralTributeAmount), true, true, AccountActivityDate, DefaultDate, false);
                }
                else
                {
                    FloralTributeData.AccountActivityDate = AccountActivityDate;
                    FloralTributeData.Price = IntAmount(FloralTributeAmount);
                    FloralTributeData.Detail = CustomerName;
                    FloralTributeData.Content = SelectedFloralTributeContent;
                }
                WizeCoreData.Add(FloralTributeData);

                if (CemeteryFlowerData == null)
                {
                    CemeteryFlowerData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location,
                        wizeCoreDept,
                        DataBaseConnect.ReferenceContent("墓地花", "811", string.Empty, false, true)[0],
                        CustomerName, IntAmount(CemeteryFlowerAmount), true, true, AccountActivityDate,
                        DefaultDate, false);
                }
                else
                {
                    CemeteryFlowerData.AccountActivityDate = AccountActivityDate;
                    CemeteryFlowerData.Price = IntAmount(CemeteryFlowerAmount);
                    CemeteryFlowerData.Detail = CustomerName;
                }
                WizeCoreData.Add(CemeteryFlowerData);

                if (IncenseStickData == null)
                {
                    IncenseStickData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location,
                        wizeCoreDept,
                        DataBaseConnect.ReferenceContent("線香", "813", string.Empty, false, true)[0], 
                        CustomerName, IntAmount(IncenseStickAmount), true, true, AccountActivityDate,
                        DefaultDate, false);
                }
                else
                {
                    IncenseStickData.AccountActivityDate = AccountActivityDate;
                    IncenseStickData.Price = IntAmount(IncenseStickAmount);
                    IncenseStickData.Detail = CustomerName;
                }
                WizeCoreData.Add(IncenseStickData);

                if (FlowerOfferingData == null)
                {
                    FlowerOfferingData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                        wizeCoreDept,
                        DataBaseConnect.ReferenceContent("献花", "828", string.Empty, false, true)[0],
                        CustomerName, FlowerOfferingAmount, true, true, AccountActivityDate, DefaultDate, false);
                }
                else
                {
                    FlowerOfferingData.AccountActivityDate = AccountActivityDate;
                    FlowerOfferingData.Price = FlowerOfferingAmount;
                    FlowerOfferingData.Detail = CustomerName;
                }
                WizeCoreData.Add(FlowerOfferingData);

                wizeCoreDept = DataBaseConnect.ReferenceCreditDept("春秋庵", true, false)[0];

                if (ShunjuanReturnGiftData == null)
                {
                    ShunjuanReturnGiftData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location,
                        wizeCoreDept,
                        DataBaseConnect.ReferenceContent("法事饅頭", "820", string.Empty, false, true)[0], 
                        CustomerName, IntAmount(ShunjuanReturnGiftAmount), true, true, 
                        AccountActivityDate, DefaultDate, false);
                }
                else
                {
                    ShunjuanReturnGiftData.AccountActivityDate = AccountActivityDate;
                    ShunjuanReturnGiftData.Price = IntAmount(ShunjuanReturnGiftAmount);
                    ShunjuanReturnGiftData.Detail = CustomerName;
                }
                WizeCoreData.Add(ShunjuanReturnGiftData);

                if (ShunjuanMealSalesData == null)
                {
                    ShunjuanMealSalesData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                        wizeCoreDept,
                        DataBaseConnect.ReferenceContent("売上", "815", "法事食事売上", false, true)[0], 
                        CustomerName, IntAmount(ShunjuanMealSalesAmount), true, true, AccountActivityDate,
                        DefaultDate, false);
                }
                else
                {
                    ShunjuanMealSalesData.AccountActivityDate = AccountActivityDate;
                    ShunjuanMealSalesData.Price = IntAmount(ShunjuanMealSalesAmount);
                    ShunjuanMealSalesData.Detail = CustomerName;
                }
                WizeCoreData.Add(ShunjuanMealSalesData);

                wizeCoreDept = DataBaseConnect.ReferenceCreditDept("蓮華庵", true, false)[0];

                Content content;

                if (IsSundry) 
                { 
                    content = 
                        DataBaseConnect.ReferenceContent("法事売上", "000", string.Empty, false, true)[0]; 
                }
                else 
                { 
                    content = 
                        DataBaseConnect.ReferenceContent("売上", "815", "法事食事売上", false, true)[0]; 
                }

                if (RengeanSalesData == null)
                {
                    RengeanSalesData = new ReceiptsAndExpenditure(0, DateTime.Today, rep, location, 
                        wizeCoreDept, content, CustomerName, IntAmount(RengeanSalesAmount), true, true, 
                        AccountActivityDate, DefaultDate, false);
                }
                else
                {
                    RengeanSalesData.AccountActivityDate = AccountActivityDate;
                    RengeanSalesData.Price = IntAmount(RengeanSalesAmount);
                    RengeanSalesData.Detail = CustomerName;
                    RengeanSalesData.Content = content;
                    CallPropertyChanged(nameof(RengeanSalesData));
                }
                WizeCoreData.Add(RengeanSalesData);

                if (RengeanMealSalesData == null)
                {
                    RengeanMealSalesData = new TransferReceiptsAndExpenditure(0, DateTime.Today, rep,
                        location, wizeCoreDept,
                        DataBaseConnect.ReferenceAccountingSubject("000", string.Empty, false, true)[0],
                        DataBaseConnect.ReferenceAccountingSubject("815", "法事食事売上", false, true)[0],
                        content.Text, CustomerName, IntAmount(RengeanMealSalesAmount), true,
                        AccountActivityDate, DefaultDate, false);
                }
                else
                {
                    RengeanMealSalesData.AccountActivityDate = AccountActivityDate;
                    RengeanMealSalesData.Price = IntAmount(RengeanMealSalesAmount);
                    RengeanMealSalesData.Detail = CustomerName;
                }
                WizeCoreTransferData.Add(RengeanMealSalesData);

                if (RengeanBeverageSalesData == null)
                {
                    RengeanBeverageSalesData = new TransferReceiptsAndExpenditure(0, DateTime.Today, rep,
                        location, wizeCoreDept, 
                        DataBaseConnect.ReferenceAccountingSubject("000", string.Empty, false, true)[0],
                        DataBaseConnect.ReferenceAccountingSubject("815", "法事飲料売上", false, true)[0], 
                        content.Text, CustomerName, IntAmount(RengeanBeverageSalesAmount), true, 
                        AccountActivityDate, DefaultDate, false);
                }
                else
                {
                    RengeanBeverageSalesData.AccountActivityDate = AccountActivityDate;
                    RengeanBeverageSalesData.Price = IntAmount(RengeanBeverageSalesAmount);
                    RengeanBeverageSalesData.Detail = CustomerName;
                }
                WizeCoreTransferData.Add(RengeanBeverageSalesData);
            }
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
                ValidationProperty(nameof(CustomerName), value);
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
                MemorialServiceDonationAmount = value.Key == "青蓮堂" ? 
                    ReturnShorendoAmount() : value.Value;
                MemorialServiceDonationData.Price = MemorialServiceDonationAmount + ServiceFee;

                int ReturnShorendoAmount()
                { return IsIndoorCemeteryCineration ? 20000 : value.Value; }
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

                VariousChangeData = new ReceiptsAndExpenditure(0, DateTime.Today, 
                    LoginRep.GetInstance().Rep,  AccountingProcessLocation.Location.ToString(), creditDept,
                    content, CustomerName, 0, true, true,  AccountActivityDate, DefaultDate, false);
            }
        }
        /// <summary>
        /// 表示用各種変更冥加金額
        /// </summary>
        public string VariousChangeAmount
        {
            get => variousChangeAmount;
            set
            {
                variousChangeAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                ValidationProperty(nameof(VariousChangeAmount), value);
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
                returnGiftAmount = CommaDelimitedAmount(value);
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
                blancAmount = CommaDelimitedAmount(value);
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
                boneWashingAmount = CommaDelimitedAmount(value);
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
                boneBagAmount = CommaDelimitedAmount(value);
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
                ValidationProperty(nameof(GraveCleaningAmount), value);
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
        /// <summary>
        /// ワイズコア小計
        /// </summary>
        public int WizeCoreSubTotal
        {
            get => wizeCoreSubTotal;
            set
            {
                wizeCoreSubTotal = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 供花
        /// </summary>
        public ReceiptsAndExpenditure FloralTributeData
        {
            get => floralTributeData;
            set
            {
                floralTributeData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 供花金額
        /// </summary>
        public string FloralTributeAmount
        {
            get => floralTributeAmount;
            set
            {
                floralTributeAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetWizeCoreSubTotal();
            }
        }
        /// <summary>
        /// 墓地花
        /// </summary>
        public ReceiptsAndExpenditure CemeteryFlowerData
        {
            get => cemeteryFlowerData;
            set
            {
                cemeteryFlowerData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 墓地花金額
        /// </summary>
        public string CemeteryFlowerAmount
        {
            get => cemeteryFlowerAmount;
            set
            {
                cemeteryFlowerAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetWizeCoreSubTotal();
            }
        }
        /// <summary>
        /// 線香
        /// </summary>
        public ReceiptsAndExpenditure IncenseStickData
        {
            get => incenseStickData;
            set
            {
                incenseStickData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 線香金額
        /// </summary>
        public string IncenseStickAmount
        {
            get => incenseStickAmount;
            set
            {
                incenseStickAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetWizeCoreSubTotal();
            }
        }
        /// <summary>
        /// 献花
        /// </summary>
        public ReceiptsAndExpenditure FlowerOfferingData
        {
            get => flowerOfferingData;
            set
            {
                flowerOfferingData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 献花金額
        /// </summary>
        public int FlowerOfferingAmount
        {
            get => flowerOfferingAmount;
            set
            {
                flowerOfferingAmount = value;
                CallPropertyChanged();
                SetWizeCoreSubTotal();
            }
        }
        /// <summary>
        /// 献花単価
        /// </summary>
        public string FlowerOfferingFlatRate
        {
            get => flowerOfferingFlatRate;
            set
            {
                flowerOfferingFlatRate = CommaDelimitedAmount(value);
                CallPropertyChanged();
                FlowerOfferingAmount = IntAmount(FlowerOfferingFlatRate) * FlowerOfferingCount;
            }
        }
        /// <summary>
        /// 献花本数
        /// </summary>
        public int FlowerOfferingCount
        {
            get => flowerOfferingCount;
            set
            {
                flowerOfferingCount = value;
                CallPropertyChanged();
                FlowerOfferingAmount = IntAmount(FlowerOfferingFlatRate) * value;
            }
        }
        /// <summary>
        /// 春秋庵返礼品
        /// </summary>
        public ReceiptsAndExpenditure ShunjuanReturnGiftData
        {
            get => shunjuanReturnGiftData;
            set
            {
                shunjuanReturnGiftData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 春秋庵返礼品金額
        /// </summary>
        public string ShunjuanReturnGiftAmount
        {
            get => shunjuanReturnGiftAmount;
            set
            {
                shunjuanReturnGiftAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetWizeCoreSubTotal();
            }
        }
        /// <summary>
        /// 春秋庵食事売上
        /// </summary>
        public ReceiptsAndExpenditure ShunjuanMealSalesData
        {
            get => shunjuanMealSalesData;
            set
            {
                shunjuanMealSalesData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 春秋庵食事売上金額
        /// </summary>
        public string ShunjuanMealSalesAmount
        {
            get => shunjuanMealSalesAmount;
            set
            {
                shunjuanMealSalesAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetWizeCoreSubTotal();
            }
        }
        /// <summary>
        /// 蓮華庵売上
        /// </summary>
        public ReceiptsAndExpenditure RengeanSalesData
        {
            get => rengeanSalesData;
            set
            {
                rengeanSalesData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 蓮華庵売上金額
        /// </summary>
        public string RengeanSalesAmount
        {
            get => rengeanSalesAmount;
            set
            {
                rengeanSalesAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetWizeCoreSubTotal();
            }
        }
        /// <summary>
        /// 諸口チェック
        /// </summary>
        public bool IsSundry
        {
            get => isSundry;
            set
            {
                isSundry = value;
                CallPropertyChanged();
                if (value) { RengeanMealSalesAmount = RengeanSalesAmount; }
                else 
                {
                    RengeanMealSalesAmount = "0";
                    RengeanBeverageSalesAmount = "0";
                }
                SetWizeCoreSubTotal();
            }
        }
        /// <summary>
        /// 蓮華庵食事売上金額
        /// </summary>
        public string RengeanMealSalesAmount
        {
            get => rengeanMealSalesAmount;
            set
            {
                rengeanMealSalesAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetWizeCoreSubTotal();
                RengeanBeverageSalesAmount =
                    (IntAmount(RengeanSalesAmount) - IntAmount(value)).ToString();
            }
        }
        /// <summary>
        /// 蓮華庵食事売上
        /// </summary>
        public TransferReceiptsAndExpenditure RengeanMealSalesData
        {
            get => rengeanMealSalesData;
            set
            {
                rengeanMealSalesData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 蓮華庵飲料売上
        /// </summary>
        public TransferReceiptsAndExpenditure RengeanBeverageSalesData
        {
            get => rengeanBeverageSalesData;
            set
            {
                rengeanBeverageSalesData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 蓮華庵飲料売上金額
        /// </summary>
        public string RengeanBeverageSalesAmount
        {
            get => rengeanBeverageSalesAmount;
            set
            {
                rengeanBeverageSalesAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetWizeCoreSubTotal();
            }
        }
        /// <summary>
        /// 懇志読経料
        /// </summary>
        public ReceiptsAndExpenditure BuddhistChantDonationData
        {
            get => buddhistChantDonationData;
            set
            {
                buddhistChantDonationData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 懇志読経料出納内容リスト
        /// </summary>
        public ObservableCollection<Content> BuddhistChantDonationContents
        {
            get => buddhistChantDonationContents;
            set
            {
                buddhistChantDonationContents = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 懇志読経料金額
        /// </summary>
        public string BuddhistChantDonationAmount
        {
            get => buddhistChantDonationAmount;
            set
            {
                buddhistChantDonationAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                ValidationProperty(nameof(BuddhistChantDonationAmount), value);
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 御布施
        /// </summary>
        public ReceiptsAndExpenditure AlmsgivingData
        {
            get => almsgivingData;
            set
            {
                almsgivingData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 御布施金額
        /// </summary>
        public string AlmsgivingAmount
        {
            get => almsgivingAmount;
            set
            {
                almsgivingAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetHoumuSubTotal();
            }
        }
        /// <summary>
        /// 法務小計
        /// </summary>
        public int HoumuSubTotal
        {
            get => houmuSubTotal;
            set
            {
                houmuSubTotal = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 御車代
        /// </summary>
        public ReceiptsAndExpenditure CarTipData
        {
            get => carTipData; set
            {
                carTipData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 御車代金額
        /// </summary>
        public string CarTipAmount
        {
            get => carTipAmount;
            set
            {
                carTipAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetHoumuSubTotal();
            }
        }
        /// <summary>
        /// 御膳料
        /// </summary>
        public ReceiptsAndExpenditure MealTipData
        {
            get => mealTipData;
            set
            {
                mealTipData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 御膳料金額
        /// </summary>
        public string MealTipAmount
        {
            get => mealTipAmount;
            set
            {
                mealTipAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetHoumuSubTotal();
            }
        }
        /// <summary>
        /// 御車代御膳料
        /// </summary>
        public ReceiptsAndExpenditure CarAndMealTipData
        {
            get => carAndMealTipData;
            set
            {
                carAndMealTipData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 御車代御膳料金額
        /// </summary>
        public string CarAndMealTipAmount
        {
            get => carAndMealTipAmount;
            set
            {
                carAndMealTipAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetHoumuSubTotal();
            }
        }
        /// <summary>
        /// 石材工事部
        /// </summary>
        public ReceiptsAndExpenditure StoneWorkPartData
        {
            get => stoneWorkPartData;
            set
            {
                stoneWorkPartData = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 石材工事部金額
        /// </summary>
        public string StoneWorkPartAmount
        {
            get => stoneWorkPartAmount;
            set
            {
                stoneWorkPartAmount = CommaDelimitedAmount(value);
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 石材工事部伝票内容リスト
        /// </summary>
        public ObservableCollection<Content> StoneWorkPartContents
        {
            get => stoneWorkPartContents;
            set
            {
                stoneWorkPartContents = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された石材工事部伝票内容
        /// </summary>
        public Content SelectedStoneWorkPartContent
        {
            get => selectedStoneWorkPartContent;
            set
            {
                selectedStoneWorkPartContent = value;
                CallPropertyChanged();
                int i = default;
                if (value.FlatRate != -1) { i = value.FlatRate; }
                StoneWorkPartAmount = CommaDelimitedAmount(i);
            }
        }
        /// <summary>
        /// 石材工事部勘定を入力するか
        /// </summary>
        public bool IsStoneWorkPartInput
        {
            get => isStoneWorkPartInput;
            set
            {
                isStoneWorkPartInput = value;
                CallPropertyChanged();
                if (value) { SelectedStoneWorkPartContent = StoneWorkPartContents[0]; }
                else { StoneWorkPartAmount = "0"; }
            }
        }
        /// <summary>
        /// 総計
        /// </summary>
        public int TotalAmount
        {
            get => totalAmount;
            set
            {
                totalAmount = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// ウィンドウを閉じるのをキャンセルするか
        /// </summary>
        public bool IsCloseCancel
        {
            get => isCloseCancel;
            set
            {
                isCloseCancel = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 登録操作を許可するか
        /// </summary>
        public bool CanOperation
        {
            get => canOperation;
            set
            {
                canOperation = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された懇志読経料伝票内容
        /// </summary>
        public Content SelectedBuddhistChantDonationContent
        {
            get => selectedBuddhistChantDonationContent;
            set
            {
                selectedBuddhistChantDonationContent = value;
                CallPropertyChanged();
                SetShunjuenSubTotal();
            }
        }
        /// <summary>
        /// 供花売上伝票内容リスト
        /// </summary>
        public ObservableCollection<Content> FloralTributeCotents
        {
            get => floralTributeCotents;
            set
            {
                floralTributeCotents = value;
                CallPropertyChanged();
            }
        }
        /// <summary>
        /// 選択された供花売上伝票内容
        /// </summary>
        public Content SelectedFloralTributeContent
        {
            get => selectedFloralTributeContent;
            set
            {
                selectedFloralTributeContent = value;
                CallPropertyChanged();
                SetWizeCoreSubTotal();
            }
        }
        /// <summary>
        /// 屋内御廟納骨か
        /// </summary>
        public bool IsIndoorCemeteryCineration
        {
            get => isIndoorCemeteryCineration;
            set
            {
                isIndoorCemeteryCineration = value;
                CallPropertyChanged();

                MemorialServiceDonationAmount = value ? ReturnAmount() : SelectedPlace.Value;

                int ReturnAmount() => SelectedPlace.Key == "青蓮堂" ? 20000 : SelectedPlace.Value;
            }
        }

        public override void ValidationProperty(string propertyName, object value)
        {
            bool b;

            switch (propertyName)
            {
                case nameof(BuddhistChantDonationAmount):
                    VerificationAmountToContent(SelectedBuddhistChantDonationContent);
                    break;
                case nameof(VariousChangeAmount):
                    b = IntAmount((string)value) > 0;
                    if (b)
                    { 
                        ErrorsListOperation
                            (string.IsNullOrEmpty(SelectedVariousChange), propertyName,
                                "伝票内容を選択してください"); 
                    }
                    else { ErrorsListOperation(false, propertyName, string.Empty); }
                    break;
                case nameof(GraveCleaningAmount):
                    VerificationAmountToContent(SelectedGraveCleaningContent);
                    break;
                case nameof(CustomerName):
                    SetNullOrEmptyError(propertyName, value);
                    break;
            }

            CanOperation = !HasErrors & TotalAmount > 0;

            void VerificationAmountToContent(Content content)
            {
                b = IntAmount((string)value) > 0;
                if (b) { ErrorsListOperation(content == null, propertyName, "伝票内容を選択してください"); }
                else { ErrorsListOperation(false, propertyName, string.Empty); }
            }
        }

        protected override void SetWindowDefaultTitle()
        { DefaultWindowTitle = $"法事計算書登録 : {AccountingProcessLocation.Location}"; }

        public bool CancelClose() => IsCloseCancel;
    }
}
