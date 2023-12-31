# MatchOdds
Implementation of a web API for managing Matches and their Odds. 

Frameworks/Libraries: 
  - ASP.NET Core 7
  - EF Core
  - Microsoft SQL Server
  - Docker

The entry point is the API Controller "MatchOddsController" which contains the endpoints. Each endpoint uses the DBService in order to interact with the Database
and perform the CRUD operation. Details of each operation are in the swagger generated by the solution. Also, initial migration scripts are in the Migrations folder 
that create the tables in the database.

# Docker
The project can run independently or inside a docker container. In order to dockerize it, open a terminal inside the "MatchOdds" folder of the repo and run "docker compose up" Use the " --build" parameter the first time. This command initializes the docker container with our project and runs it together with an sqlserver image to connect a database. Initial migrations are auto-executed when the application starts.
Also the connection string must be switched in appsettings.json -> "MatchOddsContext" to use sqlserver docker image with server=sql_server2022.
