using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using static Domain.Entities.Helpers.TextHelper;

namespace Infrastructure
{
    /// <summary>
    /// SQLServer接続クラス
    /// </summary>
    public class SQLServerConnectInfrastructure : IDataBaseConnect
    {
        private SqlConnection Cn;
        private readonly LoginRep LoginRep = LoginRep.GetInstance();
        private readonly ILogger Logger;

        public SQLServerConnectInfrastructure(ILogger logger)
        {
            Logger = logger;
        }
        public SQLServerConnectInfrastructure() : this(DefaultInfrastructure.GetLogger()) { }
        /// <summary>
        /// コネクションストリングを設定します。
        /// </summary>
        private void SettingConectionString()
        {
            Cn = new SqlConnection
            {
                ConnectionString = Properties.Settings.Default.SystemAdminConnection
                //ConnectionString = LoginRep.Rep.IsAdminPermisson
                //? Properties.Settings.Default.SystemAdminConnection
                //: Properties.Settings.Default.AccountingProcessConnection
            };
        }
        /// <summary>
        /// sqlを実行するコマンドを生成します
        /// </summary>
        /// <param name="commandText"></param>
        private SqlCommand NewCommand(CommandType commandType,string commandText)
        {
            SettingConectionString();

            SqlCommand Cmd = new SqlCommand()
            {
                Connection = Cn,
                CommandType = commandType,
                CommandText = commandText
            };

            try
            {
                Cn.Open();
            }
            catch (Exception ex)
            {
                Logger.Log(ILogger.LogInfomation.ERROR, ex.Message);
            }
            return Cmd;
        }
        /// ストアドを実行するコマンドを返します
        /// </summary>
        /// <param name="commandText">ストアドプロシージャ名</param>
        /// <param name="parameterName">パラメータ名</param>
        /// <param name="parameter">パラメータ</param>
        /// <returns></returns>
        private SqlCommand ReturnGeneretedParameterCommand
            (string commandText, Dictionary<string, object> parameters)
        {
            SqlCommand Cmd;

            using (Cn)
            {
                Cmd = NewCommand(CommandType.StoredProcedure, commandText);

                foreach (KeyValuePair<string, object> param in parameters)
                    Cmd.Parameters.AddWithValue(param.Key, param.Value);
            }

            return Cmd;
        }

        private SqlCommand ExecuteNoParameterStoredProc(string commandText)
        {
            SqlCommand Cmd;

            using (Cn) Cmd = NewCommand(CommandType.StoredProcedure, commandText);

            return Cmd;
        }

        public int Registration(Rep rep)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@staff_name", rep.Name },
                { "@password", rep.Password},
                { "@validity", rep.IsValidity},
                {"@is_permission", rep.IsAdminPermisson }
            };

            return ReturnGeneretedParameterCommand("registration_staff", parameters).ExecuteNonQuery();
        }

        public int Update(Rep rep)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@staff_id", rep.ID },{"@staff_name", rep.Name },{"@password", rep.Password },
                {"@is_validity", rep.IsValidity },{"@is_permission", rep.IsAdminPermisson },
                {"@operation_staff_id", LoginRep.Rep.ID }
            };

            return ReturnGeneretedParameterCommand
                ("update_staff", parameters).ExecuteNonQuery();
        }
       
        public ObservableCollection<Rep> ReferenceRep(string repName,bool isValidityTrueOnly)
        {
            ObservableCollection<Rep> reps = new ObservableCollection<Rep>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {{ "@staff_name", repName},{ "@true_only", isValidityTrueOnly}};

            SqlDataReader DataReader = ReturnGeneretedParameterCommand
                ("reference_staff",parameters).ExecuteReader();
            while (DataReader.Read())
                reps.Add
                    (new Rep((string)DataReader["staff_id"], (string)DataReader["name"], 
                    (string)DataReader["password"], (bool)DataReader["is_validity"], 
                    (bool)DataReader["is_permission"]));

            return reps;
        }
     
        public int Registration(AccountingSubject accountingSubject)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@subject_code", accountingSubject.SubjectCode},{"@subject", accountingSubject.Subject },
                { "@validity", accountingSubject.IsValidity},{"@staff_id", LoginRep.Rep.ID}
            };

            return ReturnGeneretedParameterCommand
                ("registration_accounting_subject", parameters).ExecuteNonQuery();
        }

        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject
            (string subjectCode, string subject, bool isTrueOnly)
        {
            ObservableCollection<AccountingSubject> accountingSubjects = 
                new ObservableCollection<AccountingSubject>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            { {"@subject_code", subjectCode},{"@subject", subject},{"@true_only", isTrueOnly} };

            SqlDataReader DataReader = ReturnGeneretedParameterCommand
                    ("reference_accounting_subject", parameters).ExecuteReader();
            
            while (DataReader.Read())
                accountingSubjects.Add
                    (new AccountingSubject((string)DataReader["accounting_subject_id"], 
                    (string)DataReader["subject_code"], (string)DataReader["subject"], 
                    (bool)DataReader["is_validity"]));
            
            return accountingSubjects;
        }
            
        public int Update(AccountingSubject accountingSubject)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@accounting_subject_id", accountingSubject.ID},{ "@subject_code",accountingSubject.SubjectCode},
                { "@subject",accountingSubject.Subject},{"@is_validity", accountingSubject.IsValidity},
                { "@operation_staff_id", LoginRep.Rep.ID}
            };

            return ReturnGeneretedParameterCommand
                ("update_accounting_subject", parameters).ExecuteNonQuery();
        }
  
        public int Registration(CreditDept creditDept)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@account", creditDept.Dept },{"@is_validity", creditDept.IsValidity},
                {"@staff_id", LoginRep.Rep.ID},{ "@is_shunjuen_account", creditDept.IsShunjuenAccount}
            };

            return ReturnGeneretedParameterCommand
                ("registration_credit_dept", parameters).ExecuteNonQuery();
        }

        public ObservableCollection<CreditDept> ReferenceCreditDept
            (string account, bool isValidityTrueOnly, bool isShunjuenAccountOnly)
        {
            ObservableCollection<CreditDept> creditDepts = new ObservableCollection<CreditDept>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@dept", account },{ "@true_only", isValidityTrueOnly},
                {"@shunjuen_account_only", isShunjuenAccountOnly }
            };

            SqlDataReader DataReader = ReturnGeneretedParameterCommand
                ("reference_credit_dept", parameters).ExecuteReader();

            while (DataReader.Read()) creditDepts.Add
                    (new CreditDept((string)DataReader["credit_dept_id"], (string)DataReader["dept"],
                    (bool)DataReader["is_validity"], isShunjuenAccountOnly));

            return creditDepts;
        }
     
        public int Update(CreditDept creditDept)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@credit_dept_id", creditDept.ID},{ "@is_validity", creditDept.IsValidity},
                {"@operation_staff_id", LoginRep.Rep.ID }
            };

            return ReturnGeneretedParameterCommand
                ("update_credit_dept", parameters).ExecuteNonQuery();
        }
   
        public int Registration(Content content)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@account_subject_id", content.AccountingSubject.ID},{"@content", content.Text},
                {"@flat_rate", content.FlatRate},{"@is_validity", content.IsValidity },
                {"@staff_id", LoginRep.Rep.ID}
            };

            return ReturnGeneretedParameterCommand
                ("registration_content", parameters).ExecuteNonQuery();
        }
   
        public int Update(Content content)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@content_id", content.ID},{"@content", content.Text},{"@flat_rate", content.FlatRate},
                {"@is_validity", content.IsValidity },{"@operation_staff_id", LoginRep.Rep.ID}
            };

            return ReturnGeneretedParameterCommand("update_content", parameters).ExecuteNonQuery();
        }
    
        public ObservableCollection<Content> ReferenceContent
            (string contentText, string accountingSubjectCode, string accountingSubject, bool isValidityTrueOnly)
        {
            ObservableCollection<Content> contents = new ObservableCollection<Content>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@content", contentText},{"@subject_code", accountingSubjectCode},
                {"@subject", accountingSubject},{ "@true_only", isValidityTrueOnly}
            };

            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("reference_content", parameters).ExecuteReader();

            while(dataReader.Read())
            {
                contents.Add
                    (
                        new Content((string)dataReader["content_id"],
                        new AccountingSubject((string)dataReader["accounting_subject_id"],
                            (string)dataReader["subject_code"], (string)dataReader["subject"], true),
                        (int)dataReader["flat_rate"], (string)dataReader["content"], (bool)dataReader["is_validity"])
                    );
            }
            return contents;
        }

        public AccountingSubject CallAccountingSubject(string id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            { { "@accounting_subject_id", id} };
            SqlDataReader DataReader = ReturnGeneretedParameterCommand
                    ("call_accounting_subject",parameters ).ExecuteReader();
            
            while (DataReader.Read())
                return new AccountingSubject
                    ((string)DataReader["accounting_subject_id"],
                    (string)DataReader["subject_code"], (string)DataReader["subject"],
                    (bool)DataReader["is_validity"]);
            
            return null;
        }

        public ObservableCollection<AccountingSubject> ReferenceAffiliationAccountingSubject
            (string contentText)
        {
            ObservableCollection<AccountingSubject> list = new ObservableCollection<AccountingSubject>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            { {"@content", contentText}};
            using SqlDataReader DataReader =
                ReturnGeneretedParameterCommand
                    ("reference_affiliation_accounting_subject", parameters).ExecuteReader();
            
            while (DataReader.Read()) list.Add
                    (CallAccountingSubject((string)DataReader["accounting_subject_id"]));
            
            return list;
        }

        public int Registration(ReceiptsAndExpenditure receiptsAndExpenditure)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@location", receiptsAndExpenditure.Location},
                {"@account_activity_date", receiptsAndExpenditure.AccountActivityDate},
                { "@registration_date", receiptsAndExpenditure.RegistrationDate},
                { "@registration_staff_id", receiptsAndExpenditure.RegistrationRep.ID},
                {"@credit_dept_id", receiptsAndExpenditure.CreditDept.ID },
                { "@content_id", receiptsAndExpenditure.Content.ID},{"@detail", receiptsAndExpenditure.Detail},
                { "@price", receiptsAndExpenditure.Price},{"@is_payment", receiptsAndExpenditure.IsPayment},
                { "@is_validity", receiptsAndExpenditure.IsValidity},
                {"@is_reduced_tax_rate", receiptsAndExpenditure.IsReducedTaxRate}
            };

            return ReturnGeneretedParameterCommand
                ("registration_receipts_and_expenditure", parameters).ExecuteNonQuery();
        }

        private ObservableCollection<ReceiptsAndExpenditure> ReferenceReceiptsAndExpenditure
            (Dictionary<string, object> parameters)
        {
            ObservableCollection<ReceiptsAndExpenditure> list =
                new ObservableCollection<ReceiptsAndExpenditure>();
            Rep paramRep;
            CreditDept paramCreditDept;
            AccountingSubject paramAccountingSubject;
            Content paramContent;

            using SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("reference_receipts_and_expenditure_all_data", parameters).ExecuteReader();

            while (dataReader.Read())
            {
                paramRep =
                    new Rep((string)dataReader["staff_id"], (string)dataReader["name"],
                        (string)dataReader["password"], true, (bool)dataReader["is_permission"]);
                paramCreditDept =
                    new CreditDept((string)dataReader["credit_dept_id"], (string)dataReader["dept"], true,
                    (bool)dataReader["is_shunjuen_dept"]);
                paramAccountingSubject =
                    new AccountingSubject((string)dataReader["accounting_subject_id"],
                    (string)dataReader["subject_code"], (string)dataReader["subject"], true);
                paramContent = new Content((string)dataReader["content_id"], paramAccountingSubject,
                    (int)dataReader["flat_rate"], (string)dataReader["content"], true);
                list.Add(new ReceiptsAndExpenditure
                    (
                    (int)dataReader["receipts_and_expenditure_id"], (DateTime)dataReader["registration_date"],
                    paramRep, (string)dataReader["location"], paramCreditDept, paramContent,
                    (string)dataReader["detail"], (int)dataReader["price"], (bool)dataReader["is_payment"],
                    (bool)dataReader["is_validity"], (DateTime)dataReader["account_activity_date"],
                    (DateTime)dataReader["output_date"], (bool)dataReader["is_reduced_tax_rate"])
                    );
            }
            return list;
        }

        public  ObservableCollection<ReceiptsAndExpenditure> ReferenceReceiptsAndExpenditure
            (
                DateTime registrationDateStart, DateTime registrationDateEnd, string location,
                string creditDept,string content,string detail, string accountingSubject, 
                string accountingSubjectCode, bool whichDepositAndWithdrawalOnly, bool isPayment,
                bool isContainOutputted, bool isValidityOnly, DateTime accountActivityDateStart,
                DateTime accountActivityDateEnd, DateTime outputDateStart, DateTime outputDateEnd
            )
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@location", location},{"@account_activity_date_start", accountActivityDateStart},
                { "@account_activity_date_end", accountActivityDateEnd},
                {"@registration_date_start", registrationDateStart},{"@registration_date_end", registrationDateEnd},
                { "@credit_dept", creditDept},{"@accounting_subject_code", accountingSubjectCode},
                { "@accounting_subject", accountingSubject }, {"@content", content},{"@detail", detail},
                {"@limiting_is_payment", whichDepositAndWithdrawalOnly}, {"@is_payment", isPayment },
                {"@contain_outputted", isContainOutputted}, {"@validity_true_only", isValidityOnly},
                {"@output_date_start", outputDateStart}, { "@output_date_end", outputDateEnd}
            };
            
            return ReferenceReceiptsAndExpenditure(parameters);
        }

        public Rep CallRep(string id)
        {
            Rep rep = default;

            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("call_staff", new Dictionary<string, object>() { { "@staff_id", id } }).ExecuteReader();
            
            while(dataReader.Read())
                rep =
                    new Rep((string)dataReader["staff_id"], (string)dataReader["name"],
                    (string)dataReader["password"], (bool)dataReader["is_validity"],
                    (bool)dataReader["is_permission"]);
            
            return rep;
        }

        public CreditDept CallCreditDept(string id)
        {
            CreditDept creditDept = default;

            SqlDataReader dataReader =
                ReturnGeneretedParameterCommand
                    ("call_credit_dept",new Dictionary<string, object>() { { "@credit_dept_id", id } })
                        .ExecuteReader();
            
            while (dataReader.Read())
                creditDept = new CreditDept((string)dataReader["credit_dept_id"], (string)dataReader["dept"],
                    (bool)dataReader["is_validity"], (bool)dataReader["is_shunjuen_dept"]);

            return creditDept;
        }

        public Content CallContent(string id)
        {
            Content content = default;

            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("call_content", new Dictionary<string, object>() { { "@content_id", id } }).ExecuteReader();
            
            while (dataReader.Read())
                content = new Content
                    (
                        (string)dataReader["content_id"],
                        new AccountingSubject((string)dataReader["accounting_subject_id"], 
                        (string)dataReader["subject_code"], (string)dataReader["subject"], true),
                        (int)dataReader["flat_rate"], (string)dataReader["content"],
                        (bool)dataReader["is_validity"]
                    );

            return content;
        }

        public int Update(ReceiptsAndExpenditure receiptsAndExpenditure)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@receipts_and_expenditure_id", receiptsAndExpenditure.ID},
                { "@location", receiptsAndExpenditure.Location},
                {"@account_activity_date", receiptsAndExpenditure.AccountActivityDate },
                { "@credit_dept_id", receiptsAndExpenditure.CreditDept.ID},
                { "@content_id", receiptsAndExpenditure.Content.ID},
                { "@detail", receiptsAndExpenditure.Detail},{ "@price", receiptsAndExpenditure.Price},
                { "@is_payment", receiptsAndExpenditure.IsPayment},
                { "@is_validity", receiptsAndExpenditure.IsValidity},
                {"@is_unprinted", receiptsAndExpenditure.IsUnprinted},
                {"@operation_staff_id", LoginRep.Rep.ID},
                { "@is_reduced_tax_rate", receiptsAndExpenditure.IsReducedTaxRate}
            };

            return ReturnGeneretedParameterCommand
                ("update_receipts_and_expenditure", parameters).ExecuteNonQuery();
        }

        public int PreviousDayFinalAmount()
        {
            SettingConectionString();
            SqlCommand Cmd = new SqlCommand("select dbo.return_previous_day_final_amount()", Cn);
            Cn.Open();
            object obj;
            using (Cn) obj = Cmd.ExecuteScalar();

            return (int)obj;
        }

        public int RegistrationPrecedingYearFinalAccount()
        {
            SqlCommand Cmd;

            using(Cn)
            {
                Cmd = NewCommand(CommandType.Text, "registration_preceding_year_final_account_table");
                return Cmd.ExecuteNonQuery();
            }
        }

        public int CallFinalAccountPerMonth()
        {
            int i = default;
            DateTime previousMonthLastDay =
                DateTime.Today.AddDays(-1 * (DateTime.Today.Day - 1)).AddDays(-1);

            SqlDataReader sdr = ReturnGeneretedParameterCommand
                ("call_final_account_per_month",
                    new Dictionary<string, object>() { { "@date", previousMonthLastDay } })
                    .ExecuteReader();

            while (sdr.Read()) i = (int)sdr["amount"];

            return i;
        }

        public int CallFinalAccountPerMonth(DateTime date)
        {
            SettingConectionString();
            SqlCommand Cmd = new SqlCommand("select dbo.call_previous_month_final_account(@date)", Cn);
            Cmd.Parameters.AddWithValue("@date", date);
            Cn.Open();

            object obj;
            using (Cn) obj = Cmd.ExecuteScalar( );

            return (int)obj;
        }

        public int ReceiptsAndExpenditurePreviousDayChange
            (ReceiptsAndExpenditure receiptsAndExpenditure) =>
                NewCommand(CommandType.Text,
                    $"select * from receipts_and_expenditure_data with(tablockx) begin tran " +
                    $"update receipts_and_expenditure_data " +
                    $"set output_date='{DateTime.Now.AddDays(-1) }'" +
                    $"where receipts_and_expenditure_id='{receiptsAndExpenditure.ID}' commit tran")
                .ExecuteNonQuery();

        public (int TotalRows, ObservableCollection<ReceiptsAndExpenditure> List)
            ReferenceReceiptsAndExpenditure(DateTime registrationDateStart,
                DateTime registrationDateEnd, string location, string creditDept, string content, string detail, 
                string accountingSubject, string accountingSubjectCode, bool whichDepositAndWithdrawalOnly, 
                bool isPayment, bool isContainOutputted, bool isValidityOnly, DateTime accountActivityDateStart, 
                DateTime accountActivityDateEnd, DateTime outputDateStart, DateTime outputDateEnd, 
                int pageCount, string sortColumn, bool sortDirection)
        {
            ObservableCollection<ReceiptsAndExpenditure> list =
                new ObservableCollection<ReceiptsAndExpenditure>();

            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@location", location},{"@account_activity_date_start", accountActivityDateStart},
                { "@account_activity_date_end", accountActivityDateEnd},
                {"@registration_date_start", registrationDateStart},{"@registration_date_end", registrationDateEnd},
                { "@credit_dept", creditDept},{"@accounting_subject_code", accountingSubjectCode},
                { "@accounting_subject", accountingSubject }, {"@content", content},{"@detail", detail},
                {"@limiting_is_payment", whichDepositAndWithdrawalOnly}, {"@is_payment", isPayment },
                {"@contain_outputted", isContainOutputted}, {"@validity_true_only", isValidityOnly},
                {"@output_date_start", outputDateStart}, { "@output_date_end", outputDateEnd},{"@page", pageCount},
                {"@column",sortColumn}, { "@is_order_asc",sortDirection}
            };

            Rep paramRep;
            CreditDept paramCreditDept;
            AccountingSubject paramAccountingSubject;
            Content paramContent;
            SqlDataReader dataReader =
                ReturnGeneretedParameterCommand
                    ("reference_receipts_and_expenditure", parameters).ExecuteReader();
            
            while (dataReader.Read())
            {
                paramRep =
                    new Rep((string)dataReader["staff_id"], (string)dataReader["name"],
                    (string)dataReader["password"], true, (bool)dataReader["is_permission"]);
                paramCreditDept =
                    new CreditDept((string)dataReader["credit_dept_id"], (string)dataReader["dept"], true,
                    (bool)dataReader["is_shunjuen_dept"]);
                paramAccountingSubject =
                    new AccountingSubject((string)dataReader["accounting_subject_id"],
                    (string)dataReader["subject_code"], (string)dataReader["subject"], true);
                paramContent = new Content((string)dataReader["content_id"], paramAccountingSubject,
                    (int)dataReader["flat_rate"], (string)dataReader["content"], true);
                list.Add(new ReceiptsAndExpenditure
                    (
                        (int)dataReader["receipts_and_expenditure_id"],
                        (DateTime)dataReader["registration_date"], paramRep, (string)dataReader["location"],
                        paramCreditDept, paramContent, (string)dataReader["detail"], (int)dataReader["price"],
                        (bool)dataReader["is_payment"], (bool)dataReader["is_validity"],
                        (DateTime)dataReader["account_activity_date"], (DateTime)dataReader["output_date"],
                        (bool)dataReader["is_reduced_tax_rate"])
                    );
            }
            parameters.Remove("@page");
            parameters.Remove("@column");
            parameters.Remove("@is_order_asc");

            return (ReferenceReceiptsAndExpenditure(parameters).Count, list);
        }

        public Dictionary<int, string> GetSoryoList()
        {
            Cn = new SqlConnection
            {
                ConnectionString =
                @"Data Source=192.168.44.163\SQLEXPRESS;Initial Catalog=SingyoujiDataBase;" +
                    "User ID=sa;Password=sqlserver"
            };

            Cn.Open();

            SqlCommand Cmd = new SqlCommand
            {
                Connection = Cn,
                CommandType = CommandType.Text,
                CommandText = "select * from PersonInChargeMaster where OrderNumber>0"
            };

            Dictionary<int, string> list = new Dictionary<int, string>();
            using(Cn)
            {
                using SqlDataReader dataReader = Cmd.ExecuteReader();

                while (dataReader.Read())
                    list.Add((int)dataReader["PersonInChargeID"], 
                                    (string)dataReader["PersonInChargeName"]);   
            }
            return list;
        }

        public int Registration(Condolence condolence)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@account_activity_date", condolence.AccountActivityDate },
                {"@location", condolence.Location},
                {"@owner_name", condolence.OwnerName },{"@soryo_name", condolence.SoryoName},
                {"@content", condolence.Content },
                {"@almsgiving", condolence.Almsgiving},
                { "@car_tip", condolence.CarTip},{"@meal_tip", condolence.MealTip},
                {"@car_and_meal_tip", condolence.CarAndMealTip},
                {"@social_gathering", condolence.SocialGathering},{"@note", condolence.Note},
                { "@counter_receiver", GetFirstName(condolence.CounterReceiver)},
                {"@mail_representative", GetFirstName(condolence.MailRepresentative) }
            };
            
            return ReturnGeneretedParameterCommand
                ("registration_condolence", parameters).ExecuteNonQuery();
        }

        public int Update(Condolence condolence)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                {"@condolence_id", condolence.ID },{"@account_activity_date",
                    condolence.AccountActivityDate },
                {"@location", condolence.Location},{"@owner_name", condolence.OwnerName },
                {"@soryo_name", condolence.SoryoName},
                {"@content", condolence.Content },
                {"@almsgiving", condolence.Almsgiving},{ "@car_tip", condolence.CarTip},
                {"@meal_tip", condolence.MealTip},{"@car_and_meal_tip", condolence.CarAndMealTip},
                {"@social_gathering", condolence.SocialGathering},{"@note", condolence.Note},
                { "@counter_receiver", GetFirstName(condolence.CounterReceiver)},
                {"@mail_representative", GetFirstName(condolence.MailRepresentative) }
            };
            
            return ReturnGeneretedParameterCommand
                ("update_condolence", parameters).ExecuteNonQuery();
        }

        public (int TotalRows, ObservableCollection<Condolence> List) ReferenceCondolence
            (DateTime startDate, DateTime endDate,string location, int pageCount)
        {
            ObservableCollection<Condolence> list = new ObservableCollection<Condolence>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            { {"@start_date", startDate},{"@end_date", endDate},
                {"@location", string.Empty},{"@page", pageCount } };
            
            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("reference_condolence", parameters).ExecuteReader();
            
            while(dataReader.Read())
            {
                list.Add(new Condolence((int)dataReader["condolence_id"],
                    (string)dataReader["location"], (string)dataReader["owner_name"],
                    (string)dataReader["soryo_name"], (string)dataReader["content"],
                    (int)dataReader["almsgiving"], (int)dataReader["car_tip"], (int)dataReader["meal_tip"],
                    (int)dataReader["car_and_meal_tip"], (int)dataReader["social_gathering"],
                    (string)dataReader["note"], (DateTime)dataReader["account_activity_date"],
                    (string)dataReader["counter_receiver"], (string)dataReader["mail_representative"]));
            }    
            
            return (ReferenceCondolence(startDate, endDate,location).Count, list);
        }

        public ObservableCollection<Condolence>
            ReferenceCondolence(DateTime startDate, DateTime endDate, string location)
        {
            ObservableCollection<Condolence> list = new ObservableCollection<Condolence>();
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {{ "@start_date", startDate},{"@end_date", endDate},{"@location",location } };
            
            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("reference_condolence_all_data",parameters).ExecuteReader();
            
            while(dataReader.Read())
            {
                list.Add(new Condolence((int)dataReader["condolence_id"],
                    (string)dataReader["location"], (string)dataReader["owner_name"],
                    (string)dataReader["soryo_name"], (string)dataReader["content"],
                    (int)dataReader["almsgiving"], (int)dataReader["car_tip"], (int)dataReader["meal_tip"],
                    (int)dataReader["car_and_meal_tip"], (int)dataReader["social_gathering"],
                    (string)dataReader["note"], (DateTime)dataReader["account_activity_date"],
                    (string)dataReader["counter_receiver"], (string)dataReader["mail_representative"]));
            }
            
            return list;
        }

        public int Registration(string id, string contentConvertText)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            { {"@content_id", id},{"@convert_text", contentConvertText}};
            
            return ReturnGeneretedParameterCommand
                ("registration_content_convert_voucher",parameters).ExecuteNonQuery();
        }

        public int Update(string id, string contentConvertText)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            { {"@content_id", id},{"@convert_text", contentConvertText}};
            
            return ReturnGeneretedParameterCommand
                ("update_content_convert_voucher",parameters).ExecuteNonQuery();
        }

        public string CallContentConvertText(string id)
        {
            if (string.IsNullOrEmpty(id)) return string.Empty;

            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("reference_content_convert_voucher",
                    new Dictionary<string, object>() { { "@content_id", id } }).ExecuteReader();
            string s = default;
            while (dataReader.Read())
            { s = (string)dataReader["convert_text"] ?? string.Empty; }

            return s;
        }

        public int DeleteContentConvertText(string id) =>
            ReturnGeneretedParameterCommand
                ("delete_content_convert_voucher",
                    new Dictionary<string, object>() { { "@content_id", id } }).ExecuteNonQuery();

        public int Registration(Voucher voucher)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
                { "@output_date",voucher.OutputDate},
                {"@addressee",voucher.Addressee},{"@is_validity",true},
                {"@registration_staff_id",LoginRep.GetInstance().Rep.ID }
            };

            return ReturnGeneretedParameterCommand
                ("registration_voucher", parameters).ExecuteNonQuery();
        }

        public int Registration(int voucherID,int receiptsAndExpenditureID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {{ "@voucher_id",voucherID},{"@receipts_and_expenditure_id",receiptsAndExpenditureID}};

            return ReturnGeneretedParameterCommand
                ("registration_voucher_gtbl", parameters).ExecuteNonQuery();
        }

        public Voucher CallLatestVoucher()
        {
            SqlDataReader dataReader = ExecuteNoParameterStoredProc("call_latest_voucher").ExecuteReader();
            Voucher voucher = new Voucher
                (0, string.Empty, new ObservableCollection<ReceiptsAndExpenditure>(), DateTime.Today, 
                    LoginRep.GetInstance().Rep, true);
            while (dataReader.Read())
                voucher = new Voucher
                    ((int)dataReader["voucher_id"], (string)dataReader["addressee"], 
                        new ObservableCollection<ReceiptsAndExpenditure>(),
                        (DateTime)dataReader["output_date"],
                        new Rep((string)dataReader["staff_id"],(string)dataReader["name"],string.Empty,true,false), 
                        (bool)dataReader["is_validity"]);

            return voucher;
        }

        public int Update(Voucher voucher)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            { {"@voucher_id",voucher.ID},{"@output_date",voucher.OutputDate},
                {"@addressee",voucher.Addressee},{"@staff_id",LoginRep.GetInstance().Rep.ID},
                {"@is_validity",voucher.IsValidity} };

            return ReturnGeneretedParameterCommand("update_voucher", parameters).ExecuteNonQuery();
        }

        public ObservableCollection<Voucher> ReferenceVoucher
            (DateTime outputDateStart, DateTime outputDateEnd, bool isValidityTrueOnly)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            { {"@output_date_start",outputDateStart},{"@output_date_end",outputDateEnd},
                {"@is_validity_true_only",isValidityTrueOnly}};
            ObservableCollection<Voucher> list = new ObservableCollection<Voucher>();

            SqlDataReader dataReader = 
                ReturnGeneretedParameterCommand("reference_voucher", parameters).ExecuteReader();

            Rep paramRep;

            while (dataReader.Read())
            {
                paramRep = new Rep((string)dataReader["staff_id"], (string)dataReader["name"],
                    (string)dataReader["password"], (bool)dataReader["staff_validity"],
                    (bool)dataReader["is_permission"]);

                list.Add(new Voucher
                    ((int)dataReader["voucher_id"], (string)dataReader["addressee"],
                    new ObservableCollection<ReceiptsAndExpenditure>(),
                    (DateTime)dataReader["output_date"], paramRep,
                    (bool)dataReader["validity"]));
            }

            foreach(Voucher voucher in list)
                voucher.ReceiptsAndExpenditures = CallVoucherGroupingReceiptsAndExpenditure(voucher.ID);

            return list;
        }

        public ObservableCollection<ReceiptsAndExpenditure> 
            CallVoucherGroupingReceiptsAndExpenditure(int voucherID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            { {"@voucher_id",voucherID}};
            ObservableCollection<ReceiptsAndExpenditure> list = 
                new ObservableCollection<ReceiptsAndExpenditure>();

            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("call_voucher_grouping_receipts_and_expenditure", parameters).ExecuteReader();

            Rep paramRep;
            CreditDept paramCreditDept;
            Content paramContent;
            AccountingSubject paramAccountingSubject;

            while (dataReader.Read())
            {
                paramRep = new Rep((string)dataReader["staff_id"], (string)dataReader["name"], 
                    (string)dataReader["password"], (bool)dataReader["staff_validity"], 
                    (bool)dataReader["is_permission"]);
                paramCreditDept = new CreditDept((string)dataReader["credit_dept_id"],
                    (string)dataReader["dept"], true, (bool)dataReader["is_shunjuen_dept"]);
                paramAccountingSubject = new AccountingSubject((string)dataReader["account_subject_id"], 
                    (string)dataReader["subject_code"], (string)dataReader["subject"], true);
                paramContent = new Content((string)dataReader["content_id"], paramAccountingSubject, 
                    (int)dataReader["flat_rate"], (string)dataReader["content"], true);
                list.Add(new ReceiptsAndExpenditure
                    ((int)dataReader["receipts_and_expenditure_id"], (DateTime)dataReader["registration_date"],
                        paramRep, (string)dataReader["location"], paramCreditDept, paramContent,
                        (string)dataReader["detail"], (int)dataReader["price"], (bool)dataReader["is_payment"],
                        (bool)dataReader["is_validity"], (DateTime)dataReader["account_activity_date"],
                        (DateTime)dataReader["output_date"], (bool)dataReader["is_reduced_tax_rate"]));
            }
            return list;
        }
    }
}
