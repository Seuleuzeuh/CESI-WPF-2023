# WPF CESI 2023

Projet support pour la présentation de WPF CESI Rouen 2023.

### Le support de présentation

Vous pouvez trouver le support à la racine de repo, sous forme d'un fichier pdf.

### Les branches 

La branche `master` contient ma version du Pokedex, avec l'implémentation du MVVM.
La branche `base` contient le projet de base, avec quelques utilitaires permettant de réaliser un Pokedex.
La branche `resultat` contient le code réalisé durant les 2 jours d'intervention.

### L'API
L'application utilise [PokéAPI](https://pokeapi.co/docs/v2).
PokeAPIService est un wrapper permettant de générer des Model simplifiés, utilisant en interne [PokeApiNet](https://github.com/mtrdp642/PokeApiNet).

### Pour générer la base de données SQL Lite : 
```
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Des questions ?

Contactez moi par e-mail ou via les Issues du repo.