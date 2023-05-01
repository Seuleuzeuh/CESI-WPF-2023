using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CESI_WPF_2023.DAL
{
    public class PokedexContext : DbContext
    {
        public DbSet<Dresseur> Dresseurs { get; set; }
        public DbSet<PokemonData> PokemonDatas { get; set; }

        public string DbPath { get; }

        public PokedexContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "pokedex.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    public class Dresseur
    {
        public int DresseurId { get; set; }
        public string Nom { get; set; }

        public List<PokemonData> PokemonDatas { get; } = new();
    }

    public class PokemonData
    {
        public int PokemonDataId { get; set; }
        public string Commentaire { get; set; }
        public PokemonDataState State { get; set; }


        public int DresseurId { get; set; }
        public Dresseur Dresseur { get; set; }
    }

    public enum PokemonDataState : int
    {
        Inconnu,
        Vu,
        Capture
    }
}
