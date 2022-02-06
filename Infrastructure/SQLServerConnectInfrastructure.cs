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
        private Dictionary<string, object> Parameters;
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
                //ConnectionString = Properties.Settings.Default.TestServerConnectionString
            };
        }
        public bool IsConnectiongProductionServer()
        {
            SettingConectionString();
            return Cn.ConnectionString == Properties.Settings.Default.SystemAdminConnection;
        }
        /// <summary>
        /// sqlを実行するコマンドを生成します
        /// </summary>
        /// <param name="commandText"></param>
        private SqlCommand NewCommand(CommandType commandType, string commandText)
        {
            SettingConectionString();
            SqlCommand Cmd = new SqlCommand();

            try
            {
                Cmd.Connection = Cn;
                Cmd.CommandType = commandType;
                Cmd.CommandText = commandText;
                Cn.Open();
            }
            catch (Exception ex)
            {
                Logger.Log(ILogger.LogInfomation.ERROR, $"SQLServerConnect\t{ex.Message}");
            }
            return Cmd;
        }
        ///<summary>
        /// ストアドを実行するコマンドを返します
        /// </summary>
        /// <param name="commandText">ストアドプロシージャ名</param>
        /// <returns></returns>
        private SqlCommand ReturnGeneretedParameterCommand
            (string commandText)
        {
            SqlCommand Cmd;

            using (Cn)
            {
                Cmd = NewCommand(CommandType.StoredProcedure, commandText);

                foreach (KeyValuePair<string, object> param in Parameters)
                { _ = Cmd.Parameters.AddWithValue(param.Key, param.Value); }
            }

            return Cmd;
        }
        /// <summary>
        /// パラメータのないストアドを実行します
        /// </summary>
        /// <param name="commandText">コマンドテキスト</param>
        /// <returns></returns>
        private SqlCommand ExecuteNoParameterStoredProc(string commandText)
        {
            SqlCommand Cmd;

            using (Cn) { Cmd = NewCommand(CommandType.StoredProcedure, commandText); }

            return Cmd;
        }

        public int Registration(Rep rep)
        {
            Parameters = new Dictionary<string, object>()
            {
                { "@staff_name", rep.Name },
                { "@password", rep.Password},
                { "@validity", rep.IsValidity},
                {"@is_permission", rep.IsAdminPermisson }
            };
            return ReturnGeneretedParameterCommand("registration_staff").ExecuteNonQuery();
        }

        public int Update(Rep rep)
        {
            Parameters = new Dictionary<string, object>()
            {
                {"@staff_id", rep.ID },{"@staff_name", rep.Name },{"@password", rep.Password },
                {"@is_validity", rep.IsValidity },{"@is_permission", rep.IsAdminPermisson },
                {"@operation_staff_id", LoginRep.Rep.ID }
            };

            return ReturnGeneretedParameterCommand
                ("update_staff").ExecuteNonQuery();
        }

        public ObservableCollection<Rep> ReferenceRep(string repName, bool isValidityTrueOnly)
        {
            ObservableCollection<Rep> reps = new ObservableCollection<Rep>();

            Parameters = new Dictionary<string, object>()
            {{ "@staff_name", repName},{ "@true_only", isValidityTrueOnly}};

            SqlDataReader DataReader = ReturnGeneretedParameterCommand
                ("reference_staff").ExecuteReader();
            while (DataReader.Read())
            {
                reps.Add
                    (new Rep((string)DataReader["staff_id"], (string)DataReader["name"],
                        (string)DataReader["password"], (bool)DataReader["is_validity"],
                        (bool)DataReader["is_permission"]));
            }

            return reps;
        }

        public int Registration(AccountingSubject accountingSubject)
        {
            Parameters = new Dictionary<string, object>()
            {
                {"@subject_code", accountingSubject.SubjectCode},{"@subject", accountingSubject.Subject },
                { "@is_shunjuen",accountingSubject.IsShunjuen},{ "@validity", accountingSubject.IsValidity},{"@staff_id", LoginRep.Rep.ID}
            };

            return ReturnGeneretedParameterCommand
                ("registration_accounting_subject").ExecuteNonQuery();
        }

        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject
            (string subjectCode, string subject, bool isShunjuen, bool isTrueOnly)
        {
            ObservableCollection<AccountingSubject> accountingSubjects =
                new ObservableCollection<AccountingSubject>();

            Parameters = new Dictionary<string, object>()
            {
                {"@subject_code", subjectCode},{"@subject", subject},{ "@is_shunjuen",isShunjuen},
                {"@true_only", isTrueOnly}
            };

            SqlDataReader DataReader = ReturnGeneretedParameterCommand
                    ("reference_accounting_subject").ExecuteReader();

            while (DataReader.Read())
            {
                accountingSubjects.Add
                    (new AccountingSubject((string)DataReader["accounting_subject_id"],
                        (string)DataReader["subject_code"], (string)DataReader["subject"],
                        (bool)DataReader["is_shunjuen"], (bool)DataReader["is_validity"]));
            }

            return accountingSubjects;
        }

        public int Update(AccountingSubject accountingSubject)
        {
            Parameters = new Dictionary<string, object>()
            {
                {"@accounting_subject_id", accountingSubject.ID},
                { "@subject_code",accountingSubject.SubjectCode},
                { "@subject",accountingSubject.Subject},{ "@is_shunjuen",accountingSubject.IsShunjuen},
                {"@is_validity", accountingSubject.IsValidity}, { "@operation_staff_id", LoginRep.Rep.ID}
            };

            return ReturnGeneretedParameterCommand
                ("update_accounting_subject").ExecuteNonQuery();
        }

        public int Registration(CreditDept creditDept)
        {
            Parameters = new Dictionary<string, object>()
            {
                {"@dept", creditDept.Dept },{"@is_validity", creditDept.IsValidity},
                {"@staff_id", LoginRep.Rep.ID},{ "@is_shunjuen_dept", creditDept.IsShunjuenDept}
            };

            return ReturnGeneretedParameterCommand
                ("registration_credit_dept").ExecuteNonQuery();
        }

        public ObservableCollection<CreditDept> ReferenceCreditDept
            (string account, bool isValidityTrueOnly, bool isShunjuenAccount)
        {
            ObservableCollection<CreditDept> creditDepts = new ObservableCollection<CreditDept>();
            Parameters = new Dictionary<string, object>()
            {
                {"@dept", account },{ "@true_only", isValidityTrueOnly},
                {"@shunjuen_account", isShunjuenAccount }
            };

            SqlDataReader DataReader = ReturnGeneretedParameterCommand
                ("reference_credit_dept").ExecuteReader();

            while (DataReader.Read())
            {
                creditDepts.Add
                    (new CreditDept((string)DataReader["credit_dept_id"], (string)DataReader["dept"],
                    (bool)DataReader["is_validity"], isShunjuenAccount));
            }

            return creditDepts;
        }

        public int Update(CreditDept creditDept)
        {
            Parameters = new Dictionary<string, object>()
            {
                { "@credit_dept_id", creditDept.ID }, { "@dept", creditDept.Dept },
                { "@is_validity", creditDept.IsValidity }, { "@operation_staff_id", LoginRep.Rep.ID },
                { "@is_shunjuen_dept", creditDept.IsShunjuenDept }
            };

            return ReturnGeneretedParameterCommand("update_credit_dept").ExecuteNonQuery();
        }

        public int Registration(Content content)
        {
            Parameters = new Dictionary<string, object>()
            {
                { "@account_subject_id", content.AccountingSubject.ID},{"@content", content.Text},
                {"@flat_rate", content.FlatRate},{"@is_validity", content.IsValidity },
                {"@staff_id", LoginRep.Rep.ID}
            };

            return ReturnGeneretedParameterCommand
                ("registration_content").ExecuteNonQuery();
        }

        public int Update(Content content)
        {
            Parameters = new Dictionary<string, object>()
            {
                { "@content_id", content.ID }, { "@content", content.Text }, { "@flat_rate", content.FlatRate },
                { "@is_validity", content.IsValidity }, { "@operation_staff_id", LoginRep.Rep.ID }
            };

            return ReturnGeneretedParameterCommand("update_content").ExecuteNonQuery();
        }

        public ObservableCollection<Content> ReferenceContent
            (string contentText, string accountingSubjectCode, string accountingSubject, bool isShunjuen,
                bool isValidityTrueOnly)
        {
            ObservableCollection<Content> contents = new ObservableCollection<Content>();

            Parameters = new Dictionary<string, object>()
            {
                { "@content", contentText},{"@subject_code", accountingSubjectCode},
                {"@subject", accountingSubject},{ "@is_shunjuen",isShunjuen},
                { "@true_only", isValidityTrueOnly}
            };

            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("reference_content").ExecuteReader();

            while (dataReader.Read())
            {
                contents.Add
                    (
                        new Content((string)dataReader["content_id"],
                        new AccountingSubject((string)dataReader["accounting_subject_id"],
                            (string)dataReader["subject_code"], (string)dataReader["subject"],
                            (bool)dataReader["is_shunjuen"], true),
                        (int)dataReader["flat_rate"], (string)dataReader["content"],
                        (bool)dataReader["is_validity"])
                    );
            }
            return contents;
        }

        public AccountingSubject CallAccountingSubject(string id)
        {
            Parameters = new Dictionary<string, object>()
            { { "@accounting_subject_id", id} };
            SqlDataReader DataReader = ReturnGeneretedParameterCommand
                    ("call_accounting_subject").ExecuteReader();

            while (DataReader.Read())
            {
                return new AccountingSubject
                    ((string)DataReader["accounting_subject_id"],
                    (string)DataReader["subject_code"], (string)DataReader["subject"],
                    (bool)DataReader["is_shunjuen"], (bool)DataReader["is_validity"]);
            }

            return null;
        }

        public ObservableCollection<AccountingSubject> ReferenceAffiliationAccountingSubject
            (string contentText)
        {
            ObservableCollection<AccountingSubject> list = new ObservableCollection<AccountingSubject>();
            Parameters = new Dictionary<string, object>()
            { {"@content", contentText}};
            using SqlDataReader DataReader =
                ReturnGeneretedParameterCommand
                    ("reference_affiliation_accounting_subject").ExecuteReader();

            while (DataReader.Read())
            { list.Add(CallAccountingSubject((string)DataReader["accounting_subject_id"])); }

            return list;
        }

        public int Registration(ReceiptsAndExpenditure receiptsAndExpenditure)
        {
            Parameters = new Dictionary<string, object>()
            {
                { "@location", receiptsAndExpenditure.Location },
                { "@account_activity_date", receiptsAndExpenditure.AccountActivityDate },
                { "@registration_date", receiptsAndExpenditure.RegistrationDate },
                { "@registration_staff_id", receiptsAndExpenditure.RegistrationRep.ID },
                { "@credit_dept_id", receiptsAndExpenditure.CreditDept.ID },
                { "@content_id", receiptsAndExpenditure.Content.ID },
                { "@detail", receiptsAndExpenditure.Detail },
                { "@price", receiptsAndExpenditure.Price },
                { "@is_payment", receiptsAndExpenditure.IsPayment },
                { "@is_validity", receiptsAndExpenditure.IsValidity },
                { "@is_reduced_tax_rate", receiptsAndExpenditure.IsReducedTaxRate }
            };

            return ReturnGeneretedParameterCommand
                ("registration_receipts_and_expenditure").ExecuteNonQuery();
        }

        private ObservableCollection<ReceiptsAndExpenditure> ReferenceReceiptsAndExpenditure()
        {
            ObservableCollection<ReceiptsAndExpenditure> list =
                new ObservableCollection<ReceiptsAndExpenditure>();

            using SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("reference_receipts_and_expenditure_all_data").ExecuteReader();

            while (dataReader.Read()) { list.Add(CreateReceiptsAndExpenditure(dataReader)); }
            return list;
        }

        public ObservableCollection<ReceiptsAndExpenditure> ReferenceReceiptsAndExpenditure
            (
                DateTime registrationDateStart, DateTime registrationDateEnd, string location,
                string creditDept, string content, string detail, string accountingSubject,
                string accountingSubjectCode, bool isShunjuen, bool whichDepositAndWithdrawalOnly,
                bool isPayment, bool isContainOutputted, bool isValidityOnly, DateTime accountActivityDateStart,
                DateTime accountActivityDateEnd, DateTime outputDateStart, DateTime outputDateEnd
            )
        {
            Parameters = new Dictionary<string, object>()
            {
                { "@location", location},{"@account_activity_date_start", accountActivityDateStart},
                { "@account_activity_date_end", accountActivityDateEnd},
                {"@registration_date_start", registrationDateStart},
                {"@registration_date_end", registrationDateEnd},
                { "@credit_dept", creditDept},{"@accounting_subject_code", accountingSubjectCode},
                { "@accounting_subject", accountingSubject }, {"@content", content},{"@detail", detail},
                {"@limiting_is_payment", whichDepositAndWithdrawalOnly}, {"@is_payment", isPayment },
                {"@is_shunjuen",isShunjuen},{"@contain_outputted", isContainOutputted},
                {"@validity_true_only", isValidityOnly},{"@output_date_start", outputDateStart},
                { "@output_date_end", outputDateEnd}
            };

            return ReferenceReceiptsAndExpenditure();
        }

        public Rep CallRep(string id)
        {
            Rep rep = default;
            Parameters = new Dictionary<string, object>() { { "@staff_id", id } };

            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("call_staff").ExecuteReader();

            while (dataReader.Read())
            {
                rep = new Rep((string)dataReader["staff_id"], (string)dataReader["name"],
                    (string)dataReader["password"], (bool)dataReader["is_validity"],
                    (bool)dataReader["is_permission"]);
            }

            return rep;
        }

        public CreditDept CallCreditDept(string id)
        {
            CreditDept creditDept = default;
            Parameters = new Dictionary<string, object>() { { "@credit_dept_id", id } };
            SqlDataReader dataReader =
                ReturnGeneretedParameterCommand("call_credit_dept").ExecuteReader();

            while (dataReader.Read())
            {
                creditDept = new CreditDept((string)dataReader["credit_dept_id"], (string)dataReader["dept"],
                    (bool)dataReader["is_validity"], (bool)dataReader["is_shunjuen_dept"]);
            }

            return creditDept;
        }

        public Content CallContent(string id)
        {
            Content content = default;
            Parameters = new Dictionary<string, object>() { { "@content_id", id } };
            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("call_content").ExecuteReader();

            while (dataReader.Read())
            {
                content = new Content
                    (
                        (string)dataReader["content_id"],
                        new AccountingSubject((string)dataReader["accounting_subject_id"],
                        (string)dataReader["subject_code"], (string)dataReader["subject"],
                        (bool)dataReader["is_shunjuen"], true),
                        (int)dataReader["flat_rate"], (string)dataReader["content"],
                        (bool)dataReader["is_validity"]
                    );
            }

            return content;
        }

        public int Update(ReceiptsAndExpenditure receiptsAndExpenditure)
        {
            Parameters = new Dictionary<string, object>()
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
                ("update_receipts_and_expenditure").ExecuteNonQuery();
        }

        public int PreviousDayFinalAmount(bool isShunjuen)
        {
            SettingConectionString();
            SqlCommand Cmd = new SqlCommand
                ($"select dbo.return_previous_day_final_amount('" +
                    $"{AccountingProcessLocation.IsAccountingGenreShunjuen}')", Cn);
            Cn.Open();
            int amount = default;
            using (Cn) { amount = (int)Cmd.ExecuteScalar(); }

            return amount;
        }

        public int PreviousDayFinalAmount(CreditDept creditDept)
        {
            int amount = default;
            SettingConectionString();
            SqlCommand cmd = new SqlCommand
                ($"select dbo.return_previous_day_final_credit_dept_amount(@credit_dept_id,@date)", Cn);
            _ = cmd.Parameters.AddWithValue("@credit_dept_id", creditDept.ID);
            _ = cmd.Parameters.AddWithValue("@date", DateTime.Today);
            //($"return_previous_day_final_credit_dept_amount('{creditDept.ID}','{DateTime.Today.ToShortDateString()}')", Cn);
            Cn.Open();

            using (Cn) { amount = (int)cmd.ExecuteScalar(); }
            return amount;
        }

        public int RegistrationPrecedingYearFinalAccount()
        {
            return ReturnGeneretedParameterCommand
                ("registration_preceding_year_final_account_table").ExecuteNonQuery();
        }

        public int RegistrationPrecedingYearFinalAccount(CreditDept creditDept)
        {
            Parameters = new Dictionary<string, object>() { { "@credit_dept_id", creditDept.ID } };

            return ReturnGeneretedParameterCommand
                ("registration_wizecore_preceding_year_final_account_table").ExecuteNonQuery();
        }

        public int CallPrecedingYearFinalAccount(DateTime date, CreditDept creditDept)
        {
            SqlCommand cmd =
                new SqlCommand("select dbo.call_previous_month_final_account(@date,@credit_dept", Cn);
            _ = cmd.Parameters.AddWithValue("@date", date);
            _ = cmd.Parameters.AddWithValue("@credit_dept", creditDept);
            Cn.Open();

            int amount = default;
            using (Cn) { amount = (int)cmd.ExecuteScalar(); }

            return amount;
        }

        public int CallPrecedingYearFinalAccount(DateTime date)
        {
            SettingConectionString();
            SqlCommand Cmd =
                new SqlCommand("select dbo.call_shunjuen_preceding_year_final_account(@date)", Cn);
            _ = Cmd.Parameters.AddWithValue("@date", date);
            Cn.Open();

            int amount = default;
            using (Cn) { amount = (int)Cmd.ExecuteScalar(); }

            return amount;
        }

        public int ReceiptsAndExpenditurePreviousDayChange
            (ReceiptsAndExpenditure receiptsAndExpenditure)
        {
            return NewCommand(CommandType.Text,
                $"select * from receipts_and_expenditure_data with(tablockx) begin tran " +
                $"update receipts_and_expenditure_data " +
                $"set output_date='{DateTime.Now.AddDays(-1) }'" +
                $"where receipts_and_expenditure_id='{receiptsAndExpenditure.ID}' commit tran")
                .ExecuteNonQuery();
        }

        public (int TotalRows, ObservableCollection<ReceiptsAndExpenditure> List)
            ReferenceReceiptsAndExpenditure(DateTime registrationDateStart,
                DateTime registrationDateEnd, string location, string creditDept, string content, string detail,
                string accountingSubject, string accountingSubjectCode, bool isShunjuen,
                bool whichDepositAndWithdrawalOnly, bool isPayment, bool isContainOutputted, bool isValidityOnly,
                DateTime accountActivityDateStart, DateTime accountActivityDateEnd, DateTime outputDateStart,
                DateTime outputDateEnd, int pageCount, string sortColumn, bool sortDirection, int countEachPage)
        {
            ObservableCollection<ReceiptsAndExpenditure> list =
                new ObservableCollection<ReceiptsAndExpenditure>();

            Parameters = new Dictionary<string, object>()
            {
                { "@location", location},{"@account_activity_date_start", accountActivityDateStart},
                { "@account_activity_date_end", accountActivityDateEnd},
                {"@registration_date_start", registrationDateStart},
                {"@registration_date_end", registrationDateEnd},
                { "@credit_dept", creditDept},{"@accounting_subject_code", accountingSubjectCode},
                { "@accounting_subject", accountingSubject }, {"@content", content},{"@detail", detail},
                {"@limiting_is_payment", whichDepositAndWithdrawalOnly}, {"@is_payment", isPayment },
                {"@is_shunjuen",isShunjuen },{"@contain_outputted", isContainOutputted},
                {"@validity_true_only", isValidityOnly},{"@output_date_start", outputDateStart},
                { "@output_date_end", outputDateEnd},{"@page", pageCount},{"@column",sortColumn},
                { "@is_order_asc",sortDirection},{"@count_each_page",countEachPage}
            };

            SqlDataReader dataReader =
                ReturnGeneretedParameterCommand
                    ("reference_receipts_and_expenditure").ExecuteReader();

            while (dataReader.Read()) { list.Add(CreateReceiptsAndExpenditure(dataReader)); }

            _ = Parameters.Remove("@page");
            _ = Parameters.Remove("@column");
            _ = Parameters.Remove("@is_order_asc");
            _ = Parameters.Remove("@count_each_page");

            return (ReferenceReceiptsAndExpenditure().Count, list);
        }

        private ReceiptsAndExpenditure CreateReceiptsAndExpenditure(SqlDataReader dataReader)
        {
            Rep paramRep;
            CreditDept paramCreditDept;
            AccountingSubject paramAccountingSubject;
            Content paramContent;

            paramRep =
                new Rep((string)dataReader["staff_id"], (string)dataReader["name"],
                    (string)dataReader["password"], true, (bool)dataReader["is_permission"]);
            paramCreditDept =
                new CreditDept((string)dataReader["credit_dept_id"], (string)dataReader["dept"], true,
                (bool)dataReader["is_shunjuen_dept"]);
            paramAccountingSubject =
                new AccountingSubject((string)dataReader["accounting_subject_id"],
                (string)dataReader["subject_code"], (string)dataReader["subject"],
                (bool)dataReader["is_shunjuen"],
                true);
            paramContent = new Content((string)dataReader["content_id"], paramAccountingSubject,
                (int)dataReader["flat_rate"], (string)dataReader["content"], true);

            return new ReceiptsAndExpenditure
                (
                    (int)dataReader["receipts_and_expenditure_id"],
                    (DateTime)dataReader["registration_date"], paramRep, (string)dataReader["location"],
                    paramCreditDept, paramContent, (string)dataReader["detail"], (int)dataReader["price"],
                    (bool)dataReader["is_payment"], (bool)dataReader["is_validity"],
                    (DateTime)dataReader["account_activity_date"], (DateTime)dataReader["output_date"],
                    (bool)dataReader["is_reduced_tax_rate"]
                );
        }

        public Dictionary<int, string> GetSoryoList()
        {
            Cn = new SqlConnection
            {
                ConnectionString = Properties.Settings.Default.SingyoujiDataBaseConnection
            };

            Cn.Open();

            SqlCommand Cmd = new SqlCommand
            {
                Connection = Cn,
                CommandType = CommandType.Text,
                CommandText = "select * from PersonInChargeMaster where OrderNumber>0"
            };

            Dictionary<int, string> list = new Dictionary<int, string>();
            using (Cn)
            {
                using SqlDataReader dataReader = Cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    list.Add((int)dataReader["PersonInChargeID"],
                                    (string)dataReader["PersonInChargeName"]);
                }
            }
            return list;
        }

        public int Registration(Condolence condolence)
        {
            Parameters = new Dictionary<string, object>()
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
                ("registration_condolence").ExecuteNonQuery();
        }

        public int Update(Condolence condolence)
        {
            Parameters = new Dictionary<string, object>()
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
                ("update_condolence").ExecuteNonQuery();
        }

        public (int TotalRows, ObservableCollection<Condolence> List) ReferenceCondolence
            (DateTime startDate, DateTime endDate, string location, int pageCount, int countEachPage)
        {
            ObservableCollection<Condolence> list = new ObservableCollection<Condolence>();
            Parameters = new Dictionary<string, object>()
            {
                {"@start_date", startDate},{"@end_date", endDate},
                {"@location", location},{"@page", pageCount } ,{"@count_each_page",countEachPage }
            };

            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("reference_condolence").ExecuteReader();

            while (dataReader.Read())
            {
                list.Add(CreateCondolence(dataReader));
            }

            return (ReferenceCondolence(startDate, endDate, location).Count, list);
        }

        private Condolence CreateCondolence(SqlDataReader dataReader)
        {
            return new Condolence((int)dataReader["condolence_id"],
                    (string)dataReader["location"], (string)dataReader["owner_name"],
                    (string)dataReader["soryo_name"], (string)dataReader["content"],
                    (int)dataReader["almsgiving"], (int)dataReader["car_tip"], (int)dataReader["meal_tip"],
                    (int)dataReader["car_and_meal_tip"], (int)dataReader["social_gathering"],
                    (string)dataReader["note"], (DateTime)dataReader["account_activity_date"],
                    (string)dataReader["counter_receiver"], (string)dataReader["mail_representative"]);
        }

        public ObservableCollection<Condolence>
            ReferenceCondolence(DateTime startDate, DateTime endDate, string location)
        {
            ObservableCollection<Condolence> list = new ObservableCollection<Condolence>();
            Parameters = new Dictionary<string, object>()
            {{ "@start_date", startDate},{"@end_date", endDate},{"@location",location } };

            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("reference_condolence_all_data").ExecuteReader();

            while (dataReader.Read())
            {
                list.Add(CreateCondolence(dataReader));
            }

            return list;
        }

        public int Registration(string id, string contentConvertText)
        {
            Parameters = new Dictionary<string, object>()
            { {"@content_id", id},{"@convert_text", contentConvertText}};

            return ReturnGeneretedParameterCommand
                ("registration_content_convert_voucher").ExecuteNonQuery();
        }

        public int Update(string id, string contentConvertText)
        {
            Parameters = new Dictionary<string, object>()
            { {"@content_id", id},{"@convert_text", contentConvertText}};

            return ReturnGeneretedParameterCommand
                ("update_content_convert_voucher").ExecuteNonQuery();
        }

        public string CallContentConvertText(string id)
        {
            if (string.IsNullOrEmpty(id)) { return string.Empty; }

            Parameters = new Dictionary<string, object>() { { "@content_id", id } };

            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("reference_content_convert_voucher").ExecuteReader();
            string s = default;
            while (dataReader.Read())
            { s = (string)dataReader["convert_text"] ?? string.Empty; }

            return s;
        }

        public int DeleteConvertContent(Content convertContent)
        {
            Parameters = new Dictionary<string, object>() { { "@content_id", convertContent.ID } };
            return ReturnGeneretedParameterCommand
                ("delete_content_convert_voucher").ExecuteNonQuery();
        }

        public int Registration(Voucher voucher)
        {
            Parameters = new Dictionary<string, object>()
            {
                { "@output_date",voucher.OutputDate},
                {"@addressee",voucher.Addressee},{"@is_validity",true},
                {"@registration_staff_id",LoginRep.GetInstance().Rep.ID }
            };

            return ReturnGeneretedParameterCommand
                ("registration_voucher").ExecuteNonQuery();
        }

        public int Registration(int voucherID, int receiptsAndExpenditureID)
        {
            Parameters = new Dictionary<string, object>()
            {{ "@voucher_id",voucherID},{"@receipts_and_expenditure_id",receiptsAndExpenditureID}};

            return ReturnGeneretedParameterCommand
                ("registration_voucher_gtbl").ExecuteNonQuery();
        }

        public Voucher CallLatestVoucher()
        {
            SqlDataReader dataReader =
                ExecuteNoParameterStoredProc("call_latest_voucher").ExecuteReader();
            Voucher voucher = new Voucher
                (0, string.Empty, new ObservableCollection<ReceiptsAndExpenditure>(), DateTime.Today,
                    LoginRep.GetInstance().Rep, true);
            while (dataReader.Read())
            {
                voucher = new Voucher
                      ((int)dataReader["voucher_id"], (string)dataReader["addressee"],
                          new ObservableCollection<ReceiptsAndExpenditure>(),
                          (DateTime)dataReader["output_date"],
                          new Rep((string)dataReader["staff_id"], (string)dataReader["name"], string.Empty, true,
                            false),
                          (bool)dataReader["is_validity"]);
            }

            return voucher;
        }

        public int Update(Voucher voucher)
        {
            Parameters = new Dictionary<string, object>()
            { {"@voucher_id",voucher.ID},{"@output_date",voucher.OutputDate},
                {"@addressee",voucher.Addressee},{"@staff_id",LoginRep.GetInstance().Rep.ID},
                {"@is_validity",voucher.IsValidity} };

            return ReturnGeneretedParameterCommand("update_voucher").ExecuteNonQuery();
        }

        public ObservableCollection<Voucher> ReferenceVoucher
            (DateTime outputDateStart, DateTime outputDateEnd, bool isValidityTrueOnly)
        {
            Parameters = new Dictionary<string, object>()
            { {"@output_date_start",outputDateStart},{"@output_date_end",outputDateEnd},
                {"@is_validity_true_only",isValidityTrueOnly}};

            ObservableCollection<Voucher> list = new ObservableCollection<Voucher>();

            SqlDataReader dataReader =
                ReturnGeneretedParameterCommand("reference_voucher").ExecuteReader();

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

            foreach (Voucher voucher in list)
            { voucher.ReceiptsAndExpenditures = CallVoucherGroupingReceiptsAndExpenditure(voucher.ID); }

            return list;
        }

        public ObservableCollection<ReceiptsAndExpenditure>
            CallVoucherGroupingReceiptsAndExpenditure(int voucherID)
        {
            Parameters = new Dictionary<string, object>()
            { {"@voucher_id",voucherID}};
            ObservableCollection<ReceiptsAndExpenditure> list =
                new ObservableCollection<ReceiptsAndExpenditure>();

            SqlDataReader dataReader = ReturnGeneretedParameterCommand
                ("call_voucher_grouping_receipts_and_expenditure").ExecuteReader();

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
                    (string)dataReader["subject_code"], (string)dataReader["subject"],
                    (bool)dataReader["is_shunjuen"], true);
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

        public int Delete(Condolence condolence)
        {
            Parameters = new Dictionary<string, object> { { "@id", condolence.ID } };
            return ReturnGeneretedParameterCommand("delete_condolence").ExecuteNonQuery();
        }

        public CreditDept CallContentDefaultCreditDept(Content content)
        {
            Parameters = new Dictionary<string, object>()
            {
                {"@content_id",content.ID }
            };
            SqlDataReader sqlDataReader =
                ReturnGeneretedParameterCommand("call_content_default_credit_dept").ExecuteReader();

            CreditDept creditDept = null;
            while (sqlDataReader.Read())
            {
                creditDept = new CreditDept((string)sqlDataReader["credit_dept_id"],
                    (string)sqlDataReader["dept"], (bool)sqlDataReader["is_validity"],
                    (bool)sqlDataReader["is_shunjuen_dept"]);
            }
            return creditDept;
        }

        public int Registration(CreditDept creditDept, Content content)
        {
            Parameters = new Dictionary<string, object>()
            {
                {"@credit_dept_id",creditDept.ID },
                {"@content_id",content.ID }
            };

            return ReturnGeneretedParameterCommand
                ("registration_content_default_credit_dept").ExecuteNonQuery();
        }

        public int Update(CreditDept creditDept, Content content)
        {
            Parameters = new Dictionary<string, object>()
            {
                {"@credit_dept_id",creditDept.ID },
                {"@content_id",content.ID }
            };

            return ReturnGeneretedParameterCommand
                ("update_content_default_credit_dept").ExecuteNonQuery();
        }

        public int DeleteContentDefaultCreditDept(Content content)
        {
            Parameters = new Dictionary<string, object>()
            {
                {"@content_id",content.ID }
            };

            return ReturnGeneretedParameterCommand
                ("delete_content_default_credit_dept").ExecuteNonQuery();
        }

        public string GetBranchNumber(AccountingSubject accountingSubject)
        {
            Parameters = new Dictionary<string, object>()
            { { "@accounting_subject_id", accountingSubject.ID } };

            SqlDataReader sqlDataReader = ReturnGeneretedParameterCommand
                ("get_branch_number").ExecuteReader();

            string s = string.Empty;
            while (sqlDataReader.Read())
            {
                s = (string)sqlDataReader["branch_number"];
            }

            return s;
        }

        public int Update(AccountingSubject accountingSubject, string branchNumber)
        {
            Parameters = new Dictionary<string, object>()
            { {"@accounting_subject_id",accountingSubject.ID},{"@branch_number",branchNumber}};

            return ReturnGeneretedParameterCommand
                ("update_branch_number").ExecuteNonQuery();
        }

        public int DeleteBranchNumber(AccountingSubject accountingSubject)
        {
            Parameters = new Dictionary<string, object>()
            { { "@accounting_subject_id", accountingSubject.ID } };

            return ReturnGeneretedParameterCommand
                ("delete_branch_number").ExecuteNonQuery();
        }

        public int Registration(AccountingSubject accountingSubject, string branchNumber)
        {
            Parameters = new Dictionary<string, object>()
            {{"@accounting_subject_id",accountingSubject.ID },{"@branch_number",branchNumber } };

            return ReturnGeneretedParameterCommand
                ("registration_branch_number").ExecuteNonQuery();
        }

        public int CallFinalMonthFinalAccount(DateTime date, bool isShunjuen, CreditDept creditDept)
        {
            SettingConectionString();
            string dept = creditDept == null ? string.Empty : creditDept.Dept;
            SqlCommand cmd = new SqlCommand
                    ($"select dbo.call_previous_month_final_account('{date}','{isShunjuen}','{dept}')", Cn);

            Cn.Open();

            int amount = default;
            using (Cn) { amount = (int)cmd.ExecuteScalar(); }

            return amount;
        }

        public int UpdatePrecedingYearFinalAccount()
        {
            int amount = CallFinalMonthFinalAccount(new DateTime(DateTime.Today.Year, 4, 1), true, null);
            Parameters = new Dictionary<string, object>()
            { {"@reference_date",DateTime.Today},{ "@amount",amount} };

            return ReturnGeneretedParameterCommand
                ("update_preceding_year_final_account").ExecuteNonQuery();
        }

        public int UpdatePrecedingYearFinalAccount(CreditDept creditDept)
        {
            int amount = CallFinalMonthFinalAccount(new DateTime(DateTime.Today.Year, 4, 1), false, creditDept);
            Parameters = new Dictionary<string, object>()
            { {"@reference_date",DateTime.Today},{"@credit_dept_id",creditDept.ID},{"@amount",amount }};

            return ReturnGeneretedParameterCommand
                ("update_wize_core_preceding_year_final_account").ExecuteNonQuery();
        }

        public ObservableCollection<TransferReceiptsAndExpenditure> ReferenceTransferReceiptsAndExpenditure
            (bool isShunjuenDept, DateTime accountActivityDateStart, DateTime accountActivityDateEnd, string location,
                string dept, string debitAccountCode, string debitAccount, string creditAccountCode, string creditAccount,
                bool isValidityTrueOnly, bool containOutputted, DateTime outputDateStart, DateTime outputDateEnd)
        {
            ObservableCollection<TransferReceiptsAndExpenditure> list = new ObservableCollection<TransferReceiptsAndExpenditure>();
            SetReferenceTransferReceiptsAndExpenditureParameters
                (isShunjuenDept, accountActivityDateStart, accountActivityDateEnd, location, dept, debitAccountCode, debitAccount,
                    creditAccountCode, creditAccount, isValidityTrueOnly, containOutputted, outputDateStart, outputDateEnd);
            SqlDataReader reader =
                ReturnGeneretedParameterCommand("reference_transfer_receipts_and_expenditure_all_data").ExecuteReader();

            Rep rep;
            CreditDept creditDept;
            AccountingSubject debitAccountingSubject;
            AccountingSubject creditAccountingSubject;

            while (reader.Read()) 
            {
                rep = new Rep((string)reader["registration_staff_id"], (string)reader["name"], (string)reader["password"], true, false);
                creditDept = new CreditDept((string)reader["credit_dept_id"], (string)reader["dept"], true, (bool)reader["is_shunjuen_dept"]);
                debitAccountingSubject = new AccountingSubject((string)reader["debit_accounts_id"], (string)reader["debit_code"], (string)reader["debit"], true, true);
                creditAccountingSubject = new AccountingSubject((string)reader["credit_accounts_id"], (string)reader["credit_code"], (string)reader["credit"], true, true);

                list.Add(new TransferReceiptsAndExpenditure
                    ((int)reader["transfer_receipts_and_expenditure_id"], (DateTime)reader["registration_date"], rep,
                        (string)reader["location"], creditDept, debitAccountingSubject, creditAccountingSubject,
                        (string)reader["content_text"], (string)reader["detail"], (int)reader["price"], (bool)reader["is_validity"],
                        (DateTime)reader["account_activity_date"], (DateTime)reader["output_date"],
                        (bool)reader["is_reduced_tax_rate"]));
            }

            return list;
        }

        public ObservableCollection<TransferReceiptsAndExpenditure> ReferenceTransferReceiptsAndExpenditure
            (bool isShunjuenDept, DateTime accountActivityDateStart, DateTime accountActivityDateEnd, string location,
                string dept, string debitAccountCode, string debitAccount, string creditAccountCode, string creditAccount,
                bool isValidityTrueOnly, bool containOutputted, DateTime outputDateStart, DateTime outputDateEnd,
                int page, string column, bool isOrderAsc, int countEachPage)
        {
            SetReferenceTransferReceiptsAndExpenditureParameters(isShunjuenDept, accountActivityDateStart,
                accountActivityDateEnd, location, dept, debitAccountCode, debitAccount, creditAccountCode, creditAccount,
                isValidityTrueOnly, containOutputted, outputDateStart, outputDateEnd);
            Parameters.Add("page", page);
            Parameters.Add("column", column);
            Parameters.Add("is_order_asc", isOrderAsc);
            Parameters.Add("count_each_page", countEachPage);

            ObservableCollection<TransferReceiptsAndExpenditure> list = new ObservableCollection<TransferReceiptsAndExpenditure>();

            SqlDataReader reader = ReturnGeneretedParameterCommand("reference_transfer_receipts_and_expenditure").ExecuteReader();

            return list;
        }

        private void SetReferenceTransferReceiptsAndExpenditureParameters
            (bool isShunjuenDept, DateTime accountActivityDateStart, DateTime accountActivityDateEnd, string location,
                string dept, string debitAccountCode, string debitAccount, string creditAccountCode, string creditAccount,
                bool isValidityTrueOnly, bool containOutputted, DateTime outputDateStart, DateTime outputDateEnd)

        {
            Parameters = new Dictionary<string, object>()
            {
                {"@is_shunjuen_dept",isShunjuenDept },{"@accont_activity_start_date",accountActivityDateStart},
                {"@account_activity_end_date",accountActivityDateEnd},{ "@location",location},{ "@dept",dept},
                { "@debit_account_code",debitAccountCode},{ "@debit_account",debitAccount},
                { "@credit_account_code",creditAccountCode},{ "@credit_account",creditAccount},
                { "@is_validity_true_only",isValidityTrueOnly},{ "@contain_outputted",containOutputted},
                { "@output_start_date",outputDateStart},{ "@output_end_date",outputDateEnd}
            };
        }
    }
}