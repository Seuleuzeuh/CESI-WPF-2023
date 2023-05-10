using CESI_WPF_2023.DAL;
using CESI_WPF_2023.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
    /// Interaction logic for PokemonDetailWindow.xaml
    /// </summary>
    public partial class PokemonDetailWindow : Window
    {
        private PokedexContext _context;

        public PokemonDetailWindow(PokemonModel pokemon)
        {
            InitializeComponent();
            Pokemon = pokemon;

            pokemonName.DataContext = pokemon;
            _context = new PokedexContext();
            LoadPokemonData();
        }

        private void LoadPokemonData()
        {
            var pokemonData = _context.PokemonDatas.Find(Pokemon.Number);
            if (pokemonData != null)
            {
                PokemonData = _context.PokemonDatas.Attach(pokemonData);
            }
            else
            {
                var dresseur = _context.Dresseurs.First();
                pokemonData = new PokemonData()
                {
                    Commentaire = string.Empty,
                    PokemonDataId = Pokemon.Number,
                    State = PokemonDataState.Inconnu,
                    DresseurId = dresseur.DresseurId
                };
                PokemonData = _context.PokemonDatas.Add(pokemonData);
                _context.SaveChanges();
            }

            pokemonStatus.DataContext = PokemonData;
            pokemonCommentaire.DataContext = PokemonData;
            statusComboBox.DataContext = PokemonData;
        }

        public PokemonModel Pokemon { get; private set; }
        public EntityEntry<PokemonData> PokemonData { get; private set; }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();
        }
    }
}
