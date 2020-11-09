
using System.Collections.Generic;

namespace ShowMeTheXAML
{
    public static class XamlDictionary
    {
        static XamlDictionary()
        {
            XamlResolver.Set("color_zones_inverted", @"<smtx:XamlDisplay UniqueKey=""color_zones_inverted"" xmlns:smtx=""clr-namespace:ShowMeTheXAML;assembly=ShowMeTheXAML"">
  <materialDesign:ColorZone Mode=""Inverted"" Padding=""16"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"">
    <DockPanel xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
      <materialDesign:PopupBox DockPanel.Dock=""Right"" PlacementMode=""BottomAndAlignRightEdges"">
        <ListBox>
          <ListBoxItem Content=""Hello World"" />
          <ListBoxItem Content=""Nice Popup"" />
          <ListBoxItem Content=""Goodbye"" />
        </ListBox>
      </materialDesign:PopupBox>
      <StackPanel Orientation=""Horizontal"">
        <ToggleButton Style=""{DynamicResource MaterialDesignHamburgerToggleButton}"" />
        <TextBlock VerticalAlignment=""Center"" Margin=""16 0 0 0"" Text=""Material Design In XAML Toolkit"" />
      </StackPanel>
    </DockPanel>
  </materialDesign:ColorZone>
</smtx:XamlDisplay>");
XamlResolver.Set("color_zones_primary_light", @"<smtx:XamlDisplay UniqueKey=""color_zones_primary_light"" xmlns:smtx=""clr-namespace:ShowMeTheXAML;assembly=ShowMeTheXAML"">
  <materialDesign:ColorZone Mode=""PrimaryLight"" Padding=""16"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"">
    <StackPanel Orientation=""Horizontal"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
      <ToggleButton Style=""{DynamicResource MaterialDesignHamburgerToggleButton}"" />
      <TextBlock VerticalAlignment=""Center"" Margin=""16 0 0 0"" Text=""Material Design In XAML Toolkit"" />
    </StackPanel>
  </materialDesign:ColorZone>
</smtx:XamlDisplay>");
XamlResolver.Set("color_zones_primary_mid", @"<smtx:XamlDisplay UniqueKey=""color_zones_primary_mid"" xmlns:smtx=""clr-namespace:ShowMeTheXAML;assembly=ShowMeTheXAML"">
  <materialDesign:ColorZone Mode=""PrimaryMid"" Padding=""16"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"">
    <DockPanel xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
      <ToggleButton Style=""{DynamicResource MaterialDesignSwitchAccentToggleButton}"" VerticalAlignment=""Center"" DockPanel.Dock=""Right"" />
      <StackPanel Orientation=""Horizontal"" materialDesign:RippleAssist.IsCentered=""True"">
        <ToggleButton Style=""{DynamicResource MaterialDesignHamburgerToggleButton}"" />
        <ComboBox SelectedIndex=""0"" Margin=""8 0 0 0"" BorderThickness=""0"" materialDesign:ColorZoneAssist.Mode=""Standard"" materialDesign:TextFieldAssist.UnderlineBrush=""{DynamicResource  MaterialDesignPaper}"" BorderBrush=""{DynamicResource MaterialDesignPaper}"">
          <ComboBoxItem Content=""Android"" />
          <ComboBoxItem Content=""iOS"" />
          <ComboBoxItem Content=""Linux"" />
          <ComboBoxItem Content=""Windows"" />
        </ComboBox>
        <materialDesign:ColorZone Mode=""Standard"" Padding=""8 4 8 4"" CornerRadius=""2"" Panel.ZIndex=""1"" Margin=""16 0 0 0"" materialDesign:ShadowAssist.ShadowDepth=""Depth1"">
          <Grid>
            <Grid.ColumnDefinitions>
              <ColumnDefinition Width=""Auto"" />
              <ColumnDefinition Width=""*"" />
              <ColumnDefinition Width=""Auto"" />
            </Grid.ColumnDefinitions>
            <Button Style=""{DynamicResource MaterialDesignToolButton}"">
              <materialDesign:PackIcon Kind=""Search"" Opacity="".56"" />
            </Button>
            <TextBox Grid.Column=""1"" Margin=""8 0 0 0"" materialDesign:HintAssist.Hint=""Build a search bar"" materialDesign:TextFieldAssist.DecorationVisibility=""Hidden"" BorderThickness=""0"" MinWidth=""200"" VerticalAlignment=""Center"" />
            <Button Style=""{DynamicResource MaterialDesignToolButton}"" Grid.Column=""2"">
              <materialDesign:PackIcon Kind=""Microphone"" Opacity="".56"" Margin=""8 0 0 0"" />
            </Button>
          </Grid>
        </materialDesign:ColorZone>
        <Button Style=""{DynamicResource MaterialDesignToolForegroundButton}"" Margin=""8 0 0 0"" Panel.ZIndex=""0"">
          <materialDesign:PackIcon Kind=""Send"" />
        </Button>
      </StackPanel>
    </DockPanel>
  </materialDesign:ColorZone>
</smtx:XamlDisplay>");
XamlResolver.Set("color_zones_primary_dark", @"<smtx:XamlDisplay UniqueKey=""color_zones_primary_dark"" Padding=""10"" xmlns:smtx=""clr-namespace:ShowMeTheXAML;assembly=ShowMeTheXAML"">
  <materialDesign:ColorZone Mode=""PrimaryDark"" Padding=""16"" CornerRadius=""10"" materialDesign:ShadowAssist.ShadowDepth=""Depth3"" ClipToBounds=""False"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"">
    <StackPanel Orientation=""Horizontal"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
      <ToggleButton Style=""{DynamicResource MaterialDesignHamburgerToggleButton}"" />
      <TextBlock VerticalAlignment=""Center"" Margin=""16 0 0 0"" Text=""Material Design In XAML Toolkit"" />
    </StackPanel>
  </materialDesign:ColorZone>
</smtx:XamlDisplay>");
XamlResolver.Set("color_zones_secondary_mid", @"<smtx:XamlDisplay UniqueKey=""color_zones_secondary_mid"" xmlns:smtx=""clr-namespace:ShowMeTheXAML;assembly=ShowMeTheXAML"">
  <materialDesign:ColorZone Mode=""Dark"" Padding=""16"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"">
    <StackPanel Orientation=""Horizontal"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
      <ToggleButton Style=""{DynamicResource MaterialDesignHamburgerToggleButton}"" />
      <TextBlock VerticalAlignment=""Center"" Margin=""16 0 0 0"" Text=""Material Design In XAML Toolkit"" />
    </StackPanel>
  </materialDesign:ColorZone>
</smtx:XamlDisplay>");
XamlResolver.Set("color_zones_custom", @"<smtx:XamlDisplay UniqueKey=""color_zones_custom"" Margin=""0 16"" xmlns:smtx=""clr-namespace:ShowMeTheXAML;assembly=ShowMeTheXAML"">
  <materialDesign:ColorZone Mode=""Custom"" Background=""Black"" Foreground=""White"" Padding=""16"" ClipToBounds=""False"" xmlns:materialDesign=""http://materialdesigninxaml.net/winfx/xaml/themes"">
    <StackPanel Orientation=""Horizontal"" xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
      <ToggleButton Style=""{DynamicResource MaterialDesignHamburgerToggleButton}"" />
      <TextBlock VerticalAlignment=""Center"" Margin=""16 0"" Text=""Material Design In XAML Toolkit"" />
      <materialDesign:Badged Badge=""123"" VerticalAlignment=""Center"">
        <Button Content=""Some action"" />
      </materialDesign:Badged>
    </StackPanel>
  </materialDesign:ColorZone>
</smtx:XamlDisplay>");
        }
    }
}