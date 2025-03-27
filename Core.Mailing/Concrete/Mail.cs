#region Usings
using MimeKit;
#endregion

#region Namespace
namespace Mailing.Concrete
{
    #region Class: Mail
    /// <summary>
    /// Represents an email message with subject, body, attachments, and recipient details.
    /// </summary>
    public class Mail
    {
        #region Properties

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the plain text body of the email.
        /// </summary>
        public string TextBody { get; set; }

        /// <summary>
        /// Gets or sets the HTML body of the email.
        /// </summary>
        public string HtmlBody { get; set; }

        /// <summary>
        /// Gets or sets the collection of attachments for the email.
        /// </summary>
        public AttachmentCollection? Attachments { get; set; }

        /// <summary>
        /// Gets or sets the full name of the email recipient.
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the recipient.
        /// </summary>
        public string ToMail { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Mail"/> class.
        /// </summary>
        public Mail()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mail"/> class with specified subject, body, attachments, and recipient details.
        /// </summary>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="textBody">The plain text content of the email.</param>
        /// <param name="htmlBody">The HTML content of the email.</param>
        /// <param name="attachments">The collection of attachments to include in the email.</param>
        /// <param name="toFullName">The full name of the recipient.</param>
        /// <param name="toEmail">The email address of the recipient.</param>
        public Mail(string subject, string textBody, string htmlBody, AttachmentCollection? attachments, string toFullName, string toEmail)
        {
            Subject = subject;
            TextBody = textBody;
            HtmlBody = htmlBody;
            Attachments = attachments;
            ToName = toFullName;
            ToMail = toEmail;
        }

        #endregion
    }
    #endregion
}
#endregion
