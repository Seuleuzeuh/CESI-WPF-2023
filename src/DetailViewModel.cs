using CESI_WPF_2023.Core;
using CESI_WPF_2023.DAL;
using CESI_WPF_2023.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace CESI_WPF_2023
{
    public class DetailViewModel : ViewModelBase
    {
        public DetailViewModel(PokemonModel pokemon)
        {
            Pokemon = pokemon;
            Series = new ISeries[]
            {
                new ColumnSeries<int>()
                {
                    Values = pokemon.Stats.Select(s => s.Value).ToList(),
                }
            };
            XAxes = new Axis[]{
                new Axis()
                {
                    LabelsRotation = 90,
                    Labels = pokemon.Stats.Select(s => s.Name).ToList()
                }
            };

            //_dbContext = new PokedexContext();

            //var dresseur = _dbContext.Dresseurs.FirstOrDefault();
            //var pokemonData = _dbContext.PokemonDatas.Find(Pokemon.Number);
            //if(pokemonData != null)
            //{
            //    PokemonData = _dbContext.PokemonDatas.Update(pokemonData);
            //}else
            //{
            //    PokemonData = _dbContext.PokemonDatas.Add(new PokemonData()
            //    {
            //        PokemonDataId = Pokemon.Number,
            //        State = PokemonDataState.Inconnu,
            //        Commentaire = string.Empty,
            //        DresseurId = dresseur.DresseurId
            //    });
            //    _dbContext.SaveChanges();
            //}
        }

        private PokemonModel _pokemon;
        public PokemonModel Pokemon
        {
            get { return _pokemon; }
            set { SetProperty(ref _pokemon, value); }
        }

        private ISeries[] _series;
        public ISeries[] Series
        {
            get { return _series; }
            set { SetProperty(ref _series, value); }
        }

        private Axis[] _xAxes;
        public Axis[] XAxes
        {
            get { return _xAxes; }
            set { SetProperty(ref _xAxes, value); }
        }

        private readonly PokedexContext _dbContext;
        private Axis[] _yAxes;
        public Axis[] YAxes
        {
            get { return _yAxes; }
            set { SetProperty(ref _yAxes, value); }
        }

        //public EntityEntry<PokemonData> PokemonData { get; }
    }
}
