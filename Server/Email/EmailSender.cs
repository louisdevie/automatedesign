using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AutomateDesign.Server.Model
{
    public class EmailSender
    {
        #region Attributs
        private readonly EmailSettings settings;
        #endregion

        public EmailSender(IOptions<EmailSettings> settings)
        {
            this.settings = settings.Value;
        }

        public async Task<bool> SendAsync(VerificationEmail email, CancellationToken ct = default)
        {
            try
            {
                var message = email.ToMimeMessage(this.settings);

                SecureSocketOptions options = SecureSocketOptions.Auto;
                if (settings.UseSSL)
                {
                    options = SecureSocketOptions.SslOnConnect;
                }
                else if (settings.UseStartTls)
                {
                    options = SecureSocketOptions.StartTls;
                }

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(settings.Host, settings.Port, options, ct);
                await smtp.AuthenticateAsync(settings.UserName, settings.Password, ct);

                await smtp.SendAsync(message, ct);

                await smtp.DisconnectAsync(true, ct);

                return true;

            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }

        public bool Send(VerificationEmail email, CancellationToken ct = default)
        {
            try
            {
                Console.WriteLine("Preparing email...");

                var message = email.ToMimeMessage(this.settings);

                SecureSocketOptions options = SecureSocketOptions.Auto;
                if (settings.UseSSL)
                {
                    options = SecureSocketOptions.SslOnConnect;
                }
                else if (settings.UseStartTls)
                {
                    options = SecureSocketOptions.StartTls;
                }

                using var smtp = new SmtpClient();

                Console.WriteLine("Sending email...");

                smtp.Connect(settings.Host, settings.Port, options, ct);
                smtp.Authenticate(settings.UserName, settings.Password, ct);

                smtp.Send(message, ct);

                smtp.Disconnect(true, ct);

                return true;

            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return false;
            }
        }
    }
}
