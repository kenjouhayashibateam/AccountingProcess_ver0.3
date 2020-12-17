using System;
using System.Globalization;
using System.Windows.Controls;

namespace WPF.Views.Datas
{
    /// <summary>
    /// エラー検証クラス※ラベル等にエラーメッセージを出力する方法を模索する
    /// </summary>
    public class InputValidation : ValidationRule
    {
        public bool IsPasswordMatched(string originalPassword, string inputPassword) => originalPassword == inputPassword;
        public override ValidationResult Validate(object value, CultureInfo cultureInfo) => ValidationResult.ValidResult;
    }
}
