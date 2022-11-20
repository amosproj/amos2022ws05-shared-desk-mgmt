# .NET Backend Code for Deskstar Project

## Getting Started

1. Download .NET SDK over at https://dotnet.microsoft.com/en-us/download/dotnet/6.0

2. Compile and Run:

``` bash 
dotnet restore
```

``` bash 
dotnet publish -c Dev -o out
```

Choose one of the two environments (Development, Production). Also you need to configure
the database connection. Take a look at the run.sh file for information on that.

``` bash 
dotnet bin\Dev\net6.0\Deskstar.dll --environment [Development|Production] #cmd
dotnet bin/Dev/net6.0/Deskstar.dll --environment [Development|Production] #bash
```

Generate models and data access context from postgres database tables.

``` bash
dotnet tool install --global dotnet-ef
dotnet ef dbcontext scaffold "Host=localhost;Database=deskstar;Username=postgres;Password=root" Npgsql.EntityFrameworkCore.PostgreSQL -o Models
```

## Running with Docker

If you want to run the project with docker, you'll have to have docker installed and
run the following commands from inside the project folder.

``` bash 
docker build -t deskstar-image -f Dockerfile .
```

Use this command to start the dockerfile configured for the test environment. To target another DB, simply
alter the corresponding ENV variables. Running for Production can be done by altering the ASPNETCORE_ENVIRONMENT
ENV variable.
``` bash
docker run -it -e DB__HOST=localhost -e DB__DATABASE=deskstar -e DB__USERNAME=postgres -e DB__PASSWORD=root -e ASPNETCORE_ENVIRONMENT=Development --rm -p 5000:5000 --name deskstar-backend deskstar-image
```

## Overall Architecture

The backend will consist of 3 Layers:

1. Service
   will be represented by the Controllers, their job is to offer an endpoint and call some Logic if needed
2. Logic
   will be represented by the Usecases, all the domain logic usecases will be encapsulated here
3. Data Access
   will be represented by the DataAccess, and stores the DB related logic

### Entitites

Internal representation / interface of all the attributes for a specific feature e.g. User

### Models

External representation / interface of a specific business logic call e.g. CreateUserRequest