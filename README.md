# WPF CESI 2023

Projet support pour la pr�sentation de WPF CESI Rouen 2023.

### Pour g�n�rer la base de donn�es SQL Lite : 
```
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```