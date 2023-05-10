using CESI_WPF_2023.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace CESI_WPF_2023.Converters
{
    public class PokemonStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var state = (PokemonDataState)value;
            return state.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString() switch
            {
                "Inconnu" => PokemonDataState.Inconnu,
                "Vu" => PokemonDataState.Vu,
                "Capture" => PokemonDataState.Capture,
                _ => throw new NotSupportedException()
            };
            //switch (value.ToString())
            //{
            //    case "Inconnu":
            //        return PokemonDataState.Inconnu;
            //    case "Vu":
            //        return PokemonDataState.Vu;
            //    case "IncoCapturennu":
            //        return PokemonDataState.Capture;
            //    default:
            //        throw new NotSupportedException();
            //}
        }
    }
}
