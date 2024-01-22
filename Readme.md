## Starting SQL Server
```terminal
<!-- docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=abc123" -e "MSSQL_PID=Evaluation" -p 1433:1433  -v sqlvolume:/var/opt/mssql --name sqlpreview --hostname sqlpreview -d mcr.microsoft.com/mssql/server:2022-preview-ubuntu-22.04 -->
```

## Docker Command to Start SQL
```
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=VillaApi@db" -p 1433:1433 --name sql --hostname sql --platform linux/amd64 -d mcr.microsoft.com/mssql/server:2022-latest
```
## Connection String Syntax
 "ConnectionStrings": {
    "DefaultSQLConnection": "Server=localhost; Database=Magic_VillaAPI;User Id=sa; Password=VillaApi@db;TrustServerCertificate=True"
  }

## EF-Migrations
```
dotnet tool install --global dotnet-ef

dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet ef migrations add InitialCreate --output-dir Data/Migrations

dotnet ef database update
```
