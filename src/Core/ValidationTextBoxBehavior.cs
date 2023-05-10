using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CESI_WPF_2023.Core
{
    public static class ValidationTextBoxBehavior
    {
        public static Color GetErrorBackgroundColor(DependencyObject obj)
        {
            return (Color)obj.GetValue(ErrorBackgroundColorProperty);
        }

        public static void SetErrorBackgroundColor(DependencyObject obj, Color value)
        {
            obj.SetValue(ErrorBackgroundColorProperty, value);
        }

        public static readonly DependencyProperty ErrorBackgroundColorProperty = DependencyProperty.RegisterAttached(
            "ErrorBackgroundColor",
            typeof(Color),
            typeof(ValidationTextBoxBehavior),
            new FrameworkPropertyMetadata(Colors.Transparent, ErrorBackgroundColorChangedCallback));

        private static void ErrorBackgroundColorChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if(textBox != null)
            {
                textBox.TextChanged += TextBox_TextChanged;
            }
        }

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if(string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Background = new SolidColorBrush(GetErrorBackgroundColor(textBox));
            }
            else
            {
                textBox.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}
