namespace Lockec.Extras
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        private static ISettings AppSettings
        {
            get { return CrossSettings.Current; }
        }

        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion


        public static string GeneralSettings
        {
            get { return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(SettingsKey, value); }
        }

        public static bool IsLoggedIn
        {
            get { return AppSettings.GetValueOrDefault(Constants.IS_LOGGED_IN, false); }
            set { AppSettings.AddOrUpdateValue(Constants.IS_LOGGED_IN, value); }
        }

        public static string AccountSettings
        {
            get { return AppSettings.GetValueOrDefault(Constants.ACCOUNT_KEY, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(Constants.ACCOUNT_KEY, value); }
        }

        public static string PersonalAccountSettings
        {
            get { return AppSettings.GetValueOrDefault(Constants.PERSONAL_KEY, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(Constants.PERSONAL_KEY, value); }
        }

        public static string EnterpriseAccountSettings
        {
            get { return AppSettings.GetValueOrDefault(Constants.ENTERPRISE_KEY, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(Constants.ENTERPRISE_KEY, value); }
        }

        public static string PhoneSettings
        {
            get { return AppSettings.GetValueOrDefault(Constants.PHONE_KEY, SettingsDefault); }
            set { AppSettings.AddOrUpdateValue(Constants.PHONE_KEY, value); }
        }

        public static void ClearAllData()
        {
            AppSettings.Clear();
        }

    }
}