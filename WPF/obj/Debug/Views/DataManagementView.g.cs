﻿#pragma checksum "..\..\..\Views\DataManagementView.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "62C5418700F9AA417FB01527D3468A97938DBF86BDDA239BE8039E1A38D79391"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

using CypherBoxControl;
using Domain.Entities.Helpers;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Expression.Interactivity.Input;
using Microsoft.Expression.Interactivity.Layout;
using Microsoft.Expression.Interactivity.Media;
using Microsoft.Xaml.Behaviors;
using Microsoft.Xaml.Behaviors.Core;
using Microsoft.Xaml.Behaviors.Input;
using Microsoft.Xaml.Behaviors.Layout;
using Microsoft.Xaml.Behaviors.Media;
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
using System.Windows.Interactivity;
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
    /// DataManagementView
    /// </summary>
    public partial class DataManagementView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 92 "..\..\..\Views\DataManagementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox RepNameTextBox;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\Views\DataManagementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CypherBoxControl.CypherBox RepCurrentPasswordCypherBox;
        
        #line default
        #line hidden
        
        
        #line 121 "..\..\..\Views\DataManagementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CypherBoxControl.CypherBox RepNewPasswordCypherBox;
        
        #line default
        #line hidden
        
        
        #line 140 "..\..\..\Views\DataManagementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CypherBoxControl.CypherBox ConfirmationPasswordCypherBox;
        
        #line default
        #line hidden
        
        
        #line 152 "..\..\..\Views\DataManagementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RepRegistrationButton;
        
        #line default
        #line hidden
        
        
        #line 173 "..\..\..\Views\DataManagementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox ReferenceRepTextBox;
        
        #line default
        #line hidden
        
        
        #line 178 "..\..\..\Views\DataManagementView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid RepListDataGrid;
        
        #line default
        #line hidden
        
        
        #line 201 "..\..\..\Views\DataManagementView.xaml"
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
            System.Uri resourceLocater = new System.Uri("/春秋苑経理システム;component/views/datamanagementview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\DataManagementView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.RepNameTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 2:
            this.RepCurrentPasswordCypherBox = ((CypherBoxControl.CypherBox)(target));
            return;
            case 3:
            this.RepNewPasswordCypherBox = ((CypherBoxControl.CypherBox)(target));
            return;
            case 4:
            this.ConfirmationPasswordCypherBox = ((CypherBoxControl.CypherBox)(target));
            return;
            case 5:
            this.RepRegistrationButton = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.ReferenceRepTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 7:
            this.RepListDataGrid = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 8:
            this.CloseButton = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

