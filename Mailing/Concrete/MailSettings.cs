#region Namespace

namespace Mailing.Concrete
{
    #region Class: MailSettings
    /// <summary>
    /// Represents configuration settings for connecting to a mail server and specifying sender details.
    /// </summary>
    public class MailSettings
    {
        #region Properties

        /// <summary>
        /// Gets or sets the mail server address.
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the port number used to connect to the mail server.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the full name of the sender.
        /// </summary>
        public string SenderFullName { get; set; }

        /// <summary>
        /// Gets or sets the email address of the sender.
        /// </summary>
        public string SenderEmail { get; set; }

        /// <summary>
        /// Gets or sets the username for mail server authentication.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for mail server authentication.
        /// </summary>
        public string Password { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MailSettings"/> class.
        /// </summary>
        public MailSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MailSettings"/> class with the specified parameters.
        /// </summary>
        /// <param name="server">The mail server address.</param>
        /// <param name="port">The port number of the mail server.</param>
        /// <param name="senderFullName">The full name of the sender.</param>
        /// <param name="senderEmail">The email address of the sender.</param>
        /// <param name="userName">The username for authentication.</param>
        /// <param name="password">The password for authentication.</param>
        public MailSettings(string server, int port, string senderFullName, string senderEmail, string userName, string password)
        {
            Server = server;
            Port = port;
            SenderFullName = senderFullName;
            SenderEmail = senderEmail;
            Username = userName;
            Password = password;
        }

        #endregion
    }
    #endregion
}
#endregion
