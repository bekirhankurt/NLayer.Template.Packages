#region Usings
using Mailing.Abstract;
using Mailing.Concrete;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
#endregion

#region Namespace

namespace Mailing.MailKitImplementation
{
    #region Class: MailKitService
    /// <summary>
    /// Implements the <see cref="IMailService"/> interface to send emails via MailKit.
    /// </summary>
    public class MailKitService : IMailService
    {
        #region Fields

        private readonly MailSettings _mailSettings;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MailKitService"/> class.
        /// Retrieves mail settings from the configuration.
        /// </summary>
        /// <param name="configuration">The application configuration containing mail settings.</param>
        public MailKitService(IConfiguration configuration)
        {
            _mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sends an email using the MailKit SMTP client.
        /// </summary>
        /// <param name="mail">A <see cref="Mail"/> object containing the email details such as subject, body, and recipient information.</param>
        public void SendMail(Mail mail)
        {
            // Create a new MIME message.
            MimeMessage email = new();

            // Set the sender and recipient.
            email.From.Add(new MailboxAddress(_mailSettings.SenderFullName, _mailSettings.SenderEmail));
            email.To.Add(new MailboxAddress(mail.ToName, mail.ToMail));
            email.Subject = mail.Subject;

            // Build the email body with text and HTML versions.
            BodyBuilder bodyBuilder = new()
            {
                TextBody = mail.TextBody,
                HtmlBody = mail.HtmlBody
            };

            // Attach any files if attachments are provided.
            if (mail.Attachments is not null)
            {
                foreach (var attachment in mail.Attachments)
                {
                    bodyBuilder.Attachments.Add(attachment);
                }
            }

            // Set the email body.
            email.Body = bodyBuilder.ToMessageBody();

            // Send the email using the SMTP client.
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Server, _mailSettings.Port);
            smtp.Authenticate(_mailSettings.Username, _mailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        #endregion
    }
    #endregion
}
#endregion
