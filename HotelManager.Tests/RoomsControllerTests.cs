using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using HotelManager.Data;
using HotelManager.Models;

public class RoomsControllerTests
{
    private ApplicationDbContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDB_" + System.Guid.NewGuid()) // Unique DB for each test
            .Options;

        var context = new ApplicationDbContext(options, new ConfigurationBuilder().Build());

        // Seed data
        var statusAvailable = new RoomStatus { Id = 1, Name = "Available" };
        var statusBooked = new RoomStatus { Id = 2, Name = "Booked" };
        var statusMaintenance = new RoomStatus { Id = 3, Name = "Maintenance" };
        
        context.RoomStatuses.AddRange(statusAvailable, statusBooked, statusMaintenance);
        context.SaveChanges();

        context.Rooms.AddRange(
            new Room { Id = 1, Name = "Room A", Size = 2, IsAvailable = true, StatusId = 1 },
            new Room { Id = 2, Name = "Room B", Size = 3, IsAvailable = false, StatusId = 2 }
        );

        context.SaveChanges();
        return context;
    }

    [Fact]
    public void GetAllRooms_ShouldReturnAllRooms()
    {
        using (var context = GetDatabaseContext())
        {
            var controller = new RoomsController(context);
            var result = controller.GetAllRooms(null, null, null) as OkObjectResult;

            Assert.NotNull(result);
            var rooms = Assert.IsType<List<Room>>(result.Value);
            Assert.Equal(2, rooms.Count);
        }
    }

    [Fact]
    public void GetAllRooms_WithFilter_ShouldReturnMatchingRooms()
    {
        using (var context = GetDatabaseContext())
        {
            var controller = new RoomsController(context);
            var result = controller.GetAllRooms("Room A", null, null) as OkObjectResult;

            Assert.NotNull(result);
            var rooms = Assert.IsType<List<Room>>(result.Value);
            Assert.Single(rooms);
            Assert.Equal("Room A", rooms[0].Name);
        }
    }

    [Fact]
    public void GetRoomById_ShouldReturnCorrectRoom()
    {
        using (var context = GetDatabaseContext())
        {
            var controller = new RoomsController(context);
            var result = controller.GetRoomById(1) as OkObjectResult;

            Assert.NotNull(result);
            var room = Assert.IsType<Room>(result.Value);
            Assert.Equal(1, room.Id);
            Assert.Equal("Room A", room.Name);
        }
    }

    [Fact]
    public void GetRoomById_ShouldReturnNotFoundForInvalidId()
    {
        using (var context = GetDatabaseContext())
        {
            var controller = new RoomsController(context);
            var result = controller.GetRoomById(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }

    [Fact]
    public void CreateRoom_ShouldAddRoom()
    {
        using (var context = GetDatabaseContext())
        {
            var controller = new RoomsController(context);
            var newRoom = new Room { Name = "New Room", Size = 4, StatusId = 1 };
            var result = controller.CreateRoom(newRoom) as CreatedAtActionResult;

            Assert.NotNull(result);
            var createdRoom = Assert.IsType<Room>(result.Value);
            Assert.Equal("New Room", createdRoom.Name);
            Assert.Equal(3, context.Rooms.Count());
        }
    }

    [Fact]
    public void UpdateRoom_ShouldModifyRoomDetails()
    {
        using (var context = GetDatabaseContext())
        {
            var controller = new RoomsController(context);
            var updatedRoom = new Room { Name = "Updated Room", Size = 5, StatusId = 1 };
            var result = controller.UpdateRoom(1, updatedRoom);

            Assert.IsType<NoContentResult>(result);
            var room = context.Rooms.Find(1);
            Assert.Equal("Updated Room", room.Name);
            Assert.Equal(5, room.Size);
        }
    }

    [Fact]
    public void UpdateRoomStatus_ShouldModifyRoomStatus()
    {
        using (var context = GetDatabaseContext())
        {
            var controller = new RoomsController(context);
            var statusUpdate = new RoomStatusUpdate { Status = context.RoomStatuses.First(rs => rs.Name == "Booked"), Details = "Guest arrived" };
            var result = controller.UpdateRoomStatus(1, statusUpdate);

            Assert.IsType<NoContentResult>(result);
            var room = context.Rooms.Find(1);
            Assert.Equal("Booked", room.RoomStatus.Name);
            Assert.Equal("Guest arrived", room.StatusDetails);
        }
    }

    [Fact]
    public void UpdateRoomStatus_ShouldRequireDetailsForCertainStatuses()
    {
        using (var context = GetDatabaseContext())
        {
            var controller = new RoomsController(context);
            var statusUpdate = new RoomStatusUpdate { Status = context.RoomStatuses.First(rs => rs.Name == "Maintenance"), Details = "" };
            var result = controller.UpdateRoomStatus(1, statusUpdate);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
