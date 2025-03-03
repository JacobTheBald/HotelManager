using HotelManager.Data;
using HotelManager.Models;

public static class DatabaseSeeder
{
    public static void Seed(ApplicationDbContext dbContext)
    {
        if (!dbContext.RoomStatuses.Any())
        {
            var statuses = new List<RoomStatus>
                {
                    new RoomStatus { Name = "Available" },
                    new RoomStatus { Name = "Booked" },
                    new RoomStatus { Name = "Cleaning" },
                    new RoomStatus { Name = "Maintenance" },
                    new RoomStatus { Name = "ManuallyLocked" }
                };

            dbContext.RoomStatuses.AddRange(statuses);
            dbContext.SaveChanges();
        }


        var roomStatusDict = dbContext.RoomStatuses.ToDictionary(s => s.Name, s => s.Id);

        if (!dbContext.Rooms.Any())
        {
            dbContext.Rooms.AddRange(new List<Room>
                {
                    new Room { Name = "Deluxe Suite", Size = 2, StatusId = roomStatusDict["Available"] },
                    new Room { Name = "Family Room", Size = 4, StatusId = roomStatusDict["Available"] },
                    new Room { Name = "Executive Suite", Size = 3, StatusId = roomStatusDict["Available"] },
                    new Room { Name = "Double Room", Size = 2, StatusId = roomStatusDict["Available"] },
                    new Room { Name = "VIP Lounge", Size = 4, StatusId = roomStatusDict["Available"] },
                    new Room { Name = "Honeymoon Suite", Size = 2, StatusId = roomStatusDict["Available"] },
                    new Room { Name = "Standard Twin", Size = 2, StatusId = roomStatusDict["Available"] },
                    new Room { Name = "King Suite", Size = 4, StatusId = roomStatusDict["Available"] },
                    new Room { Name = "Queen Room", Size = 2, StatusId = roomStatusDict["Available"] },
                    new Room { Name = "Budget Room", Size = 1, StatusId = roomStatusDict["Available"] },
                    new Room { Name = "Single Room", Size = 1, StatusId = roomStatusDict["Booked"], StatusDetails = "Reserved for guest John Doe" },
                    new Room { Name = "Economy Room", Size = 1, StatusId = roomStatusDict["Cleaning"], StatusDetails = "Cleaning in progress" },
                    new Room { Name = "Luxury Loft", Size = 3, StatusId = roomStatusDict["Maintenance"], StatusDetails = "Leaky sink in bathroom" },
                    new Room { Name = "Penthouse", Size = 5, StatusId = roomStatusDict["ManuallyLocked"], StatusDetails = "Reserved for VIP guest" },
                    new Room { Name = "Presidential Suite", Size = 6, StatusId = roomStatusDict["ManuallyLocked"], StatusDetails = "Under high-security reservation" }
                });

            dbContext.SaveChanges();
        }

    }
}