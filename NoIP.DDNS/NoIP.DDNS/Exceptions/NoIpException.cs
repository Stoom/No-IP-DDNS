using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NoIP.DDNS.Exceptions
{
    /// <summary>
    /// Base exception for any No-IP exception.
    /// </summary>
    /// <remarks>
    /// This exception does not have any custom properties, 
    /// thus it does not implement ISerializable.
    /// </remarks>
    [Serializable]
    public class NoIpException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoIpException"/> class.
        /// </summary>
        public NoIpException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoIpException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public NoIpException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoIpException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public NoIpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception></exception>
        protected NoIpException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}