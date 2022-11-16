dotnet restore
dotnet publish -c Dev -o out
export DB__HOST=localhost
export DB__DATABASE=deskstar
export DB__USERNAME=postgres
export DB__PASSWORD=root
export ASPNETCORE_ENVIRONMENT=Development
dotnet bin/Dev/net6.0/Deskstar.dll