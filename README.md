# virtu-trade

Virtu-trade is a hobby project paper trading solution.

The solution is agnostic to what you're trading. What you'll need to get started is some timeseries price data (preferably from some public API) and this solution.

## Development

### Migrations

```shell
dotnet ef migrations add YourMigrationName --project src/Persistence --startup-project src/Api

dotnet ef database update --project src/Persistence --startup-project src/Api
```