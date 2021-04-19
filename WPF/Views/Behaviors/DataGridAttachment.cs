using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPF.Views.Behaviors
{
    /// <summary>
    /// DataGridアタッチメント
    /// </summary>
    public static class DataGridAttachment
    {
        //以下のDataGridのソートを昇順→降順→ソート無しに設定する添付プロパティはビルド時にCOMの参照警告、
        //プロパティのセッターエラーが出るため使えない。要検証
        //    public static bool GetIsSortCustomize(DependencyObject obj)
        //    {
        //        return (bool)obj.GetValue(IsSortCustomizeProperty);
        //    }

        //    public static DependencyProperty IsSortCustomizeProperty =
        //        DependencyProperty.RegisterAttached("IsSortCustomize", typeof(bool),
        //        typeof(DataGridAttachment), new PropertyMetadata(OnIsSortCustomizeChanged));

        //    private static void OnIsSortCustomizeChanged
        //          (DependencyObject obj, DependencyPropertyChangedEventArgs e)
        //    {
        //        if (!(obj is DataGrid dataGrid)) return;
        //        if ((bool)e.NewValue) dataGrid.Sorting += DataGrid_Sorting;
        //        else dataGrid.Sorting -= DataGrid_Sorting;
        //    }

        //    private static void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        //    {
        //        if (!(sender is DataGrid dataGrid)) return;

        //        var listColView = (ListCollectionView)CollectionViewSource.GetDefaultView(dataGrid);
        //        if (listColView == null) return;

        //        if (e.Column.SortDirection == ListSortDirection.Descending)
        //        {
        //            e.Handled = true;
        //            e.Column.SortDirection = null;
        //            dataGrid.Items.SortDescriptions.Clear();
        //        }
        //    }
    }
}
