namespace NoIP.DDNS
{
    /// <summary>
    /// List of host(s) update statuses.
    /// </summary>
    public enum UpdateStatus
    {
        /// <summary>
        /// Host(s) were already set to specified IP address.
        /// </summary>
        IpCurrent,
        /// <summary>
        /// Host(s) have been successfully updated to the new IP address.
        /// </summary>
        Success,
        /// <summary>
        /// A host(s) were not found.
        /// </summary>
        HostNameDoesNotExist,
        /// <summary>
        /// Client Key was incorrect.
        /// </summary>
        InvalidPassword, //Invalid Login
        /// <summary>
        /// Client ID was incorrect.
        /// </summary>
        InvalidUserName, //Invalid Login
        /// <summary>
        /// The host has been updated too many times for a given time period.
        /// </summary>
        TooManyUpdates,
        /// <summary>
        /// Account has been disabled.
        /// </summary>
        AccountDisabled, //Authentication
        /// <summary>
        /// An IP address(es) were invalid
        /// </summary>
        InvalidIp,
        /// <summary>
        /// Disabled.
        /// </summary>
        Disabled,
        /// <summary>
        /// The host redirect was updated.
        /// </summary>
        HostRedirectUpdated,
        /// <summary>
        /// The group(s) do not exist.
        /// </summary>
        GroupSuppliedDoesNotExist,
        /// <summary>
        ///  The group(s) were successfully updated.
        /// </summary>
        GroupUpdateSuccess,
        /// <summary>
        /// The group(s) address was already set to specified IP address.
        /// </summary>
        GroupIsCurrent,
        /// <summary>
        /// Update client is not supported.
        /// </summary>
        UpdateClientNotSupported,
        /// <summary>
        /// Host name offline is not configured.
        /// </summary>
        HostNameOfflineNotConfigured,
        /// <summary>
        /// The client ID has been temporarily disabled.
        /// </summary>
        ClientIdTemporarilyDisabled = 98, //Authentication
        /// <summary>
        /// The client ID has been permanently disabled.
        /// </summary>
        ClientDisabled = 99, //Authentication
        /// <summary>
        /// General input error.
        /// </summary>
        InputError = 100,
        /// <summary>
        /// General IP address error.
        /// </summary>
        UnableToReadIpAddress = 50000,
    }
}
