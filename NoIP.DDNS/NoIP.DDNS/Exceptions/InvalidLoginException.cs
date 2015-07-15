using System;

namespace NoIP.DDNS.Exceptions
{
    /// <summary>
    /// Exception thrown when an invalid username or password was entered.
    /// </summary>
    /// <remarks>
    /// This exception does not have any custom properties, 
    /// thus it does not implement ISerializable.
    /// </remarks>
    [Serializable]
    public class InvalidLoginException : AuthenticationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLoginException"/> class.
        /// </summary>
        public InvalidLoginException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLoginException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidLoginException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLoginException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidLoginException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}