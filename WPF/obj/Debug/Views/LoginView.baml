   M S B A M L   `   `   `  ����V  R春秋苑経理システム, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:WPF.Views	WPF.Views  5   	   /clr-namespace:WPF.ViewModelsWPF.ViewModels  5   	   9!clr-namespace:WPF.Views.BehaviorsWPF.Views.Behaviors  5   	       WPF.Views.LoginView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:WPF.Views     5   	   +vmclr-namespace:WPF.ViewModels     5   	   b ^SharpVectors.Converters.Wpf, Version=1.7.5.0, Culture=neutral, PublicKeyToken=b532964b8548be771svgc&http://sharpvectors.codeplex.com/svgc/  5   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5	   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a4ib+http://schemas.microsoft.com/xaml/behaviors  5
   	   K GCypherBoxControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3control%https://github.com/qtbui159/CypherBox  5   	   0behavior!clr-namespace:WPF.Views.Behaviors  5   	   h	 dMicrosoft.Expression.Interactions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Linteractions9http://schemas.microsoft.com/expression/2010/interactions 	 5   	   $	��250��5   '   $	��300��64     � Title  6	   �� *WindowTitle+ +� FocusedElement 6@   ��  �� ElementName$ RepsComboBox�� � DataContext 5      #   WPF.ViewModels.LoginViewModel  5   
   .  (System.Windows.Interactivity.Interaction   Triggers 5      D 	 >Microsoft.Expression.Interactivity.Core.PropertyChangedTrigger  5   
      Binding 6.   �� *
MessageBox+.  (System.Windows.Interactivity.TriggerBase   Actions. 5      ,   &WPF.Views.Behaviors.MessageBoxBehavior  5         	Behaviors 5      ,   &WPF.Views.Behaviors.SystemMenuBehavior  5   
      CanMinimize$ FalseX�6&   *  $Microsoft.Xaml.Behaviors.Interaction	   Triggers	 5      /  )Microsoft.Xaml.Behaviors.Core.DataTrigger  5   
   
   Value$

 true��6@   :	  4Microsoft.Xaml.Behaviors.Core.PropertyChangedTrigger 	  Binding 6   �� *CallShowMessageBox+*
  $Microsoft.Xaml.Behaviors.TriggerBase 
  Actions. 5      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 6%   �� *MessageBoxCommand+5         5!   
   $

 true��6>    6   �� *WindowCloseSwich+. 5"         6%   �� *WindowCloseCommand+   CommandParameter 6L   ��  �� RelativeSource ��  �� Mode$ FindAncestor=� �� AncestorType" �5#      .��5&      �  � RowDefinitions 5'   
   �� 5(      $��1* �6   �� 5)      $��1* �6   �� 5*      $��1* �6   �� 5+      $��1* �6   �� 5,      $��1* �6   .+�5.   
   �� $��	担当者��6   $
��Left=�6   $��Bottom=�6:   $��5q�6U   �� 5/   
   -   5        $��RepsComboBox��5/      $��1��6'   ,   &WPF.Views.Behaviors.ComboBoxAttachment   IsIMEModeOnGotFocus . $��5q�6k   $��Center=�6v    �� DisplayMemberPath$
 Name��6�    �� SelectedIndex$ 0��6�    �� 
IsEditable . ��6�   �� *Reps+ �� SelectedValue 6�   �� *
CurrentRep+ �� Text 6  �� *	RepName+ �� UpdateSourceTrigger$ 	LostFocus=� �� SelectedValuePath 6N  �� *CurrentRep.ID+�� 50   
   $��パスワード��6   $��2��6!   $
��Left=�6.   $��Bottom=�6I   $��5q�6d   � 51   
   $��3��6   $��5q�6    � ColumnDefinitions 52      �� 53      $��8* �6#   �� 54      $��2* �6#   �� 55      $��1* �6#   .+�57      �� $��3��6   ���	#FFABADB3$��1q�6C    �� CornerRadius$ 5��6W   6k      CypherBoxControl.CypherBox  58      -   5          Name$! RepCurrentPasswordCypherBox��58       $��Stretch=�6E   $��Stretch=�6a   $��0q�59   h   $��Center=�6|      Placeholder$ your password��5:   <      
CypherText 59   <   �� *
Password+  �� Mode$  TwoWay=�!   ShowPlainText! 5:   X   �� *PasswordCharCheck+$  OneWay=��� 5<      $��1��6   $��Right=�6T   ��6%   �� *PasswordCheckReversCommand+��5=      �� 5>      " �� 
TargetType"" ��6&   .R�5?      �� $��../Resources/eye.png��6    5@      5B      5C      � 5D   
   $��4��6    5E      �� 5F      $��3* �6#   �� 5G      $��1* �6#   �� 5H      $��1* �6#   .+�5J      �� -   5        # ��Name$# LoginButton��5J      $��1��6*   $��ログイン��6:   $
��3,10q�6t   {�6I   �� *IsLoginButtonEnabled+$ J� InputBindings$ 5K      d� 5L      % d� MouseAction$% 	LeftClicke�6D   & �� Command& 6#   �� *LoginCommand+6D   �� 5M      
' �� Key$' Space��6B   & 6!   �� *LoginCommand+6B   5O      �� 5P      -   5        $# CloseButton��5P      $��	閉じる��6*   $��2��68   $
��3,10q�6H   $ 5Q      d� 5R      $% 	LeftClicke�6�   & 6#   �� *WindowCloseCommand+( �� CommandParameter( 6J   ��  �� $ FindAncestor=�" �6�   �� 5S      $' Space��6�   & 6!   �� *WindowCloseCommand+( 6H   ��  �� $ FindAncestor=�" �6�   5U      5V      5W      5X      