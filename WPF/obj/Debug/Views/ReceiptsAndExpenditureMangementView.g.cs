﻿#pragma checksum "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "69811CDB8D07D2F7DC8919A13FE29969964CBD3868CF65CE72F9CE134F7C1DA2"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WPF.ViewModels;
using WPF.Views;
using WPF.Views.Behaviors;


namespace WPF.Views {
    
    
    /// <summary>
    /// ReceiptsAndExpenditureMangementView
    /// </summary>
    public partial class ReceiptsAndExpenditureMangementView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton RegistrationRadioButton;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton DepositAndWithdrawalToggleButton;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CreditAccountComboBox;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox ContentComboBox;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox AccountingSubjectComboBox;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox AccountingSubjectCodeComboBox;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox DetailTextBox;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox PriceTextBox;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker AccountActivityTextBox;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker RegistrationDateTextBox;
        
        #line default
        #line hidden
        
        
        #line 119 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox RegistrationRepNameTextBox;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button OperationButton;
        
        #line default
        #line hidden
        
        
        #line 180 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ReceiptsAndExpenditureDataGrid;
        
        #line default
        #line hidden
        
        
        #line 263 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/春秋苑経理システム;component/views/receiptsandexpendituremangementview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ReceiptsAndExpenditureMangementView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.RegistrationRadioButton = ((System.Windows.Controls.RadioButton)(target));
            return;
            case 2:
            this.DepositAndWithdrawalToggleButton = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            return;
            case 3:
            this.CreditAccountComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.ContentComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 5:
            this.AccountingSubjectComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.AccountingSubjectCodeComboBox = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.DetailTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 8:
            this.PriceTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.AccountActivityTextBox = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 10:
            this.RegistrationDateTextBox = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 11:
            this.RegistrationRepNameTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            this.OperationButton = ((System.Windows.Controls.Button)(target));
            return;
            case 13:
            this.ReceiptsAndExpenditureDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 14:
            this.CloseButton = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

