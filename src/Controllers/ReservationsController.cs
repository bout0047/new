using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using RoomReservationBackend.DTOs;
using RoomReservationBackend.Services;

namespace RoomReservationBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] int? roomId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                var reservations = await _reservationService.GetReservationsAsync(userId, startDate, endDate, roomId);
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching reservations.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReservationDto>> GetReservation(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                var reservation = await _reservationService.GetReservationByIdAsync(id, userId);
                if (reservation == null)
                {
                    return NotFound();
                }

                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching the reservation.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<ReservationDto>> CreateReservation(CreateReservationDto createReservationDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                var reservation = await _reservationService.CreateReservationAsync(userId, createReservationDto);
                return CreatedAtAction(nameof(GetReservation), new { id = reservation.ReservationId }, reservation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the reservation.");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ReservationDto>> UpdateReservation(int id, UpdateReservationDto updateReservationDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                var reservation = await _reservationService.UpdateReservationAsync(id, userId, updateReservationDto);
                if (reservation == null)
                {
                    return NotFound();
                }

                return Ok(reservation);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the reservation.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                var result = await _reservationService.DeleteReservationAsync(id, userId);
                if (!result)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the reservation.");
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ReservationDto>> UpdateReservationStatus(
            int id,
            [FromBody] ReservationStatus status)
        {
            try
            {
                var reservation = await _reservationService.UpdateReservationStatusAsync(id, status);
                if (reservation == null)
                {
                    return NotFound();
                }

                return Ok(reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the reservation status.");
            }
        }
    }
}