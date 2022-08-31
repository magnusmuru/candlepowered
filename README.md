## CandlePowered

Quick implementation for Candle ingesting with Websockets.

Swagger should open by default but it is located at https://localhost:7003.

All commands need to be run inside CandlePowered project folder where `Program.cs` sits

## Docker
Docker is supported by running `docker-compose -f docker-compose.yml up`. This can be flaky due to some kind of SQL Server bug in connecting to a host that is in the same computer.

## Alternative
Alternative is to install .NET 6 SDK and run just the SQL Server in Docker.

To do so, replace the ConnectionString in appsettings.json with `"ConnectionStrings": {
"CandlePowered": "Server=db; Database=candlepowered; User Id=sa; Password=Password!322; MultipleActiveResultSets=true"
}` and run `docker-compose -f docker-compose-database.yml up`. This defaults to a local running API and dockerized SQL Server.

Run in NET supported IDE or by using `dotnet restore` to restore packages and  `dotnet run` command from the root of the project.

## Running
By default application drops all database information and keeps no state. This can be configured inside `appsettings.json` by changing both DropDatabase and MigrateDatabase keys to `false`
