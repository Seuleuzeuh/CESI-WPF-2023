using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using CESI_WPF_2023.Core;
using CESI_WPF_2023.DAL;
using CESI_WPF_2023.Models;
using CESI_WPF_2023.Services;
using CESI_WPF_2023.Views;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace CESI_WPF_2023.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            _context = new PokedexContext();
            InitializeViewModelAsync();
            SearchCommand = new RelayCommandAsync<string>(ExecuteSearchAsync);
            SaveDresseurCommand = new RelayCommandAsync(ExecuteSaveAsync);
            LoadMoreCommand = new RelayCommandAsync(ExecuteLoadMoreAsync);
        }

        private async Task ExecuteSaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private async Task ExecuteSearchAsync(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return;
            }

            PokemonModel? result = null;
            if (int.TryParse(searchText, out int pokeNumber))
            {
                result = await PokeAPIService.Instance.GetPokemonAsync(pokeNumber);
            }
            else
            {
                result = await PokeAPIService.Instance.GetPokemonAsync(searchText);
            }

            if (result != null)
            {
                var detail = new PokemonDetailWindow(result);
                detail.Show();
            }
            else
            {
                MessageBox.Show($"Aucun résultat trouvé pour : {searchText}");
            }
        }

        private async Task InitializeViewModelAsync()
        {
            await LoadDresseurAsync();
            await LoadPokemonsAsync();
        }

        private async Task LoadDresseurAsync()
        {
            var dresseur = await _context.Dresseurs.FirstOrDefaultAsync();
            if(dresseur == null) {
                dresseur = new Dresseur()
                {
                    Nom = "Sacha"
                };

                Dresseur = await _context.Dresseurs.AddAsync(dresseur);
                await _context.SaveChangesAsync();
            }
            else
            {
                Dresseur = _context.Dresseurs.Attach(dresseur);
            }
        }

        private async Task LoadPokemonsAsync()
        {
            await ExecuteLoadMoreAsync();
        }

        private int _pageLoaded = 0;
        private const int ItemsPerPage = 20;
        private async Task ExecuteLoadMoreAsync()
        {
            var pokes = await PokeAPIService.Instance.GetSimplePokemonsPageAsync(ItemsPerPage, ItemsPerPage * _pageLoaded);
            foreach(var item in pokes)
            {
                Pokemons.Add(item);
            }
            _pageLoaded++;
        }

        private ObservableCollection<SimplePokemonModel> _pokemons = new ObservableCollection<SimplePokemonModel>();
        public ObservableCollection<SimplePokemonModel> Pokemons
        {
            get { return _pokemons; }
            set { SetProperty(ref _pokemons, value); }
        }

        private EntityEntry<Dresseur> _dresseur;
        public EntityEntry<Dresseur> Dresseur
        {
            get { return _dresseur; }
            set { SetProperty(ref _dresseur, value); }
        }

        private SimplePokemonModel _selectedPokemon;
        private PokedexContext _context;

        public SimplePokemonModel SelectedPokemon
        {
            get { return _selectedPokemon; }
            set 
            { 
                if (SetProperty(ref _selectedPokemon, value))
                {
                    OnPokemonSelectionChangedAsync();
                }
            }
        }

        private async Task OnPokemonSelectionChangedAsync()
        {
            if(SelectedPokemon != null)
            {
                var result = await PokeAPIService.Instance.GetPokemonAsync(SelectedPokemon.Number);
                var detail = new PokemonDetailWindow(result);
                detail.Show();
            }
        }

        public ICommand SearchCommand  { get; set; }
        public RelayCommandAsync SaveDresseurCommand { get; }
        public RelayCommandAsync LoadMoreCommand { get; }
    }
}
