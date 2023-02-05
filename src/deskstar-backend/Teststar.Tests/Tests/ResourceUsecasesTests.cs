using Deskstar.Core.Exceptions;
using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Teststar.Tests.Tests;

public class ResourceUsecasesTests
{
  [Test]
  public void UpdateBuilding_WhenBuildingIdDoesNotExist_ShouldThrowEntityNotFoundException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = Guid.NewGuid(), CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    context.Buildings.Add(building);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<EntityNotFoundException>(() =>
      resourceUsecases.UpdateBuilding(companyId, buildingId, "Building Name", "Location"));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"There is no building with id '{buildingId}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateBuilding_WhenBuildingNameDoesAlreadyExist_ShouldThrowArgumentInvalidException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();

    var buildingName = "testbuilding";
    var company = new Company { CompanyId = companyId, CompanyName = "testcompany" };
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = buildingName, Location = "testlocation" };

    context.Companies.Add(company);
    context.Buildings.Add(building);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      resourceUsecases.UpdateBuilding(companyId, buildingId, buildingName, null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"There is already a building named '{buildingName}' in your company"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateBuilding_WhenCompanyIdDoesNotMatchBuilding_ShouldThrowInsufficientPermissionException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var company2Id = Guid.NewGuid();
    var company2 = new Company { CompanyId = company2Id, CompanyName = "testcompany2" };
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var buildingName = "testname";
    var building = new Building
      { BuildingId = buildingId, CompanyId = company2Id, BuildingName = buildingName, Location = "testlocation" };
    context.Companies.Add(company2);
    context.Buildings.Add(building);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<InsufficientPermissionException>(() =>
      resourceUsecases.UpdateBuilding(companyId, buildingId, "Building Name", "Location"));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"Your company has no access to administrate building '{buildingName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateBuilding_WhenAllInputIsValid_ShouldUpdateBuilding()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();

    var buildingName = "testbuilding";
    var company = new Company { CompanyId = companyId, CompanyName = "testcompany" };
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = buildingName, Location = "testlocation" };

    context.Companies.Add(company);
    context.Buildings.Add(building);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var updatedBuildingName = "New Building Name";
    var updatedLocation = "New York";
    // act
    var updatedBuilding = resourceUsecases.UpdateBuilding(companyId, buildingId, updatedBuildingName, updatedLocation);

    // assert
    Assert.IsNotNull(updatedBuilding);
    Assert.That(updatedBuilding.BuildingName, Is.EqualTo(updatedBuildingName));
    Assert.That(updatedBuilding.Location, Is.EqualTo(updatedLocation));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateBuilding_WhenOnlyLocationIsProvided_ShouldUpdateBuildingLocation()
  {
    // setup
    var companyId = Guid.NewGuid();
    var company = new Company { CompanyId = companyId, CompanyName = "testcompany" };
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    context.Companies.Add(company);
    context.Buildings.Add(building);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var newLocation = "newlocation";

    // act
    var updatedBuilding = resourceUsecases.UpdateBuilding(companyId, buildingId, null, newLocation);

    // assert
    Assert.NotNull(updatedBuilding);
    Assert.That(updatedBuilding.Location, Is.EqualTo(newLocation));
    Assert.That(updatedBuilding.BuildingId, Is.EqualTo(buildingId));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateBuilding_WhenOnlyBuildingNameIsProvided_ShouldUpdateBuildingName()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var location = "testlocation";
    var buildingName = "testbuilding";
    var company = new Company { CompanyId = companyId, CompanyName = "testcompany" };
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = buildingName, Location = location };

    context.Companies.Add(company);
    context.Buildings.Add(building);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var updatedBuildingName = "updatedBuildingName";

    // act
    var updatedBuilding = resourceUsecases.UpdateBuilding(companyId, buildingId, updatedBuildingName, null);

    // assert
    Assert.That(updatedBuilding.BuildingId, Is.EqualTo(buildingId));
    Assert.NotNull(updatedBuilding);
    Assert.That(updatedBuilding.BuildingName, Is.EqualTo(updatedBuildingName));
    Assert.That(updatedBuilding.Location, Is.EqualTo(location));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateBuilding_WhenNeitherLocationNorBuildingNameIsProvided_ShouldReturnBuildingId()
  {
    // setup
    var companyId = Guid.NewGuid();
    var company = new Company { CompanyId = companyId, CompanyName = "testcompany" };
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    context.Companies.Add(company);
    context.Buildings.Add(building);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act
    var updatedBuilding = resourceUsecases.UpdateBuilding(companyId, buildingId, null, null);

    // assert
    Assert.That(updatedBuilding.BuildingId, Is.EqualTo(buildingId));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateBuilding_WhenLocationIsEmpty_ShouldThrowArgumentInvalidException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var company = new Company { CompanyId = companyId, CompanyName = "testcompany" };
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    context.Companies.Add(company);
    context.Buildings.Add(building);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      resourceUsecases.UpdateBuilding(companyId, buildingId, "Building Name", ""));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo("Location must not be empty"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateBuilding_WhenBuildingNameIsEmpty_ShouldThrowArgumentInvalidException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var company = new Company { CompanyId = companyId, CompanyName = "testcompany" };
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    context.Companies.Add(company);
    context.Buildings.Add(building);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      resourceUsecases.UpdateBuilding(companyId, buildingId, "", "New York"));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo("Building name must not be empty"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateFloor_WhenFloorDoesNotExist_ShouldThrowEntitiesNotFoundException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    context.Buildings.Add(building);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<EntityNotFoundException>(() =>
      resourceUsecases.UpdateFloor(companyId, floorId, "Floor Name", buildingId));
    Assert.NotNull(ex);
    Assert.That($"There is no floor with id '{floorId}'" == ex?.Message);

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateFloor_WhenCompanyIdDoesNotMatchFloor_ShouldThrowInsufficientPermissionException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = Guid.NewGuid(), BuildingName = "testname", Location = "testlocation" };
    context.Buildings.Add(building);
    var floorName = "testfloor";
    var floor = new Floor { FloorId = floorId, FloorName = floorName, BuildingId = buildingId, Building = building };
    context.Floors.Add(floor);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<InsufficientPermissionException>(() =>
      resourceUsecases.UpdateFloor(companyId, floorId, "Floor Name", null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"Your company has no access to administrate floor '{floorName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateFloor_WhenFloorNameIsEmpty_ShouldThrowArgumentInvalidException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    context.Buildings.Add(building);
    var floor = new Floor { FloorId = floorId, FloorName = "testfloor", BuildingId = buildingId, Building = building };
    context.Floors.Add(floor);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() => resourceUsecases.UpdateFloor(companyId, floorId, "", null));
    Assert.NotNull(ex);
    Assert.That("Floor name must not be empty" == ex?.Message);

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateFloor_WhenBuildingIdDoesNotExist_ShouldThrowEntityNotFoundException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    context.Buildings.Add(building);
    var floor = new Floor { FloorId = floorId, FloorName = "testfloor", BuildingId = buildingId, Building = building };
    context.Floors.Add(floor);
    context.SaveChanges();

    // arrange
    var buildingIdNotExisting = Guid.NewGuid();
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<EntityNotFoundException>(() =>
      resourceUsecases.UpdateFloor(companyId, floorId, "testname", buildingIdNotExisting));
    Assert.NotNull(ex);
    Assert.That($"Building does not exist with id '{buildingIdNotExisting}'" == ex?.Message);

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateFloor_WhenCompanyIdDoesNotMatchBuilding_ShouldThrowInsufficientPermissionException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = Guid.NewGuid(), BuildingName = "testname", Location = "testlocation" };
    var floorName = "testname";
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = floorName };
    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<InsufficientPermissionException>(() =>
      resourceUsecases.UpdateFloor(companyId, floorId, "new name", null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"Your company has no access to administrate floor '{floorName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateFloor_WhenUpdatedBuildingBelongsToDifferentCompany_ShouldThrowInsufficientPermissionException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, FloorName = "testfloor", BuildingId = buildingId, Building = building };
    context.Buildings.Add(building);
    context.Floors.Add(floor);

    var anotherCompanyId = Guid.NewGuid();
    var building2Id = Guid.NewGuid();
    var building2Name = "testbuilding2";
    var building2 = new Building
    {
      BuildingId = building2Id, CompanyId = anotherCompanyId, BuildingName = building2Name, Location = "testlocation"
    };
    context.Buildings.Add(building2);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<InsufficientPermissionException>(() =>
      resourceUsecases.UpdateFloor(companyId, floorId, null, building2Id));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"Your company has no access to move a floor to building '{building2Name}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateFloor_WhenUpdatedBuildingNameAlreadyExists_ShouldThrowArgumentInvalidException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorName = "testfloor";
    var buildingName = "testname";
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = buildingName, Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, FloorName = floorName, BuildingId = buildingId, Building = building };
    context.Buildings.Add(building);
    context.Floors.Add(floor);

    var anotherCompanyId = Guid.NewGuid();
    var building2Id = Guid.NewGuid();
    var building2 = new Building
    {
      BuildingId = building2Id, CompanyId = anotherCompanyId, BuildingName = "testbuilding2", Location = "testlocation"
    };
    context.Buildings.Add(building2);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      resourceUsecases.UpdateFloor(companyId, floorId, floorName, null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"There is already a floor named '{floorName}' in building '{buildingName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateFloor_WhenAllInputIsValid_ShouldUpdateFloorAndReturnFloorId()
  {
    // setup
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    context.Buildings.Add(building);
    var building2Id = Guid.NewGuid();
    var building2 = new Building
      { BuildingId = building2Id, CompanyId = companyId, BuildingName = "testbuilding2", Location = "testlocation" };
    context.Buildings.Add(building2);
    var floor = new Floor { FloorId = floorId, FloorName = "testfloor", BuildingId = buildingId, Building = building };
    context.Floors.Add(floor);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var newFloorName = "new name";

    // act
    var updatedFloor = resourceUsecases.UpdateFloor(companyId, floorId, newFloorName, building2Id);

    // assert
    Assert.That(updatedFloor.FloorId, Is.EqualTo(floorId));
    Assert.NotNull(updatedFloor);
    Assert.That(updatedFloor.FloorName, Is.EqualTo(newFloorName));
    Assert.That(updatedFloor.BuildingId, Is.EqualTo(building2Id));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateFloor_WhenFloorNameAlreadyExistsInUpdatedBuilding_ShouldThrowArgumentInvalidException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var floorName = "testfloor";

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, FloorName = floorName, BuildingId = buildingId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);

    var building2Id = Guid.NewGuid();
    var floor2Id = Guid.NewGuid();
    var building2Name = "testbuilding2";
    var building2 = new Building
      { BuildingId = building2Id, CompanyId = companyId, BuildingName = building2Name, Location = "testlocation" };
    var floor2 = new Floor { FloorId = floor2Id, FloorName = floorName, BuildingId = building2Id };

    context.Buildings.Add(building2);
    context.Floors.Add(floor2);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      resourceUsecases.UpdateFloor(companyId, floorId, floorName, building2Id));
    Assert.NotNull(ex);
    Assert.That(ex?.Message,
      Is.EqualTo(
        $"You cant move floor '{floorName}' to building '{building2Name}'. In building '{building2Name}' already exists a floor called '{floorName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateFloor_WhenOnlyFloorNameIsProvided_ShouldUpdateFloorNameAndReturnFloorId()
  {
    // setup
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    context.Buildings.Add(building);
    var floorName = "testfloor";
    var floor = new Floor { FloorId = floorId, FloorName = floorName, BuildingId = buildingId, Building = building };
    context.Floors.Add(floor);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var newFloorName = "newFloorName";

    // act
    var updatedFloor = resourceUsecases.UpdateFloor(companyId, floorId, newFloorName, null);

    // assert
    Assert.That(updatedFloor.FloorId, Is.EqualTo(floorId));
    Assert.NotNull(updatedFloor);
    Assert.That(updatedFloor.BuildingId, Is.EqualTo(buildingId));
    Assert.That(updatedFloor.FloorName, Is.EqualTo(newFloorName));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateFloor_WhenOnlyBuildingIdIsProvided_ShouldUpdateFloorBuildingAndReturnFloorId()
  {
    // setup
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    context.Buildings.Add(building);
    var building2Id = Guid.NewGuid();
    var building2 = new Building
      { BuildingId = building2Id, CompanyId = companyId, BuildingName = "testbuilding2", Location = "testlocation" };
    context.Buildings.Add(building2);
    var floorName = "testfloor";
    var floor = new Floor { FloorId = floorId, FloorName = floorName, BuildingId = buildingId, Building = building };
    context.Floors.Add(floor);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act
    var updatedFloor = resourceUsecases.UpdateFloor(companyId, floorId, null, building2Id);

    // assert
    Assert.That(updatedFloor.FloorId, Is.EqualTo(floorId));
    Assert.NotNull(updatedFloor);
    Assert.That(updatedFloor.BuildingId, Is.EqualTo(building2Id));
    Assert.That(updatedFloor.FloorName, Is.EqualTo(floorName));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateFloor_WhenNeitherFloorNameNorBuildingIdIsProvided_ShouldReturnFloorId()
  {
    // setup
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    context.Buildings.Add(building);
    var floorName = "testfloor";
    var floor = new Floor { FloorId = floorId, FloorName = floorName, BuildingId = buildingId, Building = building };
    context.Floors.Add(floor);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act
    var updatedFloor = resourceUsecases.UpdateFloor(companyId, floorId, null, null);

    // assert
    Assert.That(updatedFloor.FloorId, Is.EqualTo(floorId));
    Assert.NotNull(updatedFloor);
    Assert.That(updatedFloor.BuildingId, Is.EqualTo(buildingId));
    Assert.That(updatedFloor.FloorName, Is.EqualTo(floorName));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateRoom_WhenRoomDoesNotExist_ShouldThrowEntityNotFoundException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var roomName = "testroom";
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);

    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation"
    };
    var anotherFloor = new Floor { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "testfloor" };
    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<EntityNotFoundException>(() =>
      resourceUsecases.UpdateRoom(companyId, anotherRoomId, "Room Name", floorId));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"There is no room with id '{anotherRoomId}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateRoom_WhenCompanyHasInsufficientPermission_ShouldThrowInsufficientPermissionException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var anotherCompanyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var roomName = "testroom";
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<InsufficientPermissionException>(() =>
      resourceUsecases.UpdateRoom(anotherCompanyId, roomId, "Room Name", floorId));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"Your company has no access to administrate room '{roomName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateRoom_WhenRoomNameIsEmpty_ShouldThrowArgumentInvalidException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act and assert
    var ex = Assert.Throws<ArgumentInvalidException>(() => resourceUsecases.UpdateRoom(companyId, roomId, "", floorId));
    Assert.NotNull(ex);
    Assert.That("Room name must not be empty" == ex?.Message);

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateRoom_WhenUpdatedFloorDoesNotExist_ShouldThrowEntityNotFoundException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var roomName = "testroom";
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var userUsecases = new Mock<IUserUsecases>();
    var resourceUsecases = new ResourceUsecases(logger.Object, context);

    // act+assert
    var ex = Assert.Throws<EntityNotFoundException>(() =>
      resourceUsecases.UpdateRoom(companyId, roomId, roomName, anotherFloorId));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"Floor does not exist with id '{anotherFloorId}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateRoom_WhenCompanyHasInsufficientPermissionToChangeFloor_ShouldThrowInsufficientPermissionException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var anotherCompanyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var context = new DataContext();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = anotherCompanyId, BuildingName = "anothertestname",
      Location = "testlocation"
    };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var anotherFloorName = "anotherTestFloor";
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = anotherFloorName };
    var roomName = "testroom";
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
    context.Buildings.Add(building);
    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(floor);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(room);
    context.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var userUsecases = new Mock<IUserUsecases>();
    var usecases = new ResourceUsecases(logger.Object, context);

    // act+assert
    var ex = Assert.Throws<InsufficientPermissionException>(() =>
      usecases.UpdateRoom(companyId, roomId, "New Room", anotherFloorId));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"Your company has no access to move a room to floor '{anotherFloorName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateRoom_WhenProvidedRoomNameAndFloor_ShouldUpdateBothNameAndFloor()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var roomName = "testroom";
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);

    var anotherFloorId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation"
    };
    var anotherFloor = new Floor { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "testfloor" };
    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var userUsecases = new Mock<IUserUsecases>();
    var usecases = new ResourceUsecases(logger.Object, context);
    var updatedRoomName = "updatedRoomName";

    // act
    var updatedRoom = usecases.UpdateRoom(companyId, roomId, updatedRoomName, anotherFloorId);

    // assert
    Assert.NotNull(updatedRoom);
    Assert.That(updatedRoom.RoomName, Is.EqualTo(updatedRoomName));
    Assert.That(updatedRoom.FloorId, Is.EqualTo(anotherFloorId));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateRoom_WhenUpdatingOnlyRoomName_ShouldUpdateNameOnly()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var roomName = "testroom";
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var userUsecases = new Mock<IUserUsecases>();
    var usecases = new ResourceUsecases(logger.Object, context);
    var updatedRoomName = "updatedRoomName";

    // act
    var updatedRoom = usecases.UpdateRoom(companyId, roomId, updatedRoomName, null);

    // assert
    Assert.NotNull(updatedRoom);
    Assert.That(updatedRoom.RoomName, Is.EqualTo(updatedRoomName));
    Assert.That(updatedRoom.FloorId, Is.EqualTo(floorId));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateRoom_WhenUpdatingOnlyRoomNameAndRoomNameAlreadyExists_ShouldThrowArgumentInvalidException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floorName = "testfloor";
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = floorName };
    var roomName = "testroom";
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var userUsecases = new Mock<IUserUsecases>();
    var usecases = new ResourceUsecases(logger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() => usecases.UpdateRoom(companyId, roomId, roomName, null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"There is already a room named '{roomName}' in floor '{floorName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateRoom_WhenUpdatingRoomNameAndFloorAndRoomNameAlreadyExists_ShouldThrowArgumentInvalidException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var roomId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var roomName = "testroom";
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);

    var anotherFloorId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation"
    };
    var anotherFloorName = "testfloor";
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = anotherFloorName };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = roomName };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);

    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var userUsecases = new Mock<IUserUsecases>();
    var usecases = new ResourceUsecases(logger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      usecases.UpdateRoom(companyId, roomId, roomName, anotherFloorId));
    Assert.NotNull(ex);
    Assert.That(ex?.Message,
      Is.EqualTo(
        $"You cant move room '{roomName}' to floor '{anotherFloorName}'. In floor '{anotherFloorName}' already exists a room called '{roomName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateRoom_WhenUpdatingOnlyFloor_ShouldUpdateFloorOnly()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var roomName = "testroom";
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);

    var anotherFloorId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation"
    };
    var anotherFloor = new Floor { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "testfloor" };
    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var userUsecases = new Mock<IUserUsecases>();
    var usecases = new ResourceUsecases(logger.Object, context);

    // act
    var updatedRoom = usecases.UpdateRoom(companyId, roomId, null, anotherFloorId);

    // assert
    Assert.NotNull(updatedRoom);
    Assert.That(updatedRoom.RoomName, Is.EqualTo(roomName));
    Assert.That(updatedRoom.FloorId, Is.EqualTo(anotherFloorId));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateRoom_WhenUpdatingNothing_ShouldUpdateNothing()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var roomName = "testroom";
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);

    context.SaveChanges();

    // arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var userUsecases = new Mock<IUserUsecases>();
    var usecases = new ResourceUsecases(logger.Object, context);

    // act
    var updatedRoom = usecases.UpdateRoom(companyId, roomId, null, null);

    // assert
    Assert.NotNull(updatedRoom);
    Assert.That(updatedRoom.RoomName, Is.EqualTo(roomName));
    Assert.That(updatedRoom.FloorId, Is.EqualTo(floorId));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenDeskDoesNotExist_ShouldThrowEntityNotFoundException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var floor = new Floor { FloorId = floorId, FloorName = "testfloor", BuildingId = Guid.NewGuid() };
    var context = new DataContext();
    context.Floors.Add(floor);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var unknownDeskId = Guid.NewGuid();

    // act+assert
    var ex = Assert.Throws<EntityNotFoundException>(() =>
      resourceUsecases.UpdateDesk(companyId, unknownDeskId, null, null, null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"There is no desk with id '{unknownDeskId}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenCompanyHasInsufficientPermission_ShouldThrowInsufficientPermissionException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);


    var anotherCompanyId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = anotherCompanyId, BuildingName = "anothertestname",
      Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskType = new DeskType
      { CompanyId = anotherCompanyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<InsufficientPermissionException>(() =>
      resourceUsecases.UpdateDesk(companyId, anotherDeskId, null, null, null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"Your company has no access to administrate desk '{deskName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenDeskNameIsEmpty_ShouldThrowArgumentInvalidException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);


    var anotherCompanyId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = anotherCompanyId, BuildingName = "anothertestname",
      Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskType = new DeskType
      { CompanyId = anotherCompanyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      resourceUsecases.UpdateDesk(companyId, deskId, "", null, null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo("Desk name must not be empty"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenRoomDoesNotExist_ShouldThrowEntityNotFoundException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);


    var anotherCompanyId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = anotherCompanyId, BuildingName = "anothertestname",
      Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskType = new DeskType
      { CompanyId = anotherCompanyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var unknownRoomId = Guid.NewGuid();

    // act+assert
    var ex = Assert.Throws<EntityNotFoundException>(() =>
      resourceUsecases.UpdateDesk(companyId, deskId, null, unknownRoomId, null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"Room does not exist with id '{unknownRoomId}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenCompanyHasInsufficientPermissionToChangeRoom_ShouldThrowInsufficientPermissionException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);


    var anotherCompanyId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = anotherCompanyId, BuildingName = "anothertestname",
      Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoomName = "anothertestroom";
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = anotherRoomName };
    var anotherDeskType = new DeskType
      { CompanyId = anotherCompanyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<InsufficientPermissionException>(() =>
      resourceUsecases.UpdateDesk(companyId, deskId, null, anotherRoomId, null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"Your company has no access to add a desk to room '{anotherRoomName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenDeskTypeDoesNotExist_ShouldThrowEntityNotFoundException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);


    var anotherCompanyId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = anotherCompanyId, BuildingName = "anothertestname",
      Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskType = new DeskType
      { CompanyId = anotherCompanyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var unknownDeskTypeId = Guid.NewGuid();

    // act+assert
    var ex = Assert.Throws<EntityNotFoundException>(() =>
      resourceUsecases.UpdateDesk(companyId, deskId, null, null, unknownDeskTypeId));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"DeskType does not exist with id '{unknownDeskTypeId}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void
    UpdateDesk_WhenCompanyHasInsufficientPermissionToChangeDeskType_ShouldThrowInsufficientPermissionException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);


    var anotherCompanyId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = anotherCompanyId, BuildingName = "anothertestname",
      Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskTypeName = "anotherdesktype";
    var anotherDeskType = new DeskType
      { CompanyId = anotherCompanyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = anotherDeskTypeName };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<InsufficientPermissionException>(() =>
      resourceUsecases.UpdateDesk(companyId, deskId, null, null, anotherDeskTypeId));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"Your company has no access to desk type '{anotherDeskTypeName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenUpdatingDeskNameAndRoomAndDeskType_ShouldUpdateDeskNameAndRoomAndDeskType()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);

    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskType = new DeskType
      { CompanyId = companyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var updatedDeskName = "updatedDeskName";

    // act
    var updatedDesk = resourceUsecases.UpdateDesk(companyId, deskId, updatedDeskName, anotherRoomId, anotherDeskTypeId);

    // assert
    Assert.NotNull(updatedDesk);
    Assert.That(updatedDesk.DeskName, Is.EqualTo(updatedDeskName));
    Assert.That(updatedDesk.RoomId, Is.EqualTo(anotherRoomId));
    Assert.That(updatedDesk.DeskTypeId, Is.EqualTo(anotherDeskTypeId));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenUpdatingOnlyDeskNameAndNameDoesNotExists_ShouldOnlyUpdateName()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);

    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskType = new DeskType
      { CompanyId = companyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var updatedDeskName = "updatedDeskName";

    // act
    var updatedDesk = resourceUsecases.UpdateDesk(companyId, deskId, updatedDeskName, null, null);

    // assert
    Assert.NotNull(updatedDesk);
    Assert.That(updatedDesk.DeskName, Is.EqualTo(updatedDeskName));
    Assert.That(updatedDesk.RoomId, Is.EqualTo(roomId));
    Assert.That(updatedDesk.DeskTypeId, Is.EqualTo(deskTypeId));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenUpdatingOnlyDeskNameAndNameDoesAlreadyExists_ShouldThrowArgumentInvalidException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var roomName = "testroom";
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);


    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskType = new DeskType
      { CompanyId = companyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var updatedDeskName = "updatedDeskName";
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = updatedDeskName, DeskTypeId = anotherDeskTypeId, RoomId = roomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      resourceUsecases.UpdateDesk(companyId, deskId, updatedDeskName, null, null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"There is already a desk named '{updatedDeskName}' in room '{roomName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenUpdatingOnlyRoomAndDeskNameInOtherRoomAlreadyExists_ShouldThrowArgumentInvalidException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);

    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoomName = "anothertestroom";
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = anotherRoomName };
    var anotherDeskType = new DeskType
      { CompanyId = companyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      resourceUsecases.UpdateDesk(companyId, deskId, null, anotherRoomId, null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message,
      Is.EqualTo(
        $"You cant move desk '{deskName}' to room '{anotherRoomName}'. In room '{anotherRoomName}' already exists a desk called '{deskName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenUpdatingOnlyRoomAndDeskNameInOtherRoomDoesNotExists_ShouldUpdateRoomName()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);

    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskType = new DeskType
      { CompanyId = companyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = "anotherDeskName", DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act
    var updatedDesk = resourceUsecases.UpdateDesk(companyId, deskId, null, anotherRoomId, null);

    // assert
    Assert.NotNull(updatedDesk);
    Assert.That(updatedDesk.DeskName, Is.EqualTo(deskName));
    Assert.That(updatedDesk.RoomId, Is.EqualTo(anotherRoomId));
    Assert.That(updatedDesk.DeskTypeId, Is.EqualTo(deskTypeId));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenUpdatingOnlyDeskType_ShouldOnlyUpdateDeskType()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);

    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskType = new DeskType
      { CompanyId = companyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act
    var updatedDesk = resourceUsecases.UpdateDesk(companyId, deskId, null, null, anotherDeskTypeId);

    // assert
    Assert.NotNull(updatedDesk);
    Assert.That(updatedDesk.DeskName, Is.EqualTo(deskName));
    Assert.That(updatedDesk.RoomId, Is.EqualTo(roomId));
    Assert.That(updatedDesk.DeskTypeId, Is.EqualTo(anotherDeskTypeId));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDesk_WhenUpdatingNothing_ShouldThrowArgumentInvalidException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);

    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskType = new DeskType
      { CompanyId = companyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act
    var updatedDesk = resourceUsecases.UpdateDesk(companyId, deskId, null, null, null);

    //assert
    Assert.NotNull(updatedDesk);
    Assert.That(updatedDesk.DeskName, Is.EqualTo(deskName));
    Assert.That(updatedDesk.RoomId, Is.EqualTo(roomId));
    Assert.That(updatedDesk.DeskTypeId, Is.EqualTo(deskTypeId));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDeskType_WhenDeskTypeDoesNotExist_ShouldThrowEntityNotFoundException()
  {
    // setup
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var floor = new Floor { FloorId = floorId, FloorName = "testfloor", BuildingId = Guid.NewGuid() };
    var context = new DataContext();
    context.Floors.Add(floor);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var unknownDeskTypeId = Guid.NewGuid();

    // act+assert
    var ex = Assert.Throws<EntityNotFoundException>(() =>
      resourceUsecases.UpdateDeskType(companyId, unknownDeskTypeId, null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"There is no desk type with id '{unknownDeskTypeId}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDeskType_WhenDeskTypeDoesBelongToDifferentCompany_ShouldThrowInsufficientPermissionException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();
    var deskTypeName = "testdesktype";
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = deskTypeName };

    context.DeskTypes.Add(deskType);
    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var noPermissionCompanyId = Guid.NewGuid();

    // act+assert
    var ex = Assert.Throws<InsufficientPermissionException>(() =>
      resourceUsecases.UpdateDeskType(noPermissionCompanyId, deskTypeId, null));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"Your company has no access to administrate desk type '{deskTypeName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDeskType_WhenDeskTypeNameIsEmpty_ShouldThrowArgumentInvalidException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);


    var anotherCompanyId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = anotherCompanyId, BuildingName = "anothertestname",
      Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskType = new DeskType
      { CompanyId = anotherCompanyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = "anotherdesktype" };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() => resourceUsecases.UpdateDeskType(companyId, deskTypeId, ""));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo("Desk type name must not be empty"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDeskType_WhenDeskTypeNameIsTaken_ShouldThrowArgumentInvalidException()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);


    var anotherCompanyId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = anotherCompanyId, BuildingName = "anothertestname",
      Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskTypeName = "anotherdesktype";
    var anotherDeskType = new DeskType
      { CompanyId = companyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = anotherDeskTypeName };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act+assert
    var ex = Assert.Throws<ArgumentInvalidException>(() =>
      resourceUsecases.UpdateDeskType(companyId, deskTypeId, anotherDeskTypeName));
    Assert.NotNull(ex);
    Assert.That(ex?.Message, Is.EqualTo($"There is already a desktype named '{anotherDeskTypeName}'"));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDeskType_WhenUpdatingDeskTypeName_ShouldUpdateName()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);


    var anotherCompanyId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = anotherCompanyId, BuildingName = "anothertestname",
      Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskTypeName = "anotherdesktype";
    var anotherDeskType = new DeskType
      { CompanyId = companyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = anotherDeskTypeName };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);
    var deskTypeNameThatDoesNotExistYet = "deskNameThatDoesNotExistYet";

    // act
    var updatedDeskType = resourceUsecases.UpdateDeskType(companyId, deskTypeId, deskTypeNameThatDoesNotExistYet);

    // assert
    Assert.NotNull(updatedDeskType);
    Assert.That(updatedDeskType.DeskTypeName, Is.EqualTo(deskTypeNameThatDoesNotExistYet));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void UpdateDeskType_WhenUpdatingNothing_ShouldUpdateNothing()
  {
    // setup
    var context = new DataContext();

    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    var building = new Building
      { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
    var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
    var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = "testroom" };
    var deskType = new DeskType { CompanyId = companyId, DeskTypeId = deskTypeId, DeskTypeName = "testdesktype" };
    var deskName = "testdesk";
    var desk = new Desk { DeskId = deskId, DeskName = deskName, DeskTypeId = deskTypeId, RoomId = roomId };

    context.Buildings.Add(building);
    context.Floors.Add(floor);
    context.Rooms.Add(room);
    context.DeskTypes.Add(deskType);
    context.Desks.Add(desk);


    var anotherCompanyId = Guid.NewGuid();
    var anotherBuildingId = Guid.NewGuid();
    var anotherFloorId = Guid.NewGuid();
    var anotherRoomId = Guid.NewGuid();
    var anotherDeskId = Guid.NewGuid();
    var anotherDeskTypeId = Guid.NewGuid();

    var anotherBuilding = new Building
    {
      BuildingId = anotherBuildingId, CompanyId = anotherCompanyId, BuildingName = "anothertestname",
      Location = "testlocation"
    };
    var anotherFloor = new Floor
      { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "anothertestfloor" };
    var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = "anothertestroom" };
    var anotherDeskTypeName = "anotherdesktype";
    var anotherDeskType = new DeskType
      { CompanyId = companyId, DeskTypeId = anotherDeskTypeId, DeskTypeName = anotherDeskTypeName };
    var anotherDesk = new Desk
      { DeskId = anotherDeskId, DeskName = deskName, DeskTypeId = anotherDeskTypeId, RoomId = anotherRoomId };

    context.Buildings.Add(anotherBuilding);
    context.Floors.Add(anotherFloor);
    context.Rooms.Add(anotherRoom);
    context.DeskTypes.Add(anotherDeskType);
    context.Desks.Add(anotherDesk);

    context.SaveChanges();

    // arrange
    var userLogger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(userLogger.Object, context);
    var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context);

    // act
    var updatedDeskType = resourceUsecases.UpdateDeskType(companyId, anotherDeskTypeId, null);

    // assert
    Assert.NotNull(updatedDeskType);
    Assert.That(updatedDeskType.DeskTypeName, Is.EqualTo(anotherDeskTypeName));

    // cleanup
    context.Database.EnsureDeleted();
  }

  [Test]
  public void GetBuildings_WhenNoBuildingFound_ShouldReturnAEmptyList()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    var companyId = Guid.NewGuid();
    var hasher = new PasswordHasher<User>();
    var company = new Company
    {
      CompanyId = companyId,
      CompanyName = "gehmalbierholn"
    };
    var user = new User
    {
      UserId = userId,
      MailAddress = "test@example.de",
      FirstName = "testF",
      LastName = "testL",
      CompanyId = company.CompanyId,
      IsApproved = true
    };
    user.Password = hasher.HashPassword(user, "testpw");
    db.Add(company);
    db.Add(user);
    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act
    var result = usecases.GetBuildings(userId);

    //assert
    Assert.That(result, Is.Empty);

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetBuildings_WhenTwoBuildingsFound_ShouldReturnAList()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    var companyId = Guid.NewGuid();
    SetupMockData(db, userId: userId, companyId: companyId);
    var building2 = new Building
    {
      BuildingName = "testBuilding2",
      Location = "testLocation2",
      CompanyId = companyId
    };
    db.Buildings.Add(building2);
    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act
    var result = usecases.GetBuildings(userId);

    //assert
    Assert.That(result, Is.Not.Empty);
    Assert.That(result.Count, Is.EqualTo(2));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetBuildings_WhenOneBuildingFound_ShouldReturnAException()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    SetupMockData(db, userId: userId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var callId = Guid.NewGuid();

    //act
    try
    {
      usecases.GetBuildings(callId);

      //assert
      Assert.Fail("No exception thrown");
    }
    catch (Exception e)
    {
      Assert.That(e.Message, Is.EqualTo($"There is no User with id '{callId}'"));
    }

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetAllFloors_WhenNoFloorFound_ShouldReturnAEmptyList()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    var companyId = Guid.NewGuid();
    var hasher = new PasswordHasher<User>();
    var company = new Company
    {
      CompanyId = companyId,
      CompanyName = "gehmalbierholn"
    };
    var user = new User
    {
      UserId = userId,
      MailAddress = "test@example.de",
      FirstName = "testF",
      LastName = "testL",
      CompanyId = company.CompanyId,
      IsApproved = true
    };
    user.Password = hasher.HashPassword(user, "testpw");
    db.Add(company);
    db.Add(user);
    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);

    //act
    var result = usecases.GetAllFloors(userId);

    //assert
    Assert.That(result, Is.Empty);

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetFloors_WhenNoFloorFound_ShouldReturnAEmptyList()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    var companyId = Guid.NewGuid();
    var hasher = new PasswordHasher<User>();
    var company = new Company
    {
      CompanyId = companyId,
      CompanyName = "gehmalbierholn"
    };
    var user = new User
    {
      UserId = userId,
      MailAddress = "test@example.de",
      FirstName = "testF",
      LastName = "testL",
      CompanyId = company.CompanyId,
      IsApproved = true
    };
    user.Password = hasher.HashPassword(user, "testpw");
    db.Add(company);
    db.Add(user);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act
    var result = usecases.GetBuildings(userId);

    //assert
    Assert.That(result, Is.Empty);

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetAllFloors_WhenNoFloorIdProvided_ShouldReturnList()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    SetupMockData(db, userId: userId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act
    var result = usecases.GetAllFloors(userId);

    //assert
    Assert.That(result, Is.Not.Empty);
    Assert.That(result.Count, Is.EqualTo(1));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetFloors_WhenInvaildFloorIdProvided_ShouldThrowAnExecption()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    SetupMockData(db, userId: userId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act+ assert
    Assert.Throws<ArgumentException>(() => usecases.GetFloors(userId, Guid.NewGuid().ToString()));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetFloors_WhenBuildingNotExists_ShouldReturnAException()
  {
    //setup
    using var db = new DataContext();

    var buildingId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    SetupMockData(db, userId: userId, buildingId: buildingId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var callId = Guid.NewGuid();

    //act
    try
    {
      usecases.GetFloors(userId, callId.ToString());

      //assert
      Assert.Fail("No exception thrown");
    }
    catch (Exception e)
    {
      Assert.That(e.Message, Is.EqualTo($"There is no Floor with id '{callId}'"));
    }

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetRooms_WhenRoomFound_ShouldReturnAList()
  {
    //setup
    using var db = new DataContext();
    var userId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    SetupMockData(db, userId: userId, floorId: floorId, roomId: roomId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    usecases.DeleteRoom(userId, roomId.ToString());


    //act
    var result = usecases.GetRooms(userId, floorId.ToString());

    //assert
    Assert.That(result, Is.Not.Empty);
    Assert.That(result.Count, Is.EqualTo(1));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetRooms_WhenNoRoomFound_ShouldReturnAEmptyList()
  {
    //setup
    using var db = new DataContext();
    var userId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    SetupMockData(db, userId: userId, floorId: floorId, roomId: roomId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    usecases.DeleteRoom(userId, roomId.ToString());


    //act
    var result = usecases.GetRooms(userId, Guid.NewGuid().ToString());

    //assert
    Assert.That(result, Is.Empty);

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetRooms_WhenNoValidRoomId_ShouldReturnAnException()
  {
    //setup
    using var db = new DataContext();
    var userId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    SetupMockData(db, userId: userId, floorId: floorId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);

    //assert + act
    Assert.Throws<ArgumentInvalidException>(() => usecases.GetRooms(userId, "abs"));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetRooms_WhenInvalidRoomIdProvided_ShouldThrowAnException()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    SetupMockData(db, userId: userId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act+ assert
    Assert.Throws<ArgumentException>(() => usecases.GetFloors(userId, Guid.NewGuid().ToString()));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetAllRooms_WhenNoRoomIdProvided_ShouldReturnAnList()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    SetupMockData(db, userId: userId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);

    //act
    var result = usecases.GetAllRooms(userId);

    //assert
    Assert.That(result, Is.Not.Empty);
    Assert.That(result.Count, Is.EqualTo(1));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetAllRooms_WhenNoRoomIsFound_ShouldReturnEmptyList()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    var companyId = Guid.NewGuid();
    var hasher = new PasswordHasher<User>();
    var company = new Company
    {
      CompanyId = companyId,
      CompanyName = "gehmalbierholn"
    };
    var user = new User
    {
      UserId = userId,
      MailAddress = "test@example.de",
      FirstName = "testF",
      LastName = "testL",
      CompanyId = company.CompanyId,
      IsApproved = true
    };
    user.Password = hasher.HashPassword(user, "testpw");
    db.Add(company);
    db.Add(user);
    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);

    //act
    var result = usecases.GetAllRooms(userId);

    //assert
    Assert.IsEmpty(result);

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetAllDesks_WhenNoDeskIsFound_ShouldReturnEmptyList()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    var companyId = Guid.NewGuid();
    var hasher = new PasswordHasher<User>();
    var company = new Company
    {
      CompanyId = companyId,
      CompanyName = "gehmalbierholn"
    };
    var user = new User
    {
      UserId = userId,
      MailAddress = "test@example.de",
      FirstName = "testF",
      LastName = "testL",
      CompanyId = company.CompanyId,
      IsApproved = true
    };
    user.Password = hasher.HashPassword(user, "testpw");
    db.Add(company);
    db.Add(user);
    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);

    //act
    var result = usecases.GetAllDesks(userId);

    //assert
    Assert.That(result, Is.Empty);

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDesks_WhenNoDeskFound_ShouldReturnAEmptyList()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var hasher = new PasswordHasher<User>();
    var company = new Company
    {
      CompanyId = companyId,
      CompanyName = "gehmalbierholn"
    };
    var user = new User
    {
      UserId = userId,
      MailAddress = "test@example.de",
      FirstName = "testF",
      LastName = "testL",
      CompanyId = company.CompanyId,
      IsApproved = true
    };
    user.Password = hasher.HashPassword(user, "testpw");
    var building = new Building
    {
      BuildingId = buildingId,
      BuildingName = "Gebäude1",
      Location = "Location1",
      CompanyId = company.CompanyId
    };
    var floor = new Floor
    {
      FloorId = floorId,
      FloorName = "Stockwerk1",
      BuildingId = building.BuildingId
    };
    var room = new Room
    {
      RoomId = roomId,
      FloorId = floor.FloorId,
      RoomName = "Raum1"
    };
    db.Add(company);
    db.Add(user);
    db.Add(building);
    db.Add(floor);
    db.Add(room);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act
    var result = usecases.GetDesks(userId, roomId.ToString(), DateTime.Now, DateTime.Now);

    //assert
    Assert.That(result, Is.Empty);

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDesks_WhenOneDeskFound_ShouldReturnAException()
  {
    //setup
    using var db = new DataContext();

    var roomId = Guid.NewGuid();
    var start = DateTime.Now;
    var end = DateTime.Now;
    var userId = Guid.NewGuid();
    SetupMockData(db, roomId: roomId, userId: userId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var callId = Guid.NewGuid();

    //act
    try
    {
      usecases.GetDesks(userId, callId.ToString(), start, end);

      //assert
      Assert.Fail("No exception thrown");
    }
    catch (Exception e)
    {
      Assert.That(e.Message, Is.EqualTo($"There is no Room with id '{callId}'"));
    }

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDesks_WhenInvalidDeskIdProvided_ShouldThrowAnException()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    SetupMockData(db, userId: userId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act+ assert
    Assert.Throws<ArgumentException>(() => usecases.GetDesk(Guid.NewGuid(), DateTime.Now, DateTime.MaxValue));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDesks_WhenInvalidDeskIdProvided2_ShouldThrowAnException()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    SetupMockData(db, userId: userId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act+ assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.GetDesks(userId, "abc", DateTime.Now, DateTime.MaxValue));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDesks_WhenInvalidDeskIdProvided3_ShouldThrowAnException()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    SetupMockData(db, userId: userId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act+ assert
    Assert.Throws<ArgumentException>(() =>
      usecases.GetDesks(userId, Guid.NewGuid().ToString(), DateTime.Now, DateTime.MaxValue));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetRooms_WhenInvalidDeskIdProvided_ShouldThrowAnException()
  {
    //setup
    using var db = new DataContext();

    var userId = Guid.NewGuid();
    SetupMockData(db, userId: userId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act+ assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.GetRooms(userId, "abc"));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetAllDesks_WhenNoRoomIdProvided_ShouldListOfAllDesks()
  {
    //setup
    using var db = new DataContext();

    var roomId = Guid.NewGuid();
    var start = DateTime.Now;
    var end = DateTime.Now;
    var userId = Guid.NewGuid();
    SetupMockData(db, roomId: roomId, userId: userId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);

    //act

    var result = usecases.GetAllDesks(userId);

    //assert
    Assert.That(result, Is.Not.Empty);
    Assert.That(result.Count, Is.EqualTo(1));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDesks_WhenDeskIsFound_StartBeforeEndIn_ShouldReturnACurrentsDeskList()
  {
    //setup
    using var db = new DataContext();

    var roomId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var start = DateTime.Now.AddHours(-1);
    var end = DateTime.Now.AddHours(1);
    SetupMockData(db, roomId: roomId, userId: userId, deskId: deskId);
    var booking = new Booking
    {
      BookingId = Guid.NewGuid(),
      DeskId = deskId,
      UserId = userId,
      Timestamp = DateTime.Now,
      StartTime = DateTime.Now,
      EndTime = end
    };
    db.Add(booking);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act
    var result = usecases.GetDesks(userId, roomId.ToString(), start, end);

    //assert

    Assert.Multiple(() =>
    {
      Assert.That(result, Is.Not.Null);
      Assert.That(result, Has.Count.EqualTo(1));
      Assert.That(result[0], Is.Not.Null);
      Assert.That(result[0].DeskId, Has.Length.EqualTo(36));
      Assert.That(result[0].DeskName, Is.EqualTo("Desk1"));
      Assert.That(result[0].RoomId, Has.Length.EqualTo(36));
      Assert.That(result[0].RoomName, Is.EqualTo("Raum1"));
      Assert.That(result[0].FloorId, Has.Length.EqualTo(36));
      Assert.That(result[0].FloorName, Is.EqualTo("Stockwerk1"));
      Assert.That(result[0].BuildingId, Has.Length.EqualTo(36));
      Assert.That(result[0].BuildingName, Is.EqualTo("Gebäude1"));
      Assert.That(result[0].DeskTyp, Is.EqualTo("Typ1"));
      Assert.That(result[0].Location, Is.EqualTo("Location1"));
      Assert.That(result[0].Bookings, Is.Not.Null);
      Assert.That(result[0].Bookings, Has.Count.EqualTo(1));
      Assert.That(result[0].Bookings[0], Is.Not.Null);
    });


    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDesks_WhenDeskIsFound_StartInEndBevore_ShouldReturnACurrentsDeskList()
  {
    //setup
    using var db = new DataContext();

    var roomId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var start = DateTime.Now.AddHours(1);
    var end = DateTime.Now.AddHours(2);
    SetupMockData(db, roomId: roomId, userId: userId, deskId: deskId);
    var booking = new Booking
    {
      BookingId = Guid.NewGuid(),
      DeskId = deskId,
      UserId = userId,
      Timestamp = DateTime.Now,
      StartTime = DateTime.Now,
      EndTime = end.AddMinutes(-1)
    };
    db.Add(booking);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act
    var result = usecases.GetDesks(userId, roomId.ToString(), start, end);

    //assert

    Assert.Multiple(() =>
    {
      Assert.That(result, Is.Not.Null);
      Assert.That(result, Has.Count.EqualTo(1));
      Assert.That(result[0], Is.Not.Null);
      Assert.That(result[0].DeskId, Has.Length.EqualTo(36));
      Assert.That(result[0].DeskName, Is.EqualTo("Desk1"));
      Assert.That(result[0].RoomId, Has.Length.EqualTo(36));
      Assert.That(result[0].RoomName, Is.EqualTo("Raum1"));
      Assert.That(result[0].FloorId, Has.Length.EqualTo(36));
      Assert.That(result[0].FloorName, Is.EqualTo("Stockwerk1"));
      Assert.That(result[0].BuildingId, Has.Length.EqualTo(36));
      Assert.That(result[0].BuildingName, Is.EqualTo("Gebäude1"));
      Assert.That(result[0].DeskTyp, Is.EqualTo("Typ1"));
      Assert.That(result[0].Location, Is.EqualTo("Location1"));
      Assert.That(result[0].Bookings, Is.Not.Null);
      Assert.That(result[0].Bookings, Has.Count.EqualTo(1));
      Assert.That(result[0].Bookings[0], Is.Not.Null);
    });


    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDesks_WhenDeskIsFound_SameStartAndEndTime_ShouldReturnACurrentsDeskList()
  {
    //setup
    using var db = new DataContext();

    var roomId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var start = DateTime.Now;
    var end = DateTime.Now.AddHours(1);
    SetupMockData(db, roomId: roomId, userId: userId, deskId: deskId);
    var booking = new Booking
    {
      BookingId = Guid.NewGuid(),
      DeskId = deskId,
      UserId = userId,
      Timestamp = DateTime.Now,
      StartTime = start,
      EndTime = end
    };
    db.Add(booking);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act
    var result = usecases.GetDesks(userId, roomId.ToString(), start, end);

    //assert

    Assert.Multiple(() =>
    {
      Assert.That(result, Is.Not.Null);
      Assert.That(result, Has.Count.EqualTo(1));
      Assert.That(result[0], Is.Not.Null);
      Assert.That(result[0].DeskId, Has.Length.EqualTo(36));
      Assert.That(result[0].DeskName, Is.EqualTo("Desk1"));
      Assert.That(result[0].RoomId, Has.Length.EqualTo(36));
      Assert.That(result[0].RoomName, Is.EqualTo("Raum1"));
      Assert.That(result[0].FloorId, Has.Length.EqualTo(36));
      Assert.That(result[0].FloorName, Is.EqualTo("Stockwerk1"));
      Assert.That(result[0].BuildingId, Has.Length.EqualTo(36));
      Assert.That(result[0].BuildingName, Is.EqualTo("Gebäude1"));
      Assert.That(result[0].DeskTyp, Is.EqualTo("Typ1"));
      Assert.That(result[0].Location, Is.EqualTo("Location1"));
      Assert.That(result[0].Bookings, Is.Not.Null);
      Assert.That(result[0].Bookings, Has.Count.EqualTo(1));
      Assert.That(result[0].Bookings[0], Is.Not.Null);
    });


    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDesk_WhenDeskNotFound_ShouldThrowAnException()
  {
    //setup
    using var db = new DataContext();

    var deskId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var start = DateTime.Now;
    var end = DateTime.Now;
    SetupMockData(db, deskId: deskId, userId: userId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var callId = Guid.NewGuid();

    //act
    try
    {
      usecases.GetDesk(callId, start, end);

      //assert
      Assert.Fail("No exception thrown");
    }
    catch (Exception e)
    {
      Assert.That(e.Message, Is.EqualTo($"There is no Desk with id '{callId}'"));
    }

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDesk_WhenDeskIsFound_ShouldReturnCurrentDeskObject()
  {
    //setup
    using var db = new DataContext();

    var deskId = Guid.NewGuid();
    var userId = Guid.NewGuid();
    var start = DateTime.Now;
    var end = new DateTime().AddHours(1);
    SetupMockData(db, deskId: deskId, userId: userId);
    var booking = new Booking
    {
      BookingId = Guid.NewGuid(),
      DeskId = deskId,
      UserId = userId,
      Timestamp = DateTime.Now,
      StartTime = start,
      EndTime = end
    };
    db.Add(booking);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act
    var result = usecases.GetDesk(deskId, start, end);

    //assert
    Assert.Multiple(() =>
    {
      Assert.That(result, Is.Not.Null);
      Assert.That(result.DeskId, Is.EqualTo(deskId.ToString()));
      Assert.That(result.DeskName, Is.EqualTo("Desk1"));
      Assert.That(result.RoomId, Has.Length.EqualTo(36));
      Assert.That(result.RoomName, Is.EqualTo("Raum1"));
      Assert.That(result.FloorId, Has.Length.EqualTo(36));
      Assert.That(result.FloorName, Is.EqualTo("Stockwerk1"));
      Assert.That(result.BuildingId, Has.Length.EqualTo(36));
      Assert.That(result.BuildingName, Is.EqualTo("Gebäude1"));
      Assert.That(result.DeskTyp, Is.EqualTo("Typ1"));
      Assert.That(result.Location, Is.EqualTo("Location1"));
      Assert.That(result.Bookings, Is.Not.Null);
      Assert.That(result.Bookings, Has.Count.EqualTo(1));
      Assert.That(result.Bookings[0], Is.Not.Null);
    });
    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDesks_WhenNonVaildID_ShouldThrowAnError()
  {
    //setup
    using var db = new DataContext();
    SetupMockData(db);
    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act + assert
    Assert.Throws<ArgumentException>(() => usecases.GetDesk(new Guid(), DateTime.Now, DateTime.Now));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDeskTypes_WhenDeskTypeIsFound_ShouldReturnDeskTypes()
  {
    //setup
    using var db = new DataContext();

    var companyId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();
    SetupMockData(db, deskTypeId: deskTypeId, companyId: companyId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act
    var result = usecases.GetDeskTypes(companyId);

    //assert
    Assert.That(result, Is.Not.Empty);
    Assert.That(result[0].DeskTypeId == deskTypeId);
    Assert.That(result[0].CompanyId == companyId);

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void GetDeskTypes_WhenCompanyHasNoDeskTypes_ShouldReturnEmptyList()
  {
    //setup
    using var db = new DataContext();

    var companyId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();
    SetupMockData(db, deskTypeId: deskTypeId, companyId: companyId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act
    var result = usecases.GetDeskTypes(new Guid());

    //assert
    Assert.That(result, Is.Empty);

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateDesk_WhenValidDeskTypeAndRoomId_ShouldAddDeskAndReturnItsGuid()
  {
    //setup
    using var db = new DataContext();

    var companyId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    SetupMockData(db, deskTypeId: deskTypeId, companyId: companyId, roomId: roomId);

    db.SaveChanges();

    //arrange
    var deskName = "validDeskName";
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);


    //act
    var result = usecases.CreateDesk(deskName, deskTypeId, roomId);

    //assert
    Assert.That(result != null);

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateDesk_WhenDuplicatedNameIsProvided_ShouldThrowArgumentInvalidException()
  {
    //setup
    using var db = new DataContext();

    var companyId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    SetupMockData(db, deskTypeId: deskTypeId, companyId: companyId, roomId: roomId);

    db.SaveChanges();

    //arrange
    var duplicated = "Desk1";
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);

    //act+assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.CreateDesk(duplicated, deskTypeId, roomId));


    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateDesk_WhenNoNameIsProvided_ShouldThrowArgumentInvalidException()
  {
    //setup
    using var db = new DataContext();

    var companyId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    SetupMockData(db, deskTypeId: deskTypeId, companyId: companyId, roomId: roomId);

    db.SaveChanges();

    //arrange
    var noDeskName = "";
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);

    //act+assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.CreateDesk(noDeskName, deskTypeId, roomId));


    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateDesk_WhenInvalidDeskType_ShouldThrowEntityNotFoundException()
  {
    //setup
    using var db = new DataContext();

    var companyId = Guid.NewGuid();
    var invalidDeskTypeId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    SetupMockData(db, companyId, roomId: roomId);

    db.SaveChanges();

    //arrange
    var deskName = "validDeskName";
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);

    //act+assert
    Assert.Throws<EntityNotFoundException>(() => usecases.CreateDesk(deskName, invalidDeskTypeId, roomId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateDesk_WhenInvalidRoomId_ShouldThrowEntityNotFoundException()
  {
    //setup
    using var db = new DataContext();

    var companyId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();
    var invalidRoomId = Guid.NewGuid();
    SetupMockData(db, companyId, deskTypeId: deskTypeId);

    db.SaveChanges();

    //arrange
    var deskName = "validDeskName";
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);

    //act+assert
    Assert.Throws<EntityNotFoundException>(() => usecases.CreateDesk(deskName, deskTypeId, invalidRoomId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateDeskType_WhenInvalidCompanyIdIsProvided_ShouldThrowEntitiyNotFoundException()
  {
    //setup
    using var db = new DataContext();
    SetupMockData(db);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var deskTypeName = "validDeskName";
    var invalidCompanyId = Guid.NewGuid();

    //act+assert
    Assert.Throws<EntityNotFoundException>(() => usecases.CreateDeskType(deskTypeName, invalidCompanyId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateDeskType_WhenNoDeskTypeNameIsProvided_ShouldThrowArgumentInvalidException()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    SetupMockData(db, companyId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var noDeskTypeName = "";

    //act+assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.CreateDeskType(noDeskTypeName, companyId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateDeskType_WhenDeskTypeAlreadyExists_ShouldThrowArgumentInvalidException()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    SetupMockData(db, companyId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var duplicateName = "Typ1";

    //act+assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.CreateDeskType(duplicateName, companyId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateDeskType_WhenValidArgumentsProvided_ShouldCreateDeskType()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    SetupMockData(db, companyId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var deskTypeName = "ValidName";

    //act+assert
    Assert.DoesNotThrow(() => usecases.CreateDeskType(deskTypeName, companyId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateRoom_WhenInvalidFloorIdIsProvided_ShouldThrowEntitiyNotFoundException()
  {
    //setup
    using var db = new DataContext();
    SetupMockData(db);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var roomName = "validRoomName";
    var invalidFloorId = Guid.NewGuid();

    //act+assert
    Assert.Throws<EntityNotFoundException>(() => usecases.CreateRoom(roomName, invalidFloorId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateRoom_WhenNoRoomNameIsProvided_ShouldThrowArgumentInvalidException()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    SetupMockData(db, companyId, floorId: floorId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var noRoomName = "";

    //act+assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.CreateRoom(noRoomName, floorId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateRoom_WhenRoomNameAlreadyExists_ShouldThrowArgumentInvalidException()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    SetupMockData(db, companyId, floorId: floorId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var duplicateName = "Raum1";

    //act+assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.CreateRoom(duplicateName, floorId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateRoom_WhenValidArgumentsProvided_ShouldCreateRoom()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    SetupMockData(db, companyId, floorId: floorId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var roomName = "ValidName";

    //act+assert
    Assert.DoesNotThrow(() => usecases.CreateRoom(roomName, floorId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateFloor_WhenInvalidBuildingIdIsProvided_ShouldThrowEntitiyNotFoundException()
  {
    //setup
    using var db = new DataContext();
    SetupMockData(db);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var floorName = "validName";
    var invalidBuildingId = Guid.NewGuid();

    //act+assert
    Assert.Throws<EntityNotFoundException>(() => usecases.CreateFloor(floorName, invalidBuildingId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateFloor_WhenNoFloorNameIsProvided_ShouldThrowArgumentInvalidException()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    SetupMockData(db, companyId, buildingId: buildingId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var noFloorName = "";

    //act+assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.CreateFloor(noFloorName, buildingId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateFloor_WhenFloorNameAlreadyExists_ShouldThrowArgumentInvalidException()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    SetupMockData(db, companyId, buildingId: buildingId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var duplicateName = "Stockwerk1";

    //act+assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.CreateFloor(duplicateName, buildingId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateFloor_WhenValidArgumentsProvided_ShouldCreateFloor()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    var buildingId = Guid.NewGuid();
    SetupMockData(db, companyId, buildingId: buildingId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var floorName = "ValidName";

    //act+assert
    Assert.DoesNotThrow(() => usecases.CreateFloor(floorName, buildingId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateBuilding_WhenInvalidCompanyIdIsProvided_ShouldThrowEntitiyNotFoundException()
  {
    //setup
    using var db = new DataContext();
    SetupMockData(db);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var buildingName = "validName";
    var location = "validLocation";
    var invalidCompanyId = Guid.NewGuid();

    //act+assert
    Assert.Throws<EntityNotFoundException>(() => usecases.CreateBuilding(buildingName, location, invalidCompanyId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateBuilding_WhenNoLocationIsProvided_ShouldThrowArgumentInvalidException()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    SetupMockData(db, companyId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var buildingName = "validName";
    var noLocation = "";

    //act+assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.CreateBuilding(buildingName, noLocation, companyId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateBuilding_WhenNoBuildingNameIsProvided_ShouldThrowArgumentInvalidException()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    SetupMockData(db, companyId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var noBuildingName = "";
    var location = "validLocation";

    //act+assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.CreateBuilding(noBuildingName, location, companyId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateBuilding_WhenBuildingNameAlreadyExists_ShouldThrowArgumentInvalidException()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    SetupMockData(db, companyId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var duplicateName = "Gebäude1";
    var location = "validLocation";

    //act+assert
    Assert.Throws<ArgumentInvalidException>(() => usecases.CreateBuilding(duplicateName, location, companyId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void CreateBuilding_WhenValidArgumentsProvided_ShouldCreateBuilding()
  {
    //setup
    using var db = new DataContext();
    var companyId = Guid.NewGuid();
    SetupMockData(db, companyId);

    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var usecases = new ResourceUsecases(logger.Object, db);
    var buildingName = "validName";
    var location = "validLocation";

    //act+assert
    Assert.DoesNotThrow(() => usecases.CreateBuilding(buildingName, location, companyId));

    //cleanup
    db.Database.EnsureDeleted();
  }

  [Test]
  public void DeleteBuilding_WhenValidBuildingIsProvided_ShouldUpdateBuilding()
  {
    //setup
    using var db = new DataContext();
    var buildingId = Guid.NewGuid();
    var adminId = Guid.NewGuid();

    SetupMockData(db, buildingId: buildingId, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);

    //act
    resourceUsecases.DeleteBuilding(adminId, buildingId.ToString());

    //assert
    Assert.That(db.Buildings.First(b => b.BuildingId == buildingId).IsMarkedForDeletion);
    db.Floors.Where(f => f.BuildingId == buildingId).ToList().ForEach(f => Assert.That(f.IsMarkedForDeletion));
  }

  [Test]
  public void DeleteBuilding_WhenNonValidAdminIsProvided_ShouldThrowAnException()
  {
    //setup
    using var db = new DataContext();
    var buildingId = Guid.NewGuid();
    var adminId = Guid.NewGuid();

    SetupMockData(db, buildingId: buildingId, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);

    //act
    Assert.Throws<ArgumentInvalidException>(
      () => resourceUsecases.DeleteBuilding(Guid.NewGuid(), buildingId.ToString()));
  }

  [Test]
  public void GetFloors_WhenNonValidFloorIdIsProvided_ShouldThrowAnException()
  {
    //setup
    using var db = new DataContext();
    var buildingId = Guid.NewGuid();
    var adminId = Guid.NewGuid();

    SetupMockData(db, buildingId: buildingId, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);

    //act
    Assert.Throws<ArgumentException>(() => resourceUsecases.GetFloors(adminId, Guid.NewGuid().ToString()));
  }

  [Test]
  public void GetFloors_WhenNonValidFloorIdIsProvided2_ShouldThrowAnException()
  {
    //setup
    using var db = new DataContext();
    var buildingId = Guid.NewGuid();
    var adminId = Guid.NewGuid();

    SetupMockData(db, buildingId: buildingId, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);

    //act
    Assert.Throws<ArgumentInvalidException>(() => resourceUsecases.GetFloors(adminId, "abc"));
  }

  [Test]
  public void GetFloors_WhenNoFloorFound_ShouldReturnAnEmptyList()
  {
    //setup
    using var db = new DataContext();
    var buildingId = Guid.NewGuid();
    var adminId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    SetupMockData(db, buildingId: buildingId, userId: adminId, floorId: floorId, roomId: roomId, deskId: deskId,
      deskTypeId: deskTypeId);
    var desk = db.Desks.First(d => d.DeskId == deskId);
    db.Desks.Remove(desk);
    var deskType = db.DeskTypes.First(d => d.DeskTypeId == deskTypeId);
    db.DeskTypes.Remove(deskType);
    var room = db.Rooms.First(r => r.RoomId == roomId);
    db.Rooms.Remove(room);
    var floor = db.Floors.First(f => f.FloorId == floorId);
    db.Floors.Remove(floor);
    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);
    //act
    var result = resourceUsecases.GetFloors(adminId, buildingId.ToString());
    //Assert
    Assert.That(result.Count == 0);
  }

  [Test]
  public void GetRoom_WhenNoRoomFound_ShouldReturnAnEmptyList()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();
    var floorId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    SetupMockData(db, userId: adminId, floorId: floorId, roomId: roomId, deskId: deskId,
      deskTypeId: deskTypeId);
    var desk = db.Desks.First(d => d.DeskId == deskId);
    db.Desks.Remove(desk);
    var deskType = db.DeskTypes.First(d => d.DeskTypeId == deskTypeId);
    db.DeskTypes.Remove(deskType);
    var room = db.Rooms.First(r => r.RoomId == roomId);
    db.Rooms.Remove(room);
    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);
    //act
    var result = resourceUsecases.GetRooms(adminId, floorId.ToString());
    //Assert
    Assert.That(result.Count == 0);
  }

  [Test]
  public void GetDesk_WhenNoDeskFound_ShouldReturnAnEmptyList()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();
    var roomId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    SetupMockData(db, userId: adminId, roomId: roomId, deskId: deskId,
      deskTypeId: deskTypeId);
    var desk = db.Desks.First(d => d.DeskId == deskId);
    db.Desks.Remove(desk);
    var deskType = db.DeskTypes.First(d => d.DeskTypeId == deskTypeId);
    db.DeskTypes.Remove(deskType);
    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);
    //act
    var result = resourceUsecases.GetDesks(adminId, roomId.ToString(), DateTime.MinValue, DateTime.MaxValue);
    //Assert
    Assert.That(result.Count == 0);
  }

  [Test]
  public void GetDeskType_WhenNoDeskTypeFound_ShouldReturnAnEmptyList()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var deskTypeId = Guid.NewGuid();

    SetupMockData(db, userId: adminId, deskTypeId: deskTypeId, deskId: deskId);
    var desk = db.Desks.First(d => d.DeskId == deskId);
    db.Desks.Remove(desk);
    var deskType = db.DeskTypes.First(d => d.DeskTypeId == deskTypeId);
    db.DeskTypes.Remove(deskType);
    db.SaveChanges();

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);
    //act
    var result = resourceUsecases.GetDeskTypes(adminId);
    //Assert
    Assert.That(result.Count == 0);
  }

  [Test]
  public void DeleteBuilding_WhenNonValidBuildingIsProvided_ShouldThrowEntityNotFoundException()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();

    SetupMockData(db, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);

    //act & assert
    Assert.Throws<EntityNotFoundException>(() => resourceUsecases.DeleteBuilding(adminId, new Guid().ToString()));
  }

  [Test]
  public void DeleteFloor_WhenValidFloorIsProvided_ShouldUpdateFloor()
  {
    //setup
    using var db = new DataContext();
    var floorId = Guid.NewGuid();
    var adminId = Guid.NewGuid();

    SetupMockData(db, floorId: floorId, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);

    //act
    resourceUsecases.DeleteFloor(adminId, floorId.ToString());

    //assert
    Assert.That(db.Floors.First(f => f.FloorId == floorId).IsMarkedForDeletion);
    db.Rooms.Where(r => r.FloorId == floorId).ToList().ForEach(r => Assert.That(r.IsMarkedForDeletion));
  }

  [Test]
  public void DeleteFloor_WhenNonValidFloorIsProvided_ShouldThrowEntityNotFoundException()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();

    SetupMockData(db, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);


    //act & assert
    Assert.Throws<EntityNotFoundException>(() => resourceUsecases.DeleteFloor(adminId, new Guid().ToString()));
  }

  [Test]
  public void DeleteRoom_WhenValidRoomIsProvided_ShouldUpdateRoom()
  {
    //setup
    using var db = new DataContext();
    var roomId = Guid.NewGuid();
    var adminId = Guid.NewGuid();

    SetupMockData(db, roomId: roomId, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);

    //act
    resourceUsecases.DeleteRoom(adminId, roomId.ToString());

    //assert
    Assert.That(db.Rooms.First(r => r.RoomId == roomId).IsMarkedForDeletion);
    db.Desks.Where(d => d.RoomId == roomId).ToList().ForEach(d => Assert.That(d.IsMarkedForDeletion));
  }

  [Test]
  public void DeleteRoom_WhenNonValidRoomIsProvided_ShouldThrowEntityNotFoundException()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();

    SetupMockData(db, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);


    //act & assert
    Assert.Throws<EntityNotFoundException>(() => resourceUsecases.DeleteRoom(adminId, new Guid().ToString()));
  }

  [Test]
  public void DeleteDesk_WhenValidDeskIsProvided_ShouldUpdateDesk()
  {
    //setup
    using var db = new DataContext();
    var deskId = Guid.NewGuid();
    var adminId = Guid.NewGuid();

    SetupMockData(db, deskId: deskId, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);

    //act
    resourceUsecases.DeleteDesk(adminId, deskId.ToString());

    //assert
    Assert.That(db.Desks.First(d => d.DeskId == deskId).IsMarkedForDeletion);
  }

  [Test]
  public void DeleteDesk_WhenNonValidDeskIsProvided_ShouldThrowEntityNotFoundException()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();

    SetupMockData(db, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);


    //act & assert
    Assert.Throws<EntityNotFoundException>(() => resourceUsecases.DeleteDesk(adminId, new Guid().ToString()));
  }

  [Test]
  public void DeleteDeskType_WhenValidDeskTypeIsProvided_ShouldUpdateDeskType()
  {
    //setup
    using var db = new DataContext();
    var deskTypeId = Guid.NewGuid();
    var adminId = Guid.NewGuid();
    var deskId = Guid.NewGuid();

    SetupMockData(db, deskTypeId: deskTypeId, userId: adminId, deskId: deskId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);
    resourceUsecases.DeleteDesk(adminId, deskId.ToString());
    //act
    resourceUsecases.DeleteDeskType(adminId, deskTypeId.ToString());

    //assert
    Assert.That(db.DeskTypes.First(d => d.DeskTypeId == deskTypeId).IsMarkedForDeletion);
  }

  [Test]
  public void DeleteDeskType_WhenNonValidDeskTypeIsProvided_ShouldThrowEntityNotFoundException()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();

    SetupMockData(db, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);


    //act & assert
    Assert.Throws<EntityNotFoundException>(() => resourceUsecases.DeleteDeskType(adminId, new Guid().ToString()));
  }

  [Test]
  public void RestoreBuilding_WhenValidBuildingIsProvided_ShouldUpdateBuilding()
  {
    //setup
    using var db = new DataContext();
    var buildingId = Guid.NewGuid();
    var adminId = Guid.NewGuid();

    SetupMockData(db, buildingId: buildingId, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);
    resourceUsecases.DeleteBuilding(adminId, buildingId.ToString());

    //act
    resourceUsecases.RestoreBuilding(adminId, buildingId.ToString());

    //assert
    Assert.False(db.Buildings.First(b => b.BuildingId == buildingId).IsMarkedForDeletion);
  }

  [Test]
  public void RestoreBuilding_WhenNonValidBuildingIsProvided_ShouldThrowEntityNotFoundException()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();

    SetupMockData(db, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);


    //act & assert
    Assert.Throws<EntityNotFoundException>(() => resourceUsecases.RestoreBuilding(adminId, new Guid().ToString()));
  }

  [Test]
  public void RestoreFloor_WhenValidFloorIsProvided_ShouldUpdateFloor()
  {
    //setup
    using var db = new DataContext();
    var floorId = Guid.NewGuid();
    var adminId = Guid.NewGuid();

    SetupMockData(db, floorId: floorId, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);
    resourceUsecases.DeleteFloor(adminId, floorId.ToString());

    //act
    resourceUsecases.RestoreFloor(adminId, floorId.ToString());

    //assert
    Assert.False(db.Floors.First(b => b.FloorId == floorId).IsMarkedForDeletion);
  }

  [Test]
  public void RestoreFloor_WhenNonValidFloorIsProvided_ShouldThrowEntityNotFoundException()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();

    SetupMockData(db, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);


    //act & assert
    Assert.Throws<EntityNotFoundException>(() => resourceUsecases.RestoreFloor(adminId, new Guid().ToString()));
  }

  [Test]
  public void RestoreRoom_WhenValidRoomIsProvided_ShouldUpdateRoom()
  {
    //setup
    using var db = new DataContext();
    var roomId = Guid.NewGuid();
    var adminId = Guid.NewGuid();

    SetupMockData(db, roomId: roomId, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);
    resourceUsecases.DeleteRoom(adminId, roomId.ToString());

    //act
    resourceUsecases.RestoreRoom(adminId, roomId.ToString());

    //assert
    Assert.False(db.Rooms.First(b => b.RoomId == roomId).IsMarkedForDeletion);
  }

  [Test]
  public void RestoreRoom_WhenNonValidRoomIsProvided_ShouldThrowEntityNotFoundException()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();

    SetupMockData(db, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);


    //act & assert
    Assert.Throws<EntityNotFoundException>(() => resourceUsecases.RestoreRoom(adminId, new Guid().ToString()));
  }

  [Test]
  public void RestoreDesk_WhenValidDeskIsProvided_ShouldUpdateDesk()
  {
    //setup
    using var db = new DataContext();
    var deskId = Guid.NewGuid();
    var adminId = Guid.NewGuid();

    SetupMockData(db, deskId: deskId, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);
    resourceUsecases.DeleteDesk(adminId, deskId.ToString());

    //act
    resourceUsecases.RestoreDesk(adminId, deskId.ToString());

    //assert
    Assert.False(db.Desks.First(b => b.DeskId == deskId).IsMarkedForDeletion);
  }

  [Test]
  public void RestoreDesk_WhenNonValidDeskIsProvided_ShouldThrowEntityNotFoundException()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();

    SetupMockData(db, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);


    //act & assert
    Assert.Throws<EntityNotFoundException>(() => resourceUsecases.RestoreDesk(adminId, new Guid().ToString()));
  }

  [Test]
  public void RestoreDeskType_WhenValidDeskTypeIsProvided_ShouldUpdateDeskType()
  {
    //setup
    using var db = new DataContext();
    var deskTypeId = Guid.NewGuid();
    var deskId = Guid.NewGuid();
    var adminId = Guid.NewGuid();

    SetupMockData(db, deskTypeId: deskTypeId, userId: adminId, deskId: deskId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);
    resourceUsecases.DeleteDesk(adminId, deskId.ToString());
    resourceUsecases.DeleteDeskType(adminId, deskTypeId.ToString());

    //act
    resourceUsecases.RestoreDeskType(adminId, deskTypeId.ToString());

    //assert
    Assert.False(db.DeskTypes.First(b => b.DeskTypeId == deskTypeId).IsMarkedForDeletion);
  }

  [Test]
  public void RestoreDeskType_WhenNonValidDeskTypeIsProvided_ShouldThrowEntityNotFoundException()
  {
    //setup
    using var db = new DataContext();
    var adminId = Guid.NewGuid();

    SetupMockData(db, userId: adminId);

    //arrange
    var logger = new Mock<ILogger<ResourceUsecases>>();
    var resourceUsecases = new ResourceUsecases(logger.Object, db);


    //act & assert
    Assert.Throws<EntityNotFoundException>(() => resourceUsecases.RestoreDeskType(adminId, new Guid().ToString()));
  }

  private UserUsecases SetupUserUsecases(DataContext db)
  {
    var logger = new Mock<ILogger<UserUsecases>>();
    var userUsecases = new UserUsecases(logger.Object, db);

    return userUsecases;
  }

  private void SetupMockData(DataContext moqDb, Guid companyId = new(), Guid userId = new(), Guid buildingId = new(),
    Guid floorId = new(), Guid roomId = new(), Guid deskTypeId = new(), Guid deskId = new())
  {
    if (companyId.ToString() == "00000000-0000-0000-0000-000000000000") companyId = Guid.NewGuid();
    if (userId.ToString() == "00000000-0000-0000-0000-000000000000") userId = Guid.NewGuid();
    if (buildingId.ToString() == "00000000-0000-0000-0000-000000000000") buildingId = Guid.NewGuid();
    if (floorId.ToString() == "00000000-0000-0000-0000-000000000000") floorId = Guid.NewGuid();
    if (roomId.ToString() == "00000000-0000-0000-0000-000000000000") roomId = Guid.NewGuid();
    if (deskTypeId.ToString() == "00000000-0000-0000-0000-000000000000") deskTypeId = Guid.NewGuid();
    if (deskId.ToString() == "00000000-0000-0000-0000-000000000000") deskId = Guid.NewGuid();
    var hasher = new PasswordHasher<User>();
    var company = new Company
    {
      CompanyId = companyId,
      CompanyName = "gehmalbierholn"
    };
    var user = new User
    {
      UserId = userId,
      MailAddress = "test@example.de",
      FirstName = "testF",
      LastName = "testL",
      CompanyId = company.CompanyId,
      IsApproved = true
    };
    user.Password = hasher.HashPassword(user, "testpw");
    var building = new Building
    {
      BuildingId = buildingId,
      BuildingName = "Gebäude1",
      Location = "Location1",
      CompanyId = company.CompanyId
    };
    var floor = new Floor
    {
      FloorId = floorId,
      FloorName = "Stockwerk1",
      BuildingId = building.BuildingId
    };
    var room = new Room
    {
      RoomId = roomId,
      FloorId = floor.FloorId,
      RoomName = "Raum1"
    };
    var deskTyp = new DeskType
    {
      DeskTypeId = deskTypeId,
      CompanyId = company.CompanyId,
      DeskTypeName = "Typ1"
    };
    var desk = new Desk
    {
      DeskId = deskId,
      DeskName = "Desk1",
      DeskTypeId = deskTyp.DeskTypeId,
      RoomId = room.RoomId
    };
    moqDb.Companies.Add(company);
    moqDb.Users.Add(user);
    moqDb.Buildings.Add(building);
    moqDb.Floors.Add(floor);
    moqDb.Rooms.Add(room);
    moqDb.DeskTypes.Add(deskTyp);
    moqDb.Desks.Add(desk);

    moqDb.SaveChanges();
  }
}
