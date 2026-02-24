using System.Windows;
using System.Windows.Controls;

namespace SAOResoForm.Dati
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BoundPassword",
                typeof(string),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPasswordProperty =
            DependencyProperty.RegisterAttached(
                "BindPassword",
                typeof(bool),
                typeof(PasswordBoxHelper),
                new PropertyMetadata(false, OnBindPasswordChanged));

        private static bool _updating;

        public static string GetBoundPassword(DependencyObject dp)
            => (string)dp.GetValue(BoundPasswordProperty);

        public static void SetBoundPassword(DependencyObject dp, string value)
            => dp.SetValue(BoundPasswordProperty, value);

        public static bool GetBindPassword(DependencyObject dp)
            => (bool)dp.GetValue(BindPasswordProperty);

        public static void SetBindPassword(DependencyObject dp, bool value)
            => dp.SetValue(BindPasswordProperty, value);

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox box)
            {
                if ((bool)e.OldValue)
                    box.PasswordChanged -= HandlePasswordChanged;
                if ((bool)e.NewValue)
                    box.PasswordChanged += HandlePasswordChanged;
            }
        }

        private static void OnBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            if (dp is PasswordBox box && !_updating)
                box.Password = (string)e.NewValue ?? string.Empty;
        }

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox box)
            {
                _updating = true;
                SetBoundPassword(box, box.Password);
                _updating = false;
            }
        }
    }
}