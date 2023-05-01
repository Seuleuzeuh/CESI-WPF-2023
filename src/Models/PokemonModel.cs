using CESI_WPF_2023.Core;
using System.Collections.Generic;
using System.Linq;

namespace CESI_WPF_2023.Models
{
    public class PokemonModel : SimplePokemonModel
    {
        public PokemonModel(int number, string? name, string description, string imageUrl, List<string?> types, Evolution evolution, Dictionary<string, int> stats) : base(number, name, imageUrl, types) 
        {
            Description = description;
            Evolution = evolution;
            Stats = stats?.Select(kvp => new Stat(kvp.Key, kvp.Value))?.ToList() ?? new List<Stat>();
        }

        public string Description { get; }

        public Evolution Evolution { get; }

        public List<Stat> Stats { get; }
    }

    public class SimplePokemonModel : BindableObject
    {
        public SimplePokemonModel(int number, string name, string imageUrl, List<string> types)
        {
            Number = number;
            Name = name;
            ImageUrl = imageUrl;
            Types = types;
        }

        public int Number { get; }
        public string Name { get; }
        public string ImageUrl { get; }
        public List<string> Types { get; set; }
    }

    public class Evolution
    {
        public Evolution(SimplePokemonModel pokemon, List<Evolution> evolutions)
        {
            Pokemon = pokemon;
            Evolutions = evolutions;
        }

        public SimplePokemonModel Pokemon { get; }
        public List<Evolution> Evolutions { get; }
    }

    public class Stat
    {
        public Stat(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public int Value { get; set; }
    }
}
