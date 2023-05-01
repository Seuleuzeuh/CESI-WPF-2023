using PokeApiNet;
using CESI_WPF_2023.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PokedexApp.Services
{
    internal class PokeAPIService
    {
        private static PokeAPIService? _instance;
        internal static PokeAPIService Instance
        { 
            get
            {
                if (_instance == null)
                    _instance = new PokeAPIService();

                return _instance;
            }
        }

        private PokeApiClient _pokeClient;

        private PokeAPIService()
        {
            _pokeClient = new PokeApiClient();
        }

        internal async Task<PokemonModel?> GetPokemonAsync(string pokemonName)
        {
            var pokemon = await SearchPokemonByNameAsync(pokemonName);
            if (pokemon == null)
                return null;

            return await PokemonModelAsyncFactory(pokemon);
        }

        internal async Task<PokemonModel?> GetPokemonAsync(int number)
        {
            var pokemon = await SearchPokemonByNumberAsync(number);
            if (pokemon == null)
                return null;

            return await PokemonModelAsyncFactory(pokemon);
        }

        internal async Task<List<SimplePokemonModel>> GetSimplePokemonsPageAsync(int itemsPerPage, int offset)
        {
            List<SimplePokemonModel> result = new List<SimplePokemonModel>();
            var species = await _pokeClient.GetNamedResourcePageAsync<PokemonSpecies>(itemsPerPage, offset);
            foreach(var s in species.Results)
            {
                result.Add(await SimplePokemonModelFactoryAsync(s));
            }

            return result;
        }

        private async Task<PokemonModel?> PokemonModelAsyncFactory(Pokemon pokemon)
        {
            var species = await LoadResourceAsync(pokemon.Species);
            string imageUrl = await GetPokemonImageUrlAsync(pokemon);
            IEnumerable<string?> typeNames = await GetPokemonTypeNamesAsync(pokemon);
            string? name = GetPokemonName(species);

            var evolutionChain = await LoadResourceAsync(species.EvolutionChain);
            var evolution = await ContructEvolution(evolutionChain.Chain);
            Dictionary<string, int> stats = new Dictionary<string, int>();
            foreach(var pokemonStat in pokemon.Stats)
            {
                var stat = await LoadResourceAsync(pokemonStat.Stat);
                stats.Add(stat.Names.Where(n => n.Language.Name == "fr").Select(n => n.Name).FirstOrDefault(), pokemonStat.BaseStat);
            }
            var description = species.FlavorTextEntries.Where(f => f.Language.Name == "fr").Select(f => f.FlavorText).FirstOrDefault();
            return new PokemonModel(pokemon.Id, name, description, imageUrl, typeNames.ToList(), evolution, stats);
        }

        private static string? GetPokemonName(PokemonSpecies species)
        {
            return species.Names.Where(n => n.Language.Name == "fr").Select(n => n.Name).FirstOrDefault();
        }

        private async Task<string> GetPokemonImageUrlAsync(Pokemon pokemon)
        {
            var form = await LoadResourceAsync(pokemon.Forms.First());
            var imageUrl = form.Sprites.FrontDefault;
            return imageUrl;
        }

        private async Task<IEnumerable<string?>> GetPokemonTypeNamesAsync(Pokemon pokemon)
        {
            var types = await LoadResourceAsync(pokemon.Types.Select(t => t.Type));
            var typeNames = types.Select(t => t.Names.Where(n => n.Language.Name == "fr").Select(n => n.Name).FirstOrDefault());
            return typeNames;
        }

        private async Task<Evolution> ContructEvolution(ChainLink baseChain)
        {
            SimplePokemonModel simplePokemon = await SimplePokemonModelFactoryAsync(baseChain.Species);
            return new Evolution(simplePokemon, await GetEvolutionAsync(baseChain));
        }

        private async Task<List<Evolution>> GetEvolutionAsync(ChainLink chainLink)
        {
            List<Evolution> evolutions = new List<Evolution>();
            if (chainLink != null)
            {
                foreach (var chain in chainLink.EvolvesTo)
                {
                    SimplePokemonModel simplePokemon = await SimplePokemonModelFactoryAsync(chain.Species);
                    evolutions.Add(new Evolution(simplePokemon, await GetEvolutionAsync(chain)));
                }
            }

            return evolutions;
        }

        private async Task<SimplePokemonModel> SimplePokemonModelFactoryAsync(NamedApiResource<PokemonSpecies> speciesRessource)
        {
            var species = await LoadResourceAsync(speciesRessource);
            var pokemon = await SearchPokemonByNumberAsync(species.Id);

            string imageUrl = await GetPokemonImageUrlAsync(pokemon);
            IEnumerable<string?> typeNames = await GetPokemonTypeNamesAsync(pokemon);
            string name = GetPokemonName(species);
            var simplePokemon = new SimplePokemonModel(pokemon.Id, name, imageUrl, typeNames.ToList());
            return simplePokemon;
        }

        private async Task<Pokemon?> SearchPokemonByNameAsync(string searchText)
        {
            try
            {
                return await _pokeClient.GetResourceAsync<Pokemon>(searchText);
            }
            catch(HttpRequestException)
            {
                return null;
            }
        }

        private async Task<T> LoadResourceAsync<T>(UrlNavigation<T> resource) where T : ResourceBase
        {
            return await _pokeClient.GetResourceAsync(resource);
        }

        private async Task<IEnumerable<T>> LoadResourceAsync<T>(IEnumerable<UrlNavigation<T>> resource) where T : ResourceBase
        {
            return await _pokeClient.GetResourceAsync(resource);
        }

        private async Task<Pokemon?> SearchPokemonByNumberAsync(int pokeNumber)
        {
            try
            {
                return await _pokeClient.GetResourceAsync<Pokemon>(pokeNumber);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}
