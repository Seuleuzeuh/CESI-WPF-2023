# WPF CESI 2023

Projet support pour la pr�sentation de WPF CESI Rouen 2023.

### Le support de pr�sentation

Vous pouvez trouver le support � la racine de repo, sous forme d'un fichier pdf.

### Les branches 

La branche `master` contient ma version du Pokedex, avec l'impl�mentation du MVVM.
La branche `base` contient le projet de base, avec quelques utilitaires permettant de r�aliser un Pokedex.
La branche `resultat` contient le code r�alis� durant les 2 jours d'intervention.

### L'API
L'application utilise [Pok�API](https://pokeapi.co/docs/v2).
PokeAPIService est un wrapper permettant de g�n�rer des Model simplifi�s, utilisant en interne [PokeApiNet](https://github.com/mtrdp642/PokeApiNet).

### Pour g�n�rer la base de donn�es SQL Lite : 
```
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Des questions ?

Contactez moi par e-mail ou via les Issues du repo.