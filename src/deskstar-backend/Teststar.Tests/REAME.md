# run tests

```
dotnet test
```

This repo contains only unit tests for business logic of usecase classes

# test structure
All tests for a usecase and its dependant test data is saved in a file called 
```
<Usecase>Tests.cs

Each individual test is called and follows this structure:
public void <MethodNameYouWannaTest>_When<Scenario>_Should<ExpectedResult>(){
    //setup

    //arrange

    //act

    //assert

    //cleanup
}

```

changes of existing setup functions MUST only be commited if all existing tests still work.
# test coverage

show test coverage
```
rm -rf ../Deskstar/Migrations && rm -rf ../Deskstar/Controllers
dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage" --logger trx --results-directory coverage

cp coverage/*/coverage.cobertura.xml coverage/coverage.cobertura.xml

pycobertura show coverage/coverage.cobertura.xml
```