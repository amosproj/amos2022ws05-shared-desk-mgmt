using Deskstar.Core.Exceptions;
using Deskstar.DataAccess;
using Deskstar.Entities;
using Deskstar.Usecases;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Teststar.Tests.Tests;

public class ResourceUsecaseTests
{
    [Test]
    public void UpdateFloor_WhenFloorDoesNotExist_ShouldThrowEntitiesNotFoundException()
    {
        // setup
        var companyId = Guid.NewGuid();
        var floorId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();
        var context = new DataContext();
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        context.Buildings.Add(building);
        context.SaveChanges();

        // arrange
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act+assert
        var ex = Assert.Throws<EntityNotFoundException>(() => resourceUsecases.UpdateFloor(companyId, floorId, "Floor Name", buildingId));
        Assert.NotNull(ex);
        Assert.That($"There is no floor with id '{floorId}'" == ex.Message);

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
        var building = new Building { BuildingId = buildingId, CompanyId = Guid.NewGuid(), BuildingName = "testname", Location = "testlocation" };
        context.Buildings.Add(building);
        var floor = new Floor { FloorId = floorId, FloorName = "testfloor", BuildingId = buildingId, Building = building };
        context.Floors.Add(floor);
        context.SaveChanges();

        // arrange
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act+assert
        var ex = Assert.Throws<InsufficientPermissionException>(() => resourceUsecases.UpdateFloor(companyId, floorId, "Floor Name", null));
        Assert.NotNull(ex);
        Assert.That($"'{companyId}' has no access to administrate floor '{floorId}'" == ex.Message);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        context.Buildings.Add(building);
        var floor = new Floor { FloorId = floorId, FloorName = "testfloor", BuildingId = buildingId, Building = building };
        context.Floors.Add(floor);
        context.SaveChanges();

        // arrange
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act+assert
        var ex = Assert.Throws<ArgumentInvalidException>(() => resourceUsecases.UpdateFloor(companyId, floorId, "", null));
        Assert.NotNull(ex);
        Assert.That($"Floor name must not be empty" == ex.Message);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        context.Buildings.Add(building);
        var floor = new Floor { FloorId = floorId, FloorName = "testfloor", BuildingId = buildingId, Building = building };
        context.Floors.Add(floor);
        context.SaveChanges();

        // arrange
        var buildingIdNotExisting = Guid.NewGuid();
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act+assert
        var ex = Assert.Throws<EntityNotFoundException>(() => resourceUsecases.UpdateFloor(companyId, floorId, "testname", buildingIdNotExisting));
        Assert.NotNull(ex);
        Assert.That($"Building does not exist with id '{buildingIdNotExisting}'" == ex.Message);

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
        var building = new Building { BuildingId = buildingId, CompanyId = Guid.NewGuid(), BuildingName = "testname", Location = "testlocation" };
        var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testname" };
        context.Buildings.Add(building);
        context.Floors.Add(floor);
        context.SaveChanges();

        // arrange
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act+assert
        var ex = Assert.Throws<InsufficientPermissionException>(() => resourceUsecases.UpdateFloor(companyId, floorId, "new name", null));
        Assert.NotNull(ex);
        Assert.That($"'{companyId}' has no access to administrate floor '{floorId}'" == ex.Message);

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

        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        var floor = new Floor { FloorId = floorId, FloorName = "testfloor", BuildingId = buildingId, Building = building };
        context.Buildings.Add(building);
        context.Floors.Add(floor);

        var anotherCompanyId = Guid.NewGuid();
        var building2Id = Guid.NewGuid();
        var building2 = new Building { BuildingId = building2Id, CompanyId = anotherCompanyId, BuildingName = "testbuilding2", Location = "testlocation" };
        context.Buildings.Add(building2);
        context.SaveChanges();

        // arrange
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act+assert
        var ex = Assert.Throws<InsufficientPermissionException>(() => resourceUsecases.UpdateFloor(companyId, floorId, null, building2Id));
        Assert.NotNull(ex);
        Assert.AreEqual($"'{companyId}' has no access to move a floor to building '{building2Id}'", ex.Message);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        var floor = new Floor { FloorId = floorId, FloorName = floorName, BuildingId = buildingId, Building = building };
        context.Buildings.Add(building);
        context.Floors.Add(floor);

        var anotherCompanyId = Guid.NewGuid();
        var building2Id = Guid.NewGuid();
        var building2 = new Building { BuildingId = building2Id, CompanyId = anotherCompanyId, BuildingName = "testbuilding2", Location = "testlocation" };
        context.Buildings.Add(building2);
        context.SaveChanges();

        // arrange
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act+assert
        var ex = Assert.Throws<ArgumentInvalidException>(() => resourceUsecases.UpdateFloor(companyId, floorId, floorName, null));
        Assert.NotNull(ex);
        Assert.AreEqual($"There is already a floor named '{floorName}' in building '{buildingId}'", ex.Message);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        context.Buildings.Add(building);
        var building2Id = Guid.NewGuid();
        var building2 = new Building { BuildingId = building2Id, CompanyId = companyId, BuildingName = "testbuilding2", Location = "testlocation" };
        context.Buildings.Add(building2);
        var floor = new Floor { FloorId = floorId, FloorName = "testfloor", BuildingId = buildingId, Building = building };
        context.Floors.Add(floor);
        context.SaveChanges();

        // arrange
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);
        var newFloorName = "new name";

        // act
        var returnedFloorId = resourceUsecases.UpdateFloor(companyId, floorId, newFloorName, building2Id);
        var updatedFloor = context.Floors.SingleOrDefault(f => f.FloorId == floorId);

        // assert
        Assert.That(floorId == returnedFloorId);
        Assert.NotNull(updatedFloor);
        Assert.That(newFloorName == updatedFloor.FloorName);
        Assert.That(building2Id == updatedFloor.BuildingId);

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

        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        var floor = new Floor { FloorId = floorId, FloorName = floorName, BuildingId = buildingId };
        
        context.Buildings.Add(building);
        context.Floors.Add(floor);

        var company2Id = Guid.NewGuid();
        var building2Id = Guid.NewGuid();
        var floor2Id = Guid.NewGuid();

        var building2 = new Building { BuildingId = building2Id, CompanyId = companyId, BuildingName = "testbuilding2", Location = "testlocation" };
        var floor2 = new Floor { FloorId = floor2Id, FloorName = floorName, BuildingId = building2Id };
        
        context.Buildings.Add(building2);
        context.Floors.Add(floor2);

        context.SaveChanges();

        // arrange
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act+assert
        var ex = Assert.Throws<ArgumentInvalidException>(() => resourceUsecases.UpdateFloor(companyId, floorId, floorName, building2Id));
        Assert.NotNull(ex);
        Assert.AreEqual($"You cant move floor '{floorId}' to building '{building2Id}'. In building '{building2Id}' already exists a floor called '{floorName}'", ex.Message);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        context.Buildings.Add(building);
        var floorName = "testfloor";
        var floor = new Floor { FloorId = floorId, FloorName = floorName, BuildingId = buildingId, Building = building };
        context.Floors.Add(floor);
        context.SaveChanges();

        // arrange
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);
        var newFloorName = "newFloorName";

        // act
        var returnedFloorId = resourceUsecases.UpdateFloor(companyId, floorId, newFloorName, null);
        var updatedFloor = context.Floors.SingleOrDefault(f => f.FloorId == floorId);

        // assert
        Assert.That(floorId == returnedFloorId);
        Assert.NotNull(updatedFloor);
        Assert.That(buildingId == updatedFloor.BuildingId);
        Assert.That(newFloorName == updatedFloor.FloorName);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        context.Buildings.Add(building);
        var building2Id = Guid.NewGuid();
        var building2 = new Building { BuildingId = building2Id, CompanyId = companyId, BuildingName = "testbuilding2", Location = "testlocation" };
        context.Buildings.Add(building2);
        var floorName = "testfloor";
        var floor = new Floor { FloorId = floorId, FloorName = floorName, BuildingId = buildingId, Building = building };
        context.Floors.Add(floor);
        context.SaveChanges();

        // arrange
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act
        var returnedFloorId = resourceUsecases.UpdateFloor(companyId, floorId, null, building2Id);
        var updatedFloor = context.Floors.SingleOrDefault(f => f.FloorId == floorId);

        // assert
        Assert.That(floorId == returnedFloorId);
        Assert.NotNull(updatedFloor);
        Assert.That(building2Id == updatedFloor.BuildingId);
        Assert.That(floorName == updatedFloor.FloorName);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        context.Buildings.Add(building);
        var floorName = "testfloor";
        var floor = new Floor { FloorId = floorId, FloorName = floorName, BuildingId = buildingId, Building = building };
        context.Floors.Add(floor);
        context.SaveChanges();

        // arrange
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act
        var returnedFloorId = resourceUsecases.UpdateFloor(companyId, floorId, null, null);
        var updatedFloor = context.Floors.SingleOrDefault(f => f.FloorId == floorId);

        // assert
        Assert.That(floorId == returnedFloorId);
        Assert.NotNull(updatedFloor);
        Assert.That(buildingId == updatedFloor.BuildingId);
        Assert.That(floorName == updatedFloor.FloorName);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
        var roomName = "testroom";
        var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
        context.Buildings.Add(building);
        context.Floors.Add(floor);
        context.Rooms.Add(room);

        var anotherBuildingId = Guid.NewGuid();
        var anotherFloorId = Guid.NewGuid();
        var anotherRoomId = Guid.NewGuid();
        var anotherBuilding = new Building { BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation" };
        var anotherFloor = new Floor { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "testfloor" };
        var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = roomName };
        context.Buildings.Add(anotherBuilding);
        context.Floors.Add(anotherFloor);
        context.SaveChanges();

        // arrange
        var userLogger = new Mock<ILogger<UserUsecases>>();
        var userUsecases = new UserUsecases(userLogger.Object, context);
        var resourceLogger = new Mock<ILogger<ResourceUsecases>>();
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act+assert
        var ex = Assert.Throws<EntityNotFoundException>(() => resourceUsecases.UpdateRoom(companyId, anotherRoomId, "Room Name", floorId));
        Assert.NotNull(ex);
        Assert.AreEqual($"There is no room with id '{anotherRoomId}'", ex.Message);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
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
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act+assert
        var ex = Assert.Throws<InsufficientPermissionException>(() => resourceUsecases.UpdateRoom(anotherCompanyId, roomId, "Room Name", floorId));
        Assert.NotNull(ex);
        Assert.That($"'{anotherCompanyId}' has no access to administrate floor '{floorId}'" == ex.Message);

        // cleanup
        context.Database.EnsureDeleted();
    }

    [Test]
    public void UpdateRoom_WhenRoomNameIsEmpty_ShouldThrowArgumentInvalidException()
    {
        // setup
        var companyId = Guid.NewGuid();
        var anotherCompanyId = Guid.NewGuid();
        var floorId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var context = new DataContext();
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
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
        var resourceUsecases = new ResourceUsecases(resourceLogger.Object, context, userUsecases);

        // act and assert
        var ex = Assert.Throws<ArgumentInvalidException>(() => resourceUsecases.UpdateRoom(companyId, roomId, "", floorId));
        Assert.NotNull(ex);
        Assert.That($"Room name must not be empty" == ex.Message);

        // cleanup
        context.Database.EnsureDeleted();
    }

    [Test]
    public void UpdateRoom_WhenUpdatedFloorDoesNotExist_ShouldThrowEntityNotFoundException()
    {
        // setup
        var companyId = Guid.NewGuid();
        var anotherCompanyId = Guid.NewGuid();
        var floorId = Guid.NewGuid();
        var anotherFloorId = Guid.NewGuid();
        var buildingId = Guid.NewGuid();
        var roomId = Guid.NewGuid();
        var context = new DataContext();
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
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
        var resourceUsecases = new ResourceUsecases(logger.Object, context, userUsecases.Object);

        // act+assert
        var ex = Assert.Throws<EntityNotFoundException>(() => resourceUsecases.UpdateRoom(companyId, roomId, roomName, anotherFloorId));
        Assert.NotNull(ex);
        Assert.AreEqual($"Floor does not exist with id '{anotherFloorId}'", ex.Message);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        var anotherBuilding = new Building { BuildingId = anotherBuildingId, CompanyId = anotherCompanyId, BuildingName = "anothertestname", Location = "testlocation" };
        var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
        var anotherFloor = new Floor { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "testfloor" };
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
        var usecases = new ResourceUsecases(logger.Object, context, userUsecases.Object);

        // act+assert
        var ex = Assert.Throws<InsufficientPermissionException>(() => usecases.UpdateRoom(companyId, roomId, "New Room", anotherFloorId));
        Assert.NotNull(ex);
        Assert.AreEqual($"'{companyId}' has no access to move a room to floor '{anotherFloorId}'", ex.Message);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
        var roomName = "testroom";
        var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
        context.Buildings.Add(building);
        context.Floors.Add(floor);
        context.Rooms.Add(room);

        var anotherFloorId = Guid.NewGuid();
        var anotherBuildingId = Guid.NewGuid();
        var anotherBuilding = new Building { BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation" };
        var anotherFloor = new Floor { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "testfloor" };
        context.Buildings.Add(anotherBuilding);
        context.Floors.Add(anotherFloor);
        context.SaveChanges();

        // arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var userUsecases = new Mock<IUserUsecases>();
        var usecases = new ResourceUsecases(logger.Object, context, userUsecases.Object);
        var updatedRoomName = "updatedRoomName";

        // act
        var gid = usecases.UpdateRoom(companyId, roomId, updatedRoomName, anotherFloorId);
        var updatedRoom = context.Rooms.SingleOrDefault(r => r.RoomId == gid);

        // assert
        Assert.NotNull(updatedRoom);
        Assert.AreEqual(updatedRoomName, updatedRoom.RoomName);
        Assert.AreEqual(anotherFloorId, updatedRoom.FloorId);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
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
        var usecases = new ResourceUsecases(logger.Object, context, userUsecases.Object);
        var updatedRoomName = "updatedRoomName";

        // act
        var gid = usecases.UpdateRoom(companyId, roomId, updatedRoomName, null);
        var updatedRoom = context.Rooms.SingleOrDefault(r => r.RoomId == gid);

        // assert
        Assert.NotNull(updatedRoom);
        Assert.AreEqual(updatedRoomName, updatedRoom.RoomName);
        Assert.AreEqual(floorId, updatedRoom.FloorId);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
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
        var usecases = new ResourceUsecases(logger.Object, context, userUsecases.Object);

        // act+assert
        var ex = Assert.Throws<ArgumentInvalidException>(() => usecases.UpdateRoom(companyId, roomId, roomName, null));
        Assert.NotNull(ex);
        Assert.AreEqual($"There is already a room named '{roomName}' in floor '{floorId}'", ex.Message);

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
        
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
        var roomName = "testroom";
        var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
        
        context.Buildings.Add(building);
        context.Floors.Add(floor);
        context.Rooms.Add(room);

        var anotherFloorId = Guid.NewGuid();
        var anotherBuildingId = Guid.NewGuid();
        var anotherRoomId = Guid.NewGuid();

        var anotherBuilding = new Building { BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation" };
        var anotherFloor = new Floor { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "testfloor" };
        var anotherRoom = new Room { RoomId = anotherRoomId, FloorId = anotherFloorId, RoomName = roomName };
        
        context.Buildings.Add(anotherBuilding);
        context.Floors.Add(anotherFloor);
        context.Rooms.Add(anotherRoom);
        
        context.SaveChanges();

        // arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var userUsecases = new Mock<IUserUsecases>();
        var usecases = new ResourceUsecases(logger.Object, context, userUsecases.Object);

        // act+assert
        var ex = Assert.Throws<ArgumentInvalidException>(() => usecases.UpdateRoom(companyId, roomId, roomName, anotherFloorId));
        Assert.NotNull(ex);
        Assert.AreEqual($"You cant move room '{roomId}' to floor '{anotherFloorId}'. In floor '{anotherFloorId}' already exists a room called '{roomName}'", ex.Message);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
        var floor = new Floor { FloorId = floorId, BuildingId = buildingId, FloorName = "testfloor" };
        var roomName = "testroom";
        var room = new Room { RoomId = roomId, FloorId = floorId, RoomName = roomName };
        context.Buildings.Add(building);
        context.Floors.Add(floor);
        context.Rooms.Add(room);

        var anotherFloorId = Guid.NewGuid();
        var anotherBuildingId = Guid.NewGuid();
        var anotherBuilding = new Building { BuildingId = anotherBuildingId, CompanyId = companyId, BuildingName = "anothertestname", Location = "testlocation" };
        var anotherFloor = new Floor { FloorId = anotherFloorId, BuildingId = anotherBuildingId, FloorName = "testfloor" };
        context.Buildings.Add(anotherBuilding);
        context.Floors.Add(anotherFloor);
        context.SaveChanges();

        // arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var userUsecases = new Mock<IUserUsecases>();
        var usecases = new ResourceUsecases(logger.Object, context, userUsecases.Object);

        // act
        var gid = usecases.UpdateRoom(companyId, roomId, null, anotherFloorId);
        var updatedRoom = context.Rooms.SingleOrDefault(r => r.RoomId == gid);

        // assert
        Assert.NotNull(updatedRoom);
        Assert.AreEqual(roomName, updatedRoom.RoomName);
        Assert.AreEqual(anotherFloorId, updatedRoom.FloorId);

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
        var building = new Building { BuildingId = buildingId, CompanyId = companyId, BuildingName = "testname", Location = "testlocation" };
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
        var usecases = new ResourceUsecases(logger.Object, context, userUsecases.Object);

        // act
        var gid = usecases.UpdateRoom(companyId, roomId, null, null);
        var updatedRoom = context.Rooms.SingleOrDefault(r => r.RoomId == gid);

        // assert
        Assert.NotNull(updatedRoom);
        Assert.AreEqual(roomName, updatedRoom.RoomName);
        Assert.AreEqual(floorId, updatedRoom.FloorId);

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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetBuildings(userId);

        //assert
        Assert.That(result, Is.Empty);

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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetBuildings(userId);

        //assert
        Assert.That(result, Is.Empty);

        //cleanup
        db.Database.EnsureDeleted();
    }

    [Test]
    public void GetFloors_WhenBuildingNotExsits_ShouldReturnAException()
    {
        //setup 
        using var db = new DataContext();

        var buildingId = Guid.NewGuid();
        SetupMockData(db, buildingId: buildingId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var callId = Guid.NewGuid();

        //act
        try
        {
            usecases.GetFloors(callId);

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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetDesks(roomId, DateTime.Now, DateTime.Now);

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
        SetupMockData(db, roomId: roomId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var callId = Guid.NewGuid();

        //act
        try
        {
            usecases.GetDesks(callId, start, end);

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
    public void GetDesks_WhenDeskIsFound_StartBevoreEndIn_ShouldReturnACurrentsDeskList()
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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetDesks(roomId, start, end);

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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetDesks(roomId, start, end);

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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


        //act
        var result = usecases.GetDesks(roomId, start, end);

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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));


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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));

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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));

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
        SetupMockData(db, companyId: companyId, roomId: roomId);

        db.SaveChanges();

        //arrange
        var deskName = "validDeskName";
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));

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
        SetupMockData(db, companyId: companyId, deskTypeId: deskTypeId);

        db.SaveChanges();

        //arrange
        var deskName = "validDeskName";
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));

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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId, floorId: floorId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId, floorId: floorId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId, floorId: floorId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId, buildingId: buildingId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId, buildingId: buildingId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId, buildingId: buildingId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
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
        SetupMockData(db, companyId: companyId);

        db.SaveChanges();

        //arrange
        var logger = new Mock<ILogger<ResourceUsecases>>();
        var usecases = new ResourceUsecases(logger.Object, db, SetupUserUsecases(db));
        var buildingName = "validName";
        var location = "validLocation";

        //act+assert
        Assert.DoesNotThrow(() => usecases.CreateBuilding(buildingName, location, companyId));

        //cleanup
        db.Database.EnsureDeleted();
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