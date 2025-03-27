#region Usings
using Mailing.Concrete;
#endregion

#region Namespace

namespace Mailing.Abstract
{
    #region Interface: IMailService
    /// <summary>
    /// Defines operations for sending email messages.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Sends the specified email message.
        /// </summary>
        /// <param name="mail">A <see cref="Mail"/> object containing the email details.</param>
        void SendMail(Mail mail);
    }
    #endregion
}
#endregion