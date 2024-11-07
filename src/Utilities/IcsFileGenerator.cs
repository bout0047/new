using System.Text;
using RoomReservationBackend.Models;

namespace RoomReservationBackend.Utilities
{
    public interface IIcsFileGenerator
    {
        string GenerateIcsContent(Reservation reservation, Room room, User user);
    }

    public class IcsFileGenerator : IIcsFileGenerator
    {
        public string GenerateIcsContent(Reservation reservation, Room room, User user)
        {
            var sb = new StringBuilder();
            var now = DateTime.UtcNow;

            // Add required ICS fields
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:-//Marjane//Room Reservation System//EN");
            sb.AppendLine("CALSCALE:GREGORIAN");
            sb.AppendLine("METHOD:REQUEST");

            // Add event
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine($"DTSTART:{FormatDateTime(reservation.StartTime)}");
            sb.AppendLine($"DTEND:{FormatDateTime(reservation.EndTime)}");
            sb.AppendLine($"DTSTAMP:{FormatDateTime(now)}");
            sb.AppendLine($"ORGANIZER;CN={user.Name}:mailto:{user.Email}");
            sb.AppendLine($"UID:{Guid.NewGuid()}");
            sb.AppendLine($"CREATED:{FormatDateTime(now)}");
            sb.AppendLine($"DESCRIPTION:{EscapeString(reservation.Purpose)}");
            sb.AppendLine($"LAST-MODIFIED:{FormatDateTime(now)}");
            sb.AppendLine($"LOCATION:{EscapeString(room.Location)}");
            sb.AppendLine($"SEQUENCE:0");
            sb.AppendLine($"STATUS:{GetStatusText(reservation.Status)}");
            sb.AppendLine($"SUMMARY:{EscapeString($"Room Reservation - {room.Name}")}");
            sb.AppendLine("TRANSP:OPAQUE");
            sb.AppendLine("END:VEVENT");

            sb.AppendLine("END:VCALENDAR");

            return sb.ToString();
        }

        private string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToUniversalTime().ToString("yyyyMMddTHHmmssZ");
        }

        private string EscapeString(string text)
        {
            return text
                .Replace(",", "\\,")
                .Replace(";", "\\;")
                .Replace("\n", "\\n")
                .Replace("\r", "");
        }

        private string GetStatusText(ReservationStatus status)
        {
            return status switch
            {
                ReservationStatus.Confirmed => "CONFIRMED",
                ReservationStatus.Cancelled => "CANCELLED",
                _ => "TENTATIVE"
            };
        }
    }
}