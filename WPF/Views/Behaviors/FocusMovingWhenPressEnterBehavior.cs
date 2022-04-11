using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Input;

namespace WPF.Views.Behaviors
{
    /// <summary>
    /// Enter押下で次のコントロールにフォーカスを移動させるBehavior
    /// </summary>
    public class FocusMovingWhenPressEnterBehavior
    {
        public static readonly DependencyProperty PressEnterCommand =
            DependencyProperty.RegisterAttached
                ("PressEnter", typeof(bool), typeof(FocusMovingWhenPressEnterBehavior),
                    new UIPropertyMetadata(false,PressEnterChanged));

        public static bool GetPressEnterCommand(DependencyObject obj) =>
            (bool)obj.GetValue(PressEnterCommand);        

        public static void SetPressEnterCommand(DependencyObject obj, bool value)
        {
            obj.SetValue(PressEnterCommand, value) ;
        }
        public static void PressEnterChanged
            (DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is UIElement element)) { return; }

            if (GetPressEnterCommand(element))
            { element.KeyDown += KeyDown; }
            else { element.KeyDown -= KeyDown; }
        }

        private static void KeyDown(object sender, KeyEventArgs e)
        {
            UIElement element = sender as UIElement;

            if ((Keyboard.Modifiers == ModifierKeys.None) && (e.Key == Key.Enter))
            {
                element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            
            if ((Keyboard.Modifiers == ModifierKeys.Shift) && (e.Key == Key.Enter))
            {
                element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }
        }
    }
}
