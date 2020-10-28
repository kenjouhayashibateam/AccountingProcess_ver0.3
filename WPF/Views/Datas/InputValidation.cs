using System;
using System.Globalization;
using System.Windows.Controls;

namespace WPF.Views.Datas
{
    public class InputValidation : ValidationRule
    {
        public bool IsPasswordMatched(string originalPassword,string inputPassword)
        {
            return originalPassword == inputPassword;
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return ValidationResult.ValidResult;
        }
    }
}
