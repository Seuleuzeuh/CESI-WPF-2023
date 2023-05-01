# WPF CESI 2023

Projet support pour la présentation de WPF CESI Rouen 2023.

### Pour générer la base de données SQL Lite : 
```
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```