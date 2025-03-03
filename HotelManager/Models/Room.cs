
namespace HotelManager.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public bool IsAvailable { get; set; } = true;
        public int StatusId { get; set; } 
        public RoomStatus RoomStatus { get; set; } 

        public string? StatusDetails { get; set; }
    }
}
