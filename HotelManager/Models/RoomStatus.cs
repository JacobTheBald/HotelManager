namespace HotelManager.Models
{
    public class RoomStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Room> Rooms { get; set; } 
    }
}

