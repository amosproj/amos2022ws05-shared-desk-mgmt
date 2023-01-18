#!/bin/bash
# export DB__HOST=localhost
# export DB__DATABASE=postgres
# export DB__USERNAME=postgres
# export DB__PASSWORD=postgres
# set DB__HOST=localhost
# set DB__DATABASE=postgres
# set DB__USERNAME=postgres
# set DB__PASSWORD=postgres

dotnet restore
dotnet publish -c Dev -o out
dotnet bin/Dev/net6.0/Deskstar.dll
