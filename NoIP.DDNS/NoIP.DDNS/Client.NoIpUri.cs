namespace NoIP.DDNS
{
    public partial class Client
    {
        private const string HOST_ACTIONS_DOM_LIST_URL_SECURE = "https://dynupdate.noip.com/ext/host-actions.php?action=domlist&email={0}";
        private const string HOST_ACTIONS_HOST_ADD_URL_SECURE = "https://dynupdate.noip.com/ext/host-actions.php?action=hostadd&host={0}&domain={1}&email={2}";
        private const string HOST_ACTIONS_HOST_DELETE_URL_SECURE = "https://dynupdate.noip.com/ext/host-actions.php?action=hostdelete&host={0}&domain={1}&email={2}";
        private const string NOTICES_URL_SECURE = "https://dynupdate.noip.com/notices.php?email={0}&lastnotice={1}&lastalert={2}";
        private const string REGISTER_URL_SECURE = "https://dynupdate.noip.com/client/register.php?email={0}&pass={1}";
        private const string SETTINGS_URL_SECURE = "https://dynupdate.noip.com/settings.php?username={0}";
        private const string UPDATE_URL_SECURE = "https://dynupdate.noip.com/ducupdate.php?username={0}&{1}&ip={2}";
    }
}
