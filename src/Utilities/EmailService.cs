using System.Net;
using System.Net.Mail;
using RoomReservationBackend.Models;

namespace RoomReservationBackend.Utilities
{
    public interface IEmailService
    {
        Task SendReservationConfirmationAsync(Reservation reservation, User user, Room room);
        Task SendReservationUpdateAsync(Reservation reservation, User user, Room room);
        Task SendReservationCancellationAsync(Reservation reservation, User user, Room room);
    }

    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _fromPassword;

        public EmailService(string smtpServer, int smtpPort, string fromEmail, string fromPassword)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _fromEmail = fromEmail;
            _fromPassword = fromPassword;
        }

        public async Task SendReservationConfirmationAsync(Reservation reservation, User user, Room room)
        {
            var subject = $"Reservation Confirmation - {room.Name}";
            var body = GenerateReservationEmailBody(reservation, user, room, "confirmed");
            await SendEmailAsync(user.Email, subject, body);
        }

        public async Task SendReservationUpdateAsync(Reservation reservation, User user, Room room)
        {
            var subject = $"Reservation Update - {room.Name}";
            var body = GenerateReservationEmailBody(reservation, user, room, "updated");
            await SendEmailAsync(user.Email, subject, body);
        }

        public async Task SendReservationCancellationAsync(Reservation reservation, User user, Room room)
        {
            var subject = $"Reservation Cancellation - {room.Name}";
            var body = GenerateReservationEmailBody(reservation, user, room, "cancelled");
            await SendEmailAsync(user.Email, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using var smtpClient = new SmtpClient(_smtpServer, _smtpPort)
                {
                    Credentials = new NetworkCredential(_fromEmail, _fromPassword),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, "Marjane Room Reservation"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the error but don't throw to prevent disrupting the main flow
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }

        private string GenerateReservationEmailBody(Reservation reservation, User user, Room room, string action)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>Room Reservation {action.ToUpperInvariant()}</h2>
                    <p>Dear {user.Name},</p>
                    <p>Your room reservation has been {action}.</p>
                    <div style='margin: 20px 0; padding: 15px; background-color: #f5f5f5; border-radius: 5px;'>
                        <h3>Reservation Details:</h3>
                        <p><strong>Room:</strong> {room.Name}</p>
                        <p><strong>Location:</strong> {room.Location}</p>
                        <p><strong>Date:</strong> {reservation.StartTime.ToShortDateString()}</p>
                        <p><strong>Time:</strong> {reservation.StartTime.ToShortTimeString()} - {reservation.EndTime.ToShortTimeString()}</p>
                        <p><strong>Purpose:</strong> {reservation.Purpose}</p>
                        {(!string.IsNullOrEmpty(reservation.Notes) ? $"<p><strong>Notes:</strong> {reservation.Notes}</p>" : "")}
                    </div>
                    <p>If you have any questions, please contact the facility management.</p>
                    <p>Best regards,<br>Marjane Room Reservation System</p>
                </body>
                </html>";
        }
    }
}