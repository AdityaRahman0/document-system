using System.Net;
using System.Net.Mail;

namespace Services
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _appPassword = "veqh hjwe hsbb yjgm";
        private readonly string _myEmail = "rahmanaditya868@gmail.com";
        private readonly string _subject = "Document Management System";
        private readonly string _host = "smtp.gmail.com";

        public EmailService()
        {
            _smtpClient = new SmtpClient
            {
                Host = _host,
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(_myEmail, _appPassword)
            };
        }

        public void SendApprovalEmail(string toEmail, string subject, string messageBody)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_myEmail, _subject),
                Subject = subject,
                Body = messageBody,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            _smtpClient.Send(mailMessage);
        }

        public string BodyEmailApproval(string documentNo)
        {
            return "<p>A document " + documentNo + " has been submitted for your approval.</p><p>Please review it as soon as possible.</p>";
        }
    }
}
