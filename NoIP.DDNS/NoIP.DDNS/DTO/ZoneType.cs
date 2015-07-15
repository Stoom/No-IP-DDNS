namespace NoIP.DDNS.DTO
{
    public enum ZoneType
    {
        /// <summary>
        /// Free No-IP domain.
        /// </summary>
        Free,
        Enterprise,
        /// <summary>
        /// Paid domain managed through No-IP.
        /// </summary>
        Plus,
        Static,

        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Used for parsing.
        /// </summary>
        free = Free,
        /// <summary>
        /// Used for parsing.
        /// </summary>
        enterprise = Enterprise,
        /// <summary>
        /// Used for parsing.
        /// </summary>
        plus = Plus,
        /// <summary>
        /// Used for parsing.
        /// </summary>
        @static = Static
        // ReSharper restore InconsistentNaming
    }
}
