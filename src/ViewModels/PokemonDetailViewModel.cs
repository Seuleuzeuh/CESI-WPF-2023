using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using CESI_WPF_2023.Core;
using CESI_WPF_2023.Models;
using SkiaSharp;
using System.Linq;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using CESI_WPF_2023.DAL;
using System;
using System.Threading.Tasks;
using CESI_WPF_2023.Helpers;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CESI_WPF_2023.ViewModels
{
    public class PokemonDetailViewModel : ViewModelBase
    {
        public PokemonDetailViewModel(PokemonModel pokemon)
        {
            Pokemon = pokemon;
            Series = new ISeries[] {
                new ColumnSeries<int>
                {
                    Values = pokemon.Stats.Select(s => s.Value),
                    Stroke = null,
                    Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                    IgnoresBarPosition = true,
                    TooltipLabelFormatter = (point) => $"{point.PrimaryValue}"
                }
            };
            XAxes = new[]
            {
                new Axis()
                {
                    Labels = pokemon.Stats.Select(s => s.Name).ToList(),
                    LabelsRotation = 15
                }
            };
            YAxes = new[]
            {
                new Axis()
                {
                    MinLimit = 0,
                    MaxLimit = 500
                }
            };
            _context = new PokedexContext();
            LoadPokemonDataAsync().ExecuteWithoutAwait();

            SavePokemonCommand = new RelayCommandAsync(ExecuteSavePokemonAsync);
        }

        private async Task ExecuteSavePokemonAsync()
        {
            await _context.SaveChangesAsync();
        }

        private async Task LoadPokemonDataAsync()
        {
            var pokemonData = await _context.PokemonDatas.FindAsync(Pokemon.Number);
            if (pokemonData != null)
            {
                PokemonData = _context.PokemonDatas.Attach(pokemonData);
            }
            else
            {
                var dresseur = await _context.Dresseurs.FirstAsync();
                pokemonData = new PokemonData()
                {
                    Commentaire = string.Empty,
                    PokemonDataId = Pokemon.Number,
                    State = PokemonDataState.Inconnu,
                    DresseurId = dresseur.DresseurId
                };
                PokemonData = await _context.PokemonDatas.AddAsync(pokemonData);
                await _context.SaveChangesAsync();
            }
        }

        public PokemonModel Pokemon { get; }

        public ISeries[] Series { get; set; }

        public Axis[] YAxes { get; set; }

        private PokedexContext _context;

        public Axis[] XAxes { get; set; }

        private EntityEntry<PokemonData> _pokemonData;
        public EntityEntry<PokemonData> PokemonData
        {
            get { return _pokemonData; }
            set { SetProperty(ref _pokemonData, value); }
        }

        public RelayCommandAsync SavePokemonCommand { get; }
    }
}
