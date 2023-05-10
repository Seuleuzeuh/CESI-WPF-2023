using FontAwesome6;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CESI_WPF_2023.Converters
{
    public class TypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = value.ToString();
            switch (type.ToLowerInvariant())
            {
                case "feu":
                    return EFontAwesomeIcon.Solid_Fire;
                case "électrik":
                    return EFontAwesomeIcon.Solid_BoltLightning;
                case "plante":
                    return EFontAwesomeIcon.Solid_PlantWilt;
                case "poison":
                    return EFontAwesomeIcon.Solid_Biohazard;
                case "vol":
                    return EFontAwesomeIcon.Brands_FantasyFlightGames;
                case "psy":
                    return EFontAwesomeIcon.Solid_Heading;
                case "eau":
                    return EFontAwesomeIcon.Solid_Water;
                default:
                    return EFontAwesomeIcon.Solid_Fan;
            }            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
