namespace NoIP.DDNS
{
    public enum UpdateStatus
    {
        IpCurrent,
        Success,
        HostNameDoesNotExist,
        InvalidPassword, //Invalid Login
        InvalidUserName, //Invalid Login
        TooManyUpdates,
        AccountDisabled, //Authentication
        InvalidIp,
        Disabled,
        HostRedirectUpdated,
        GroupSuppliedDoesNotExist,
        GroupUpdateSuccess,
        GroupIsCurrent,
        UpdateClientNotSupported,
        HostNameOfflineNotConfigured,
        ClientIdTemporarilyDisabled = 98, //Authentication
        ClientDisabled = 99, //Authentication
        InputError = 100,
        UnableToReadIpAddress = 50000,
    }
}
