using FontAwesome6;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CESI_WPF_2023.Converters
{
    public class TypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = Colors.Transparent;
            var type = value.ToString();
            switch (type.ToLowerInvariant())
            {
                case "feu":
                    color = (Color)Application.Current.FindResource("ColorTypeFeu");
                    break;
                case "électrik":
                    color = Colors.Yellow;
                    break;
                case "plante":
                    color = Colors.Green;
                    break;
                case "poison":
                    color = Colors.Purple;
                    break;
                case "vol":
                    color = Colors.Gray;
                    break;
                case "psy":
                    color = Colors.GreenYellow;
                    break;
                case "eau":
                    color = Colors.Blue;
                    break;
                default:
                    color = Colors.Transparent;
                    break;
            }   
            
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
