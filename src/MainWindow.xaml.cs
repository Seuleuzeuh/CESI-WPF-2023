using CESI_WPF_2023.DAL;
using CESI_WPF_2023.Models;
using PokedexApp.Services;
using SQLitePCL;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CESI_WPF_2023
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Random r = new Random(75);
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel = new MainViewModel();
            LoadPokemonList();
        }

        private async void LoadPokemonList()
        {
            
            r.Next(1000);
            var premiersPokemon = await PokeAPIService.Instance.GetSimplePokemonsPageAsync(20, 0);
            pokemonsList.ItemsSource = premiersPokemon;
            dataGrid.ItemsSource = premiersPokemon;
        }

        public MainViewModel ViewModel { get; }

        private void ButtonEnregistrerClick(object sender, RoutedEventArgs e)
        {
            //Si MainWindow
            //_context.SaveChanges();

            //Si ViewModel
            ViewModel.SaveDresseur();
        }

        private void ButtonAfficherDetail(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var pokemon = button.DataContext as PokemonModel;
            if(pokemon != null)
            {
                new PokemonDetailWindow(pokemon).Show();
            }
        }

        private async void pokemonsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            var pokemonModel = listView.SelectedItem as SimplePokemonModel;
            if(pokemonModel != null)
            {
                var pokemon = await PokeAPIService.Instance.GetPokemonAsync(pokemonModel.Number);
                new PokemonDetailWindow(pokemon).Show();
            }
        }

        private void ChangeDynamicResource(object sender, RoutedEventArgs e)
        {
        }

        /**
         * Ajouter à la fin de la méthode InitializeAsync
         * panelDresseur.DataContext = Dresseur;
         */
    }
}
