using Microsoft.AspNetCore.Mvc;
using System.Linq;
using HotelManager.Models;
using HotelManager.Data;

[ApiController]
[Route("api/rooms")]
public class RoomsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RoomsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAllRooms([FromQuery] string? name, [FromQuery] int? size, [FromQuery] bool? available)
    {
        var result = _context.Rooms.AsQueryable();
        if (!result.Any())
        {
            return Ok(new List<Room>());
        }

        if (!string.IsNullOrEmpty(name))
            result = result.Where(r => r.Name.Contains(name));
        if (size.HasValue)
            result = result.Where(r => r.Size == size);
        if (available.HasValue)
            result = result.Where(r => r.IsAvailable == available);

        return Ok(result.ToList());
    }

    [HttpGet("{id}")]
    public IActionResult GetRoomById(int id)
    {
        var room = _context.Rooms.Find(id);
        if (room == null) return NotFound();
        return Ok(room);
    }

    [HttpPost]
    public IActionResult CreateRoom([FromBody] Room room)
    {
        _context.Rooms.Add(room);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateRoom(int id, [FromBody] Room updatedRoom)
    {
        var room = _context.Rooms.Find(id);
        if (room == null) return NotFound();

        room.Name = updatedRoom.Name;
        room.Size = updatedRoom.Size;
        room.IsAvailable = updatedRoom.IsAvailable;
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public IActionResult UpdateRoomStatus(int id, [FromBody] RoomStatusUpdate update)
    {
        var room = _context.Rooms.Find(id);
        if (room == null) return NotFound();

        if ((update.Status.Name == "ManuallyLocked " || update.Status.Name == "Maintenance") && string.IsNullOrEmpty(update.Details))
        {
            return BadRequest("Details are required for this status update.");
        }

        room.RoomStatus = update.Status;
        room.StatusDetails = update.Details;
        room.IsAvailable = _CheckIfRoomIsFree(update.Status);
        _context.SaveChanges();

        return NoContent();
    }

    private bool _CheckIfRoomIsFree(RoomStatus status)
    {
        switch (status.Name)
        {
            case "Booked":
                return false;
            case "Cleaning":
                return false;
            case "Maintenance":
                return false;
            case "ManuallyLocked":
                return false;
            default: 
                return true;
        }
    }
}