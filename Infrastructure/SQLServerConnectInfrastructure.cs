using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure
{
    /// <summary>
    /// SQLServer接続クラス
    /// </summary>
    public class SQLServerConnectInfrastructure : IDataBaseConnect
    {
        private SqlConnection Cn;
        private readonly LoginRep LoginRep=LoginRep.GetInstance();

        /// <summary>
        /// コネクションストリングを設定します。
        /// </summary>
        private void SettingConectionString()
        {
            Cn = new SqlConnection
            {
                ConnectionString=Properties.Settings.Default.SystemAdminConnection
                //ConnectionString = LoginRep.Rep.IsAdminPermisson
                //? Properties.Settings.Default.SystemAdminConnection
                //: Properties.Settings.Default.AccountingProcessConnection
            };
        }
        /// <summary>
        /// ストアドプロシージャを実行するコマンドを生成します
        /// </summary>
        /// <param name="commandText">ストアドプロシージャ名</param>
        private void ADO_NewInstance_StoredProc(SqlCommand cmd, string commandText)
        {
            SettingConectionString();

            cmd.Connection = Cn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = commandText;
            Cn.Open();
        }
   
        public int Registration(Rep rep)
        {
            SqlCommand Cmd = new SqlCommand();
            using (Cn) 
            {
                ADO_NewInstance_StoredProc(Cmd,"registration_staff");
                Cmd.Parameters.AddWithValue("@staff_name", rep.Name);
                Cmd.Parameters.AddWithValue("@password", rep.Password);
                Cmd.Parameters.AddWithValue("@validity", rep.IsValidity);
                Cmd.Parameters.AddWithValue("@is_permission", rep.IsAdminPermisson);
                return Cmd.ExecuteNonQuery();
            }
        }
       
        public int Update(Rep rep)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"update_staff");
                Cmd.Parameters.AddWithValue("@staff_id", rep.ID);
                Cmd.Parameters.AddWithValue("@staff_name", rep.Name);
                Cmd.Parameters.AddWithValue("@password", rep.Password);
                Cmd.Parameters.AddWithValue("@is_validity", rep.IsValidity);
                Cmd.Parameters.AddWithValue("@is_permission", rep.IsAdminPermisson);
                Cmd.Parameters.AddWithValue("@operation_staff_id", LoginRep.Rep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
       
        public ObservableCollection<Rep> ReferenceRep(string repName,bool isValidityTrueOnly)
        {
            SqlCommand Cmd = new SqlCommand();
            ObservableCollection<Rep> reps = new ObservableCollection<Rep>();
            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"reference_staff");
                Cmd.Parameters.Add(new SqlParameter("@staff_name", repName));
                Cmd.Parameters.Add(new SqlParameter("@true_only", isValidityTrueOnly));
                SqlDataReader DataReader = Cmd.ExecuteReader();

                while (DataReader.Read())
                    reps.Add
                        (new Rep((string)DataReader["staff_id"], (string)DataReader["name"], 
                        (string)DataReader["password"], (bool)DataReader["is_validity"], 
                        (bool)DataReader["is_permission"]));
            }
            return reps;
        }
     
        public int Registration(AccountingSubject accountingSubject)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"registration_accounting_subject");
                Cmd.Parameters.AddWithValue("@subject_code", accountingSubject.SubjectCode);
                Cmd.Parameters.AddWithValue("@subject", accountingSubject.Subject);
                Cmd.Parameters.AddWithValue("@validity", accountingSubject.IsValidity);
                Cmd.Parameters.AddWithValue("@staff_id", LoginRep.Rep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }

        public ObservableCollection<AccountingSubject> ReferenceAccountingSubject
            (string subjectCode, string subject, bool isTrueOnly)
        {
            SqlDataReader DataReader;
            ObservableCollection<AccountingSubject> accountingSubjects = 
                new ObservableCollection<AccountingSubject>();
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"reference_accounting_subject");
                Cmd.Parameters.AddWithValue("@subject_code", subjectCode);
                Cmd.Parameters.AddWithValue("@subject", subject);
                Cmd.Parameters.AddWithValue("@true_only", isTrueOnly);
                DataReader = Cmd.ExecuteReader();

                while (DataReader.Read())
                    accountingSubjects.Add
                        (new AccountingSubject((string)DataReader["accounting_subject_id"], 
                        (string)DataReader["subject_code"], (string)DataReader["subject"], 
                        (bool)DataReader["is_validity"]));

                return accountingSubjects;
            }
        }
            
        public int Update(AccountingSubject accountingSubject)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"update_accounting_subject");
                Cmd.Parameters.AddWithValue("@accounting_subject_id", accountingSubject.ID);
                Cmd.Parameters.AddWithValue("@is_validity", accountingSubject.IsValidity);
                Cmd.Parameters.AddWithValue("@operation_staff_id", LoginRep.Rep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
  
        public int Registration(CreditDept creditDept)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"registration_credit_dept");
                Cmd.Parameters.AddWithValue("@account", creditDept.Dept);
                Cmd.Parameters.AddWithValue("@is_validity", creditDept.IsValidity);
                Cmd.Parameters.AddWithValue("@staff_id", LoginRep.Rep.ID);
                Cmd.Parameters.AddWithValue("@is_shunjuen_account", creditDept.IsShunjuenAccount);
                return Cmd.ExecuteNonQuery();
            }
        }
  
        public ObservableCollection<CreditDept> ReferenceCreditDept
            (string account, bool isValidityTrueOnly,bool isShunjuenAccountOnly)
        {
            SqlDataReader DataReader;
            ObservableCollection<CreditDept> creditDepts = new ObservableCollection<CreditDept>();
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"reference_credit_dept");
                Cmd.Parameters.AddWithValue("@dept", account);
                Cmd.Parameters.AddWithValue("@true_only", isValidityTrueOnly);
                Cmd.Parameters.AddWithValue("@shunjuen_account_only", isShunjuenAccountOnly);
                DataReader = Cmd.ExecuteReader();

                while (DataReader.Read()) creditDepts.Add
                        (new CreditDept((string)DataReader["credit_dept_id"], (string)DataReader["dept"], 
                        (bool)DataReader["is_validity"], isShunjuenAccountOnly));
            }
            return creditDepts;
       }
     
        public int Update(CreditDept creditDept)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"update_credit_dept");
                Cmd.Parameters.AddWithValue("@credit_dept_id", creditDept.ID);
                Cmd.Parameters.AddWithValue("@is_validity", creditDept.IsValidity);
                Cmd.Parameters.AddWithValue("@operation_staff_id", LoginRep.Rep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
   
        public int Registration(Content content)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"registration_content");
                Cmd.Parameters.AddWithValue("@account_subject_id", content.AccountingSubject.ID);
                Cmd.Parameters.AddWithValue("@content", content.Text);
                Cmd.Parameters.AddWithValue("@flat_rate", content.FlatRate);
                Cmd.Parameters.AddWithValue("@is_validity", content.IsValidity);
                Cmd.Parameters.AddWithValue("@staff_id", LoginRep.Rep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
   
        public int Update(Content content)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd,"update_content");
                Cmd.Parameters.AddWithValue("@content_id", content.ID);
                Cmd.Parameters.AddWithValue("@content", content.Text);
                Cmd.Parameters.AddWithValue("@flat_rate", content.FlatRate);
                Cmd.Parameters.AddWithValue("@is_validity", content.IsValidity);
                Cmd.Parameters.AddWithValue("@operation_staff_id", LoginRep.Rep.ID);
                return Cmd.ExecuteNonQuery();
            }
        }
    
        public ObservableCollection<Content> ReferenceContent
            (string contentText,string accountingSubjectCode, 
            string accountingSubject, bool isValidityTrueOnly)
        {
            ObservableCollection<Content> contents = new ObservableCollection<Content>();
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd, "reference_content");
                Cmd.Parameters.AddWithValue("@content", contentText);
                Cmd.Parameters.AddWithValue("@subject_code", accountingSubjectCode);
                Cmd.Parameters.AddWithValue("@subject", accountingSubject);
                Cmd.Parameters.AddWithValue("@true_only", isValidityTrueOnly);
                SqlDataAdapter sda = new SqlDataAdapter(Cmd);

                using DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                    contents.Add
                        (
                            new Content((string)dr["content_id"],
                            new AccountingSubject((string)dr["accounting_subject_id"],
                            (string)dr["subject_code"], (string)dr["subject"], true), (int)dr["flat_rate"],
                            (string)dr["content"], (bool)dr["is_validity"]));
            }
            return contents;
        }

        public AccountingSubject CallAccountingSubject(string id)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                using SqlDataReader DataReader = 
                    ReturnReaderCommandOneParameterStoredProc
                        (Cmd, "call_accounting_subject", "@accounting_subject_id", id);

                while (DataReader.Read())
                    return new AccountingSubject
                        ((string)DataReader["accounting_subject_id"],
                            (string)DataReader["subject_code"], (string)DataReader["subject"],
                            (bool)DataReader["is_validity"]);
            }
            return null;
        }
        /// <summary>
        /// パラメータが一つのストアドプロシージャを実行し、DataReaderを返します
        /// </summary>
        /// <param name="commandText">ストアドプロシージャ名</param>
        /// <param name="parameterName">パラメータ名</param>
        /// <param name="parameter">パラメータ</param>
        /// <returns>ExecuteReader</returns>
        private SqlDataReader ReturnReaderCommandOneParameterStoredProc
            (SqlCommand cmd, string commandText,string parameterName,string parameter)
        {
            ADO_NewInstance_StoredProc(cmd,commandText);
            cmd.Parameters.AddWithValue(parameterName, parameter);
            return cmd.ExecuteReader();
        }

        public ObservableCollection<AccountingSubject> ReferenceAffiliationAccountingSubject
            (string contentText)
        {
            SqlCommand Cmd = new SqlCommand();
            ObservableCollection<AccountingSubject> list = new ObservableCollection<AccountingSubject>();

            using(Cn)
            {
                using SqlDataReader DataReader =
                    ReturnReaderCommandOneParameterStoredProc
                        (Cmd, "reference_affiliation_accounting_subject", "@content", contentText);

                while (DataReader.Read()) list.Add
                        (CallAccountingSubject((string)DataReader["accounting_subject_id"]));
            }
            return list;
        }

        public int Registration(ReceiptsAndExpenditure receiptsAndExpenditure)
        {
            SqlCommand Cmd = new SqlCommand();

            using(Cn)
            {
                ADO_NewInstance_StoredProc(Cmd, "registration_receipts_and_expenditure");
                Cmd.Parameters.AddWithValue("@location", receiptsAndExpenditure.Location);
                Cmd.Parameters.AddWithValue
                    ("@account_activity_date", receiptsAndExpenditure.AccountActivityDate);
                Cmd.Parameters.AddWithValue("@registration_date", receiptsAndExpenditure.RegistrationDate);
                Cmd.Parameters.AddWithValue
                    ("@registration_staff_id", receiptsAndExpenditure.RegistrationRep.ID);
                Cmd.Parameters.AddWithValue("@credit_dept_id", receiptsAndExpenditure.CreditDept.ID);
                Cmd.Parameters.AddWithValue("@content_id", receiptsAndExpenditure.Content.ID);
                Cmd.Parameters.AddWithValue("@detail", receiptsAndExpenditure.Detail);
                Cmd.Parameters.AddWithValue("@price", receiptsAndExpenditure.Price);
                Cmd.Parameters.AddWithValue("@is_payment", receiptsAndExpenditure.IsPayment);
                Cmd.Parameters.AddWithValue("@is_validity", receiptsAndExpenditure.IsValidity);
                Cmd.Parameters.AddWithValue
                    ("@is_reduced_tax_rate", receiptsAndExpenditure.IsReducedTaxRate);
                return Cmd.ExecuteNonQuery();
            }
        }

        public  ObservableCollection<ReceiptsAndExpenditure> ReferenceReceiptsAndExpenditure
            (
                DateTime registrationDateStart, DateTime registrationDateEnd, string location, string creditDept,
                string content,string detail, string accountingSubject, string accountingSubjectCode, 
                bool whichDepositAndWithdrawalOnly, bool isPayment,bool isContainOutputted, 
                bool isValidityOnly, DateTime accountActivityDateStart, DateTime accountActivityDateEnd,
                DateTime outputDateStart, DateTime outputDateEnd
            )
        {
            ObservableCollection<ReceiptsAndExpenditure> list =
                new ObservableCollection<ReceiptsAndExpenditure>();
            SqlCommand Cmd = new SqlCommand();
            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd, "reference_receipts_and_expenditure_all_data");
                Cmd.Parameters.AddWithValue("@location", location);
                Cmd.Parameters.AddWithValue("@account_activity_date_start", accountActivityDateStart);
                Cmd.Parameters.AddWithValue("@account_activity_date_end", accountActivityDateEnd);
                Cmd.Parameters.AddWithValue("@registration_date_start", registrationDateStart);
                Cmd.Parameters.AddWithValue("@registration_date_end", registrationDateEnd);
                Cmd.Parameters.AddWithValue("@credit_dept", creditDept);
                Cmd.Parameters.AddWithValue("@accounting_subject_code", accountingSubjectCode);
                Cmd.Parameters.AddWithValue("@content", content);
                Cmd.Parameters.AddWithValue("@detail", detail);
                Cmd.Parameters.AddWithValue("@limiting_is_payment", whichDepositAndWithdrawalOnly);
                Cmd.Parameters.AddWithValue("@is_payment", isPayment);
                Cmd.Parameters.AddWithValue("@contain_outputted", isContainOutputted);
                Cmd.Parameters.AddWithValue("@validity_true_only", isValidityOnly);
                Cmd.Parameters.AddWithValue("@output_date_start", outputDateStart);
                Cmd.Parameters.AddWithValue("@output_date_end", outputDateEnd);
                Rep paramRep;
                CreditDept paramCreditDept;
                AccountingSubject paramAccountingSubject;
                Content paramContent;
                using SqlDataReader dataReader= Cmd.ExecuteReader();

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
            }
            return list;
        }

        public Rep CallRep(string id)
        {
            SqlCommand Cmd = new SqlCommand();
            SqlDataReader dataReader;
            Rep rep=default;

            using(Cn)
            {
                dataReader = ReturnReaderCommandOneParameterStoredProc(Cmd, "call_staff", "@staff_id", id);

                while(dataReader.Read())
                    rep = 
                        new Rep((string)dataReader["staff_id"], (string)dataReader["name"], 
                            (string)dataReader["password"], (bool)dataReader["is_validity"], 
                            (bool)dataReader["is_permission"]);
            }
            return rep;
        }

        public CreditDept CallCreditDept(string id)
        {
            SqlCommand Cmd = new SqlCommand();
            SqlDataReader dataReader;
            CreditDept creditDept = default;

            using(Cn)
            {
                dataReader = 
                    ReturnReaderCommandOneParameterStoredProc
                        (Cmd, "call_credit_dept", "@credit_dept_id", id);

                while (dataReader.Read())
                    creditDept = new CreditDept((string)dataReader["credit_dept_id"], (string)dataReader["dept"], 
                        (bool)dataReader["is_validity"],(bool)dataReader["is_shunjuen_dept"]);
            }
            return creditDept;
        }

        public Content CallContent(string id)
        {
            SqlCommand Cmd = new SqlCommand();
            SqlDataReader dataReader;
            Content content = default;

            using(Cn)
            {
                dataReader = 
                    ReturnReaderCommandOneParameterStoredProc(Cmd, "call_content", "@content_id", id);

                while (dataReader.Read())
                    content = new Content
                        (
                            (string)dataReader["content_id"],
                            new AccountingSubject((string)dataReader["accounting_subject_id"], 
                            (string)dataReader["subject_code"], (string)dataReader["subject"], true),
                            (int)dataReader["flat_rate"], (string)dataReader["content"], (bool)dataReader["is_validity"]
                        );
            }
            return content;
        }

        public int Update(ReceiptsAndExpenditure receiptsAndExpenditure)
        {
            SqlCommand Cmd = new SqlCommand();

            using(Cn)
            {
                ADO_NewInstance_StoredProc(Cmd, "update_receipts_and_expenditure");
                Cmd.Parameters.AddWithValue("@receipts_and_expenditure_id", receiptsAndExpenditure.ID);
                Cmd.Parameters.AddWithValue("@location", receiptsAndExpenditure.Location);
                Cmd.Parameters.AddWithValue
                    ("@account_activity_date", receiptsAndExpenditure.AccountActivityDate);
                Cmd.Parameters.AddWithValue("@credit_dept_id", receiptsAndExpenditure.CreditDept.ID);
                Cmd.Parameters.AddWithValue("@content_id", receiptsAndExpenditure.Content.ID);
                Cmd.Parameters.AddWithValue("@detail", receiptsAndExpenditure.Detail);
                Cmd.Parameters.AddWithValue("@price", receiptsAndExpenditure.Price);
                Cmd.Parameters.AddWithValue("@is_payment", receiptsAndExpenditure.IsPayment);
                Cmd.Parameters.AddWithValue("@is_validity", receiptsAndExpenditure.IsValidity);
                Cmd.Parameters.AddWithValue("@is_unprinted", receiptsAndExpenditure.IsUnprinted);
                Cmd.Parameters.AddWithValue("@operation_staff_id", LoginRep.Rep.ID);
                Cmd.Parameters.AddWithValue
                    ("@is_reduced_tax_rate", receiptsAndExpenditure.IsReducedTaxRate);
                return Cmd.ExecuteNonQuery();
            }
        }

        public int PreviousDayFinalAmount()
        {
            SettingConectionString();
            SqlCommand Cmd = new SqlCommand("select dbo.return_previous_day_final_amount()",Cn);
            Cn.Open();
            object obj;
            using (Cn) obj = Cmd.ExecuteScalar();

            return (int)obj;
        }

        public int RegistrationPerMonthFinalAccount()
        {
            SqlCommand Cmd = new SqlCommand();

            using(Cn)
            {
                ADO_NewInstance_StoredProc(Cmd, "registration_final_account_per_month_table");
                return Cmd.ExecuteNonQuery();
            }
        }

        public int CallFinalAccountPerMonth()
        {
            SqlCommand Cmd = new SqlCommand();
            int i=default;

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd, "call_final_account_per_month");
                DateTime previousMonthLastDay =
                    DateTime.Today.AddDays(-1 * (DateTime.Today.Day - 1)).AddDays(-1);
                Cmd.Parameters.AddWithValue("@date", previousMonthLastDay);

                SqlDataReader sdr = Cmd.ExecuteReader();

                while (sdr.Read()) i = (int)sdr["amount"];
            }
            return i;
        }

        public int ReceiptsAndExpenditurePreviousDayChange(ReceiptsAndExpenditure receiptsAndExpenditure)
        {
            SqlCommand Cmd = new SqlCommand();

            using (Cn)
            {
                ADO_NewInstance_StoredProc(Cmd, "update_receipts_and_expenditure");
                Cmd.Parameters.AddWithValue("@receipts_and_expenditure_id", receiptsAndExpenditure.ID);
                return Cmd.ExecuteNonQuery();
            }
        }

        public (int TotalRows, ObservableCollection<ReceiptsAndExpenditure> List)
            ReferenceReceiptsAndExpenditure(DateTime registrationDateStart, DateTime registrationDateEnd, 
                string location, string creditDept, string content, string detail, string accountingSubject, string accountingSubjectCode, 
                bool whichDepositAndWithdrawalOnly, bool isPayment, bool isContainOutputted, bool isValidityOnly, 
                DateTime accountActivityDateStart, DateTime accountActivityDateEnd, DateTime outputDateStart, 
                DateTime outputDateEnd, int pageCount)
        {
            ObservableCollection<ReceiptsAndExpenditure> list =
                new ObservableCollection<ReceiptsAndExpenditure>();
            int totalRows = default;
            SqlCommand Cmd = new SqlCommand();
            using (Cn)
            {
                SettingConectionString();
                Cmd.Connection = Cn;
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = $"select count(*)as return_count from reference_receipts_and_expenditure_data_view\r\n" +
                    $"where location like '%' + '{location}' + '%'\r\n" +
                    $"and account_activity_date between '{accountActivityDateStart.ToShortDateString()}' and" +
                    $" '{accountActivityDateEnd.ToShortDateString()}'\r\n" +
                    $"and dept like '%' + '{creditDept}' + '%'\r\n" +
                    $"and subject_code like '%'+'{accountingSubjectCode}'+'%'\r\n" +
                    $"and content like '%' + '{content}' + '%'\r\n" +
                    $"and detail like '%' + '{detail}' + '%'\r\n" +
                    $"and is_payment =case when '{whichDepositAndWithdrawalOnly}'=" +
                    $" 'true' then '{isPayment}' else is_payment end\r\n" +
                    $"and output_date between '{outputDateStart.ToShortDateString()}' and '{outputDateEnd.ToShortDateString()}'\r\n" +
                    $"and is_validity = case when '{isValidityOnly}' = 'true' then 'true' else is_validity end ";
                Cn.Open();
                using SqlDataReader countReader = Cmd.ExecuteReader();

                while (countReader.Read()) totalRows = (int)countReader["return_count"];

                ADO_NewInstance_StoredProc(Cmd, "reference_receipts_and_expenditure");
                Cmd.Parameters.AddWithValue("@location", location);
                Cmd.Parameters.AddWithValue("@account_activity_date_start", accountActivityDateStart);
                Cmd.Parameters.AddWithValue("@account_activity_date_end", accountActivityDateEnd);
                Cmd.Parameters.AddWithValue("@registration_date_start", registrationDateStart);
                Cmd.Parameters.AddWithValue("@registration_date_end", registrationDateEnd);
                Cmd.Parameters.AddWithValue("@credit_dept", creditDept);
                Cmd.Parameters.AddWithValue("@accounting_subject_code", accountingSubjectCode);
                Cmd.Parameters.AddWithValue("@content", content);
                Cmd.Parameters.AddWithValue("@detail", detail);
                Cmd.Parameters.AddWithValue("@limiting_is_payment", whichDepositAndWithdrawalOnly);
                Cmd.Parameters.AddWithValue("@is_payment", isPayment);
                Cmd.Parameters.AddWithValue("@contain_outputted", isContainOutputted);
                Cmd.Parameters.AddWithValue("@validity_true_only", isValidityOnly);
                Cmd.Parameters.AddWithValue("@output_date_start", outputDateStart);
                Cmd.Parameters.AddWithValue("@output_date_end", outputDateEnd);
                Cmd.Parameters.AddWithValue("@page", pageCount);
                Rep paramRep;
                CreditDept paramCreditDept;
                AccountingSubject paramAccountingSubject;
                Content paramContent;
                using SqlDataReader dataReader = Cmd.ExecuteReader();

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
            }
            return (totalRows, list);
        }

        public Dictionary<int, string> GetSoryoList()
        {
            Cn = new SqlConnection
            {
                ConnectionString =
                "Data Source=192.168.44.163\\SQLEXPRESS;Initial Catalog=SingyoujiDataBase;" +
                "User ID=sa;Password=sqlserver"
            };

            Cn.Open();

            SqlCommand Cmd = new SqlCommand
            {
                Connection = Cn,
                CommandType = CommandType.Text,
                CommandText = "select * from PersonInChargeMaster"
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
            throw new NotImplementedException();
        }
    }
}
