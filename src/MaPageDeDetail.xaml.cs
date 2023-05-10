using CESI_WPF_2023.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CESI_WPF_2023
{
    /// <summary>
    /// Interaction logic for MaPageDeDetail.xaml
    /// </summary>
    public partial class MaPageDeDetail : Window
    {
        public MaPageDeDetail(PokemonModel pokemon)
        {
            DataContext = new DetailViewModel(pokemon);
            InitializeComponent();
        }
    }
}
