using System;
using System.Windows;
using System.Windows.Controls;

namespace CESI_WPF_2023
{
    public class WatermarkTextBox : TextBox
    {
        private TextBlock _watermark;

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if(string.IsNullOrEmpty(Text))
            {
                _watermark.Visibility = Visibility.Visible;
            }
            else
            {
                _watermark.Visibility = Visibility.Collapsed;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _watermark = (TextBlock)GetTemplateChild("PART_Watermak");
            OnNewWatermarkApply(Watermark);
        }

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register(
                "Watermark", 
                typeof(string), 
                typeof(WatermarkTextBox), 
                new PropertyMetadata(null, propertyChangedCallback: OnWatermarkChanged));

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var watermarkTextBox = d as WatermarkTextBox;
            watermarkTextBox.OnNewWatermarkApply(e.NewValue?.ToString());
        }

        private void OnNewWatermarkApply(string? newWatermark)
        {
            if(_watermark != null)
            {
                if (!string.IsNullOrEmpty(newWatermark))
                {
                    _watermark.Visibility = Visibility.Visible;
                }
                else
                {
                    _watermark.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
