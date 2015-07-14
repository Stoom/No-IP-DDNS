using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NoIP.DDNS.Exceptions
{
    /// <summary>
    /// Exception thrown when a NoIP host update has failed.
    /// </summary>
    /// <remarks>
    /// Contains a custom property, thus it Implements ISerializable 
    /// and the special serialization constructor.
    /// </remarks>
    [Serializable]
    public class UpdateException : NoIpException, ISerializable
    {
        private readonly IDictionary<string, UpdateStatus> _hostsStatus;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateException"/> class.
        /// </summary>
        public UpdateException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UpdateException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="hostsStatus">The hostsStatus.</param>
        public UpdateException(string message, IDictionary<string, UpdateStatus> hostsStatus) : base(message)
        {
            _hostsStatus = hostsStatus;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="hostsStatus">The hostsStatus.</param>
        /// <param name="innerException">The inner exception.</param>
        public UpdateException(string message, IDictionary<string, UpdateStatus> hostsStatus, Exception innerException) : base(message, innerException)
        {
            _hostsStatus = hostsStatus;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception></exception>
        protected UpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _hostsStatus = info.GetValue("HostStatus", typeof (IDictionary<string, UpdateStatus>)) as Dictionary<string, UpdateStatus>;
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/>
        /// with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception></exception>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("HostStatus", _hostsStatus);
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Gets the HostStatus.
        /// </summary>
        public IDictionary<string, UpdateStatus> HostStatus
        {
            get { return _hostsStatus; }
        }
    }

}