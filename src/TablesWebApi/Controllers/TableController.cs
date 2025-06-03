using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TablesWebApi.DAL;

namespace TablesWebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TableController: ControllerBase
    {
        private readonly ReservationDbContext _reservationDb;
        public TableController (ReservationDbContext reservationDb)
        {
            _reservationDb = reservationDb;
        }

        [HttpGet("availability")]
        public async Task<IActionResult> GetAvailability([FromQuery] DateTime date, [FromQuery] int peopleCount)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (peopleCount < 1 || peopleCount > 10)
                return BadRequest("People count must be between 1 and 10.");

            var hours = Enumerable.Range(0, 24)
                .Select(h => TimeSpan.FromHours(h).ToString(@"hh\:mm"))
                .ToList();

            var reservations = await _reservationDb.Reservations
                .Where(r => r.ReservationDate == date.Date)
                .Include(r => r.ReservationTableLinks)
                .ToListAsync();

            var tables = await _reservationDb.Tables
                .Where(t => t.MaxSeats >= peopleCount)
                .ToListAsync();

            var tableAvailability = tables.Select(table =>
            {
                var hourAvailability = new Dictionary<string, bool>();
                var hourReservedByUser = new Dictionary<string, bool>();

                foreach (var hour in hours)
                {
                    var time = TimeSpan.Parse(hour);

                    var matchingReservation = reservations.FirstOrDefault(r =>
                        r.ReservationTableLinks.Any(link => link.TableId == table.Id) &&
                        r.StartTime <= time && r.EndTime > time
                    );

                    hourAvailability[hour] = matchingReservation == null;
                    if (matchingReservation != null)
                    {
                        hourReservedByUser[hour] = matchingReservation.UserId == userId;
                    }
                    else
                    {
                        hourReservedByUser[hour] = false;
                    }
                }

                return new
                {
                    table.TableNumber,
                    hourAvailability,
                    hourReservedByUser
                };
            });

            return Ok(new
            {
                hours,
                tables = tableAvailability
            });
        }


        public class CreateReservationRequest
        {
            public DateTime Date { get; set; }
            public string StartHour { get; set; } = default!;
            public int PeopleCount { get; set; }
            public string TableNumber { get; set; } = default!;
            public string Name { get; set; } = default!;
            public string Email { get; set; } = default!;
            public string PhoneNumber { get; set; } = default!;
            public string? Note { get; set; }
        }

        [HttpPost("reserve")]
        public async Task<IActionResult> Reserve([FromBody] CreateReservationRequest request)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound("User ID not found.");
            }
            var table = await _reservationDb.Tables
                .FirstOrDefaultAsync(t => t.TableNumber.ToString() == request.TableNumber);

            if (table == null)
                return NotFound("Table not found");

            var start = TimeSpan.Parse(request.StartHour);
            var end = start.Add(TimeSpan.FromHours(1));

            var reservation = new Reservation
            {
                UserId = userId,
                ReservationDate = request.Date.Date,
                StartTime = start,
                EndTime = end,
                PeopleCount = request.PeopleCount,
                Note = request.Note
            };

            _reservationDb.Reservations.Add(reservation);

            var contact = new ReservationContact
            {
                Reservation = reservation,
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            _reservationDb.ReservationContacts.Add(contact);

            _reservationDb.ReservationTableLinks.Add(new ReservationTableLink
            {
                Reservation = reservation,
                TableId = table.Id
            });

            await _reservationDb.SaveChangesAsync();
            return Ok("Reservation successful");
        }

        public class CancelReservationDto
        {
            public DateTime Date { get; set; }
            public string Hour { get; set; }
            public int TableNumber { get; set; }
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> CancelReservation([FromBody] CancelReservationDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User ID not found.");

            if (dto == null || string.IsNullOrEmpty(dto.Hour) || dto.TableNumber <= 0)
                return BadRequest("Invalid cancellation request.");

            var parsedHour = TimeSpan.TryParse(dto.Hour, out var hour);
            if (!parsedHour)
                return BadRequest("Invalid hour format.");

            var reservation = await _reservationDb.Reservations
                .Include(r => r.ReservationTableLinks)
                .FirstOrDefaultAsync(r =>
                    r.ReservationDate == dto.Date.Date &&
                    r.StartTime <= hour &&
                    r.EndTime > hour &&
                    r.ReservationTableLinks.Any(link => link.Table.TableNumber == dto.TableNumber) &&
                    r.UserId == userId);

            if (reservation == null)
                return NotFound("Reservation not found or does not belong to the user.");

            _reservationDb.Reservations.Remove(reservation);
            await _reservationDb.SaveChangesAsync();

            return Ok(new { message = "Reservation cancelled successfully." });
        }
    }
}
