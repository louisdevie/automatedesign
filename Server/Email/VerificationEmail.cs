using MimeKit;
using MimeKit.Text;
using System.Net.Mail;

namespace AutomateDesign.Server.Model
{
    public class VerificationEmail
    {
        private MailAddress recipient;
        private string subject;
        private string body;

        public VerificationEmail(MailAddress recipient, string subject, string template, uint secretCode)
        {
            this.recipient = recipient;
            this.subject = subject;
            this.body = Template.Get(template).Render(secretCode);
        }

        internal MimeMessage ToMimeMessage(EmailSettings settings)
        {
            MimeMessage result = new MimeMessage();

            result.From.Add(new MailboxAddress(settings.DisplayName, settings.From));
            result.To.Add((MailboxAddress)this.recipient);

            result.Subject = this.subject;

            result.Body = new TextPart(TextFormat.Html) { Text = this.body };

            return result;
        }
    }
}
