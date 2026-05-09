using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingSlotMonitoring.API.Data;
using ParkingSlotMonitoring.API.Models;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ParkingSlotsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ParkingSlotsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var slots = _context.ParkingSlots.ToList();
        return Ok(slots);
    }

    [HttpPost("reserve")]
    public IActionResult ReserveSlot(int slotId, int userId)
    {
        var slot = _context.ParkingSlots.FirstOrDefault(s => s.slot_id == slotId);

        if (slot == null)
            return NotFound("Slot not found");

        if (slot.status != "Available")
            return BadRequest("Slot is not available");

        var reservation = new Reservation
        {
            user_id = userId,
            slot_id = slotId,
            start_time = DateTime.Now.Date,
            end_time = DateTime.Now.Date, 
            status = "Reserved",
            total_amount = 0
        };

        slot.status = "Reserved";

        _context.Reservations.Add(reservation);
        _context.SaveChanges();

        return Ok(reservation);
    }

    [HttpPost("checkin")]
    public IActionResult CheckIn(int slotId, int userId)
    {
        var reservation = _context.Reservations
            .FirstOrDefault(r => r.slot_id == slotId && r.user_id == userId && r.status == "Reserved");

        if (reservation == null)
            return BadRequest("No valid reservation found");

        var slot = _context.ParkingSlots.FirstOrDefault(s => s.slot_id == slotId);

        reservation.status = "CheckedIn";
        slot.status = "Occupied";

        _context.SaveChanges();

        return Ok("Checked in successfully");
    }

    [HttpPost("checkout")]
    public IActionResult CheckOut(int slotId, int userId)
    {
        var reservation = _context.Reservations
            .FirstOrDefault(r => r.slot_id == slotId && r.user_id == userId && r.status == "CheckedIn");

        if (reservation == null)
            return BadRequest("No active check-in found");

        var slot = _context.ParkingSlots.FirstOrDefault(s => s.slot_id == slotId);

        reservation.status = "Completed";
        reservation.end_time = DateTime.Now;

        slot.status = "Available";

        _context.SaveChanges();

        return Ok("Checked out successfully");
    }

    [Authorize(Roles = "2")]
    [HttpPut("update-status")]
    public IActionResult UpdateSlotStatus(int slotId, string status)
    {
        var slot = _context.ParkingSlots
            .FirstOrDefault(s => s.slot_id == slotId);

        if (slot == null)
            return NotFound("Slot not found");

        slot.status = status;

        _context.SaveChanges();

        return Ok("Slot status updated successfully");
    }

    [Authorize(Roles = "2")]
    [HttpPost("add-slot")]
    public IActionResult AddSlot(string slotNumber)
    {
        var slot = new ParkingSlot
        {
            slot_number = slotNumber,
            status = "Available",
            area_id = 1,
            row_number = 1,
            column_number = 1
        };

        _context.ParkingSlots.Add(slot);

        _context.SaveChanges();

        return Ok("Parking slot added successfully");
    }

    [Authorize(Roles = "2")]
    [HttpDelete("delete-slot")]
    public IActionResult DeleteSlot(int slotId)
    {
        var slot = _context.ParkingSlots
            .FirstOrDefault(s => s.slot_id == slotId);

        if (slot == null)
            return NotFound("Slot not found");

        _context.ParkingSlots.Remove(slot);

        _context.SaveChanges();

        return Ok("Parking slot deleted successfully");
    }

    [Authorize(Roles = "2")]
    [HttpGet("reservation-history")]
    public IActionResult GetReservationHistory()
    {
        var reservations = _context.Reservations
            .OrderByDescending(r => r.reservation_id)
            .ToList();

        return Ok(reservations);
    }
}