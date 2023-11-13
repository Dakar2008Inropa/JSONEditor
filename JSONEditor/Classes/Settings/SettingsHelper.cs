namespace JSONEditor.Classes.Settings
{
    public static class SettingsHelper
    {
        private static string SettingsPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Inropa", "JSONEditor");
        private static string SettingsFilePath { get; } = $@"Settings.json";
        private static string SettingsFile { get; } = Path.Combine(SettingsPath, SettingsFilePath);

        public static void Save(Settings settings)
        {
            try
            {
                Directory.CreateDirectory(SettingsPath);
                JsonHelper.SerializeObject(settings, SettingsFile);
                Log4net.Log.Info("Settings Saved");
            }
            catch (Exception ex)
            {
                Log4net.Log.Error(ex.Message);
                throw;
            }
        }
        public static Settings Load()
        {
            try
            {
                if (File.Exists(SettingsFile))
                {
                    return JsonHelper.DeserializeObject<Settings>(SettingsFile);
                }
                else
                {
                    Settings appsettings = new Settings();
                    appsettings.WindowPosition = new WindowPosition();
                    appsettings.WindowSize = new WindowSize();

                    return appsettings;
                }
            }
            catch (Exception ex)
            {
                Log4net.Log.Error(ex.Message);

                Settings appsettings = new Settings();
                appsettings.WindowPosition = new WindowPosition();
                appsettings.WindowSize = new WindowSize();

                return appsettings;
            }
        }
    }
}