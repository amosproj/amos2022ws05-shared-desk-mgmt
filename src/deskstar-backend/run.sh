dotnet restore
dotnet publish -c Dev -o out
dotnet bin/Dev/net6.0/Deskstar.dll --environment Development