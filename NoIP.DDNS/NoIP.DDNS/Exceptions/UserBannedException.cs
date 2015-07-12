using System;

namespace NoIP.DDNS.Exceptions
{
    /// <summary>
    /// Exception thrown when a No-IP user has been banned.
    /// </summary>
    /// <remarks>
    /// This exception does not have any custom properties, 
    /// thus it does not implement ISerializable.
    /// </remarks>
    [Serializable]
    public class UserBannedException : NoIpException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserBannedException"/> class.
        /// </summary>
        public UserBannedException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBannedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UserBannedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBannedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UserBannedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}