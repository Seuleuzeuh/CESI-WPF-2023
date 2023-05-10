using PokeApiNet;
using CESI_WPF_2023.Models;
using CESI_WPF_2023.ViewModels;
using System.Windows;

namespace CESI_WPF_2023.Views
{
    /// <summary>
    /// Interaction logic for PokemonDetailWindow.xaml
    /// </summary>
    public partial class PokemonDetailWindow : Window
    {
        public PokemonDetailWindow(PokemonModel pokemon)
        {
            InitializeComponent();
            DataContext = new PokemonDetailViewModel(pokemon);
        }
    }
}
