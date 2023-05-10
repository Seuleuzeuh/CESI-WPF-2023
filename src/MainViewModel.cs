using CESI_WPF_2023.Core;
using CESI_WPF_2023.Models;
using PokedexApp.Services;
using System.Windows;
using System.Linq;
using System;
using System.Threading.Tasks;
using CESI_WPF_2023.DAL;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace CESI_WPF_2023
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            RechercheCommand = new RelayCommand(ExecuteSearch);


            _context = new PokedexContext();
            InitializeAsync();
        }

        internal void SaveDresseur()
        {
            _context.SaveChanges();
        }

        private PokedexContext _context;

        private async Task InitializeAsync()
        {
            var dresseur = _context.Dresseurs.FirstOrDefault();
            if (dresseur == null)
            {
                dresseur = new Dresseur(){ Nom = "Sacha"};

                Dresseur = _context.Dresseurs.Add(dresseur);
                _context.SaveChanges();
            }
            else
            {
                Dresseur = _context.Dresseurs.Attach(dresseur);
            }
        }

        private async void ExecuteSearch()
        {
            if (!string.IsNullOrEmpty(SearchText))
            {
                Models.PokemonModel? pokemon = await PokeAPIService.Instance.GetPokemonAsync(int.Parse(SearchText));
                Resultat = pokemon;
                //if(pokemon != null)
                //    new MaPageDeDetail(pokemon).Show();
            }
        }

        private string? _searchText;
        public string? SearchText
        {
            get => _searchText;
            set => SetProperty(ref _searchText, value);
        }

        private PokemonModel? _resultat;
        public PokemonModel? Resultat
        {
            get => _resultat;
            set => SetProperty(ref _resultat, value);
        }

        private SimplePokemonModel? _selectedItem;
        public SimplePokemonModel? SelectedItem
        {
            get { return _selectedItem; }
            set 
            {
                if (SetProperty(ref _selectedItem, value))
                {
                    if(_selectedItem != null)
                    {
                        MessageBox.Show($"Pokemon sélectionné : {SelectedItem.Name}");
                    }
                }
            }
        }


        public RelayCommand RechercheCommand { get; }

        private EntityEntry<Dresseur> _dresseur;
        public EntityEntry<Dresseur> Dresseur
        {
            get { return _dresseur; }
            set { SetProperty(ref _dresseur, value); }
        }

    }
}
