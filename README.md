# ServiceRepoDemo

ASP.NET Core 8 MVC demo of the Service–Repository pattern with EF Core and SQL Server.

## Layers

    Controller  ->  IProductService  ->  IProductRepository  ->  AppDbContext  ->  SQL Server
    (HTTP/UI)       (business rules)     (data access only)      (EF Core)

- **Repositories/** – EF Core queries and persistence. No business rules.
- **Services/** – Business rules (unique names, can't delete in-stock items, sets CreatedAt).
- **Controllers/** – Depend only on `IProductService`; never touch `DbContext` or repositories.

## Run

1. Adjust `ConnectionStrings:DefaultConnection` in `appsettings.json`
   (e.g. `Server=(localdb)\\mssqllocaldb;...` or a SQL login for Docker/remote SQL Server).
2. Create the database:

       dotnet tool install --global dotnet-ef
       dotnet ef migrations add InitialCreate
       dotnet ef database update

3. `dotnet run` and open https://localhost:7080
