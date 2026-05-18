using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinanzasPersonales.NET.Services
{
    public class EmailSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
    }

    public class AppSettings
    {
        public EmailSettings? EmailSettings { get; set; }
    }

    public interface IEmailService
    {
        Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpo);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var settingsPath = Path.Combine(basePath, "appsettings.json");
            
            if (!File.Exists(settingsPath))
            {
                settingsPath = Path.Combine(basePath, "..", "..", "..", "appsettings.json");
            }

            if (File.Exists(settingsPath))
            {
                var json = File.ReadAllText(settingsPath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var settings = JsonSerializer.Deserialize<AppSettings>(json, options);
                
                _emailSettings = settings?.EmailSettings ?? new EmailSettings();
            }
            else
            {
                _emailSettings = new EmailSettings();
            }
        }

        public async Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpo)
        {
            if (string.IsNullOrEmpty(_emailSettings.Host) || string.IsNullOrEmpty(_emailSettings.Username))
            {
                throw new InvalidOperationException("La configuración del servidor de correo no está establecida en appsettings.json");
            }

            var fromAddress = new MailAddress(_emailSettings.Username, _emailSettings.FromName);
            var toAddress = new MailAddress(destinatario);

            using var smtp = new SmtpClient
            {
                Host = _emailSettings.Host,
                Port = _emailSettings.Port,
                EnableSsl = _emailSettings.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = asunto,
                Body = cuerpo,
                IsBodyHtml = false // El reporte actual es en texto plano
            };

            await smtp.SendMailAsync(message);
        }
    }
}
