namespace NoIP.DDNS
{
    public enum UpdateStatus
    {
        IpCurrent,
        Success,
        HostNameDoesNotExist,
        InvalidPassword,
        InvalidUserName,
        TooManyUpdates,
        AccountDisabled,
        InvalidIp,
        Disabled,
        HostRedirectUpdated,
        GroupSuppliedDoesNotExist,
        GroupUpdateSuccess,
        GroupIsCurrent,
        UpdateClientNotSupported,
        HostNameOfflineNotConfigured,
        ClientIdTemporarilyDisabled = 98,
        ClientDisabled = 99,
        InputError = 100,
        UnableToReadIpAddress = 50000,
    }
}
