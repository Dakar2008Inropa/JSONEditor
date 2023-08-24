using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONEditor.Classes.RecentFiles
{
    public static class RecentFilesHelper
    {
        private static string RecentFilesPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Inropa", "JSONEditor");
        private static string RecentFilesFilePath { get; } = $@"RecentFiles.json";
        private static string RecentFilesFile { get; } = Path.Combine(RecentFilesPath, RecentFilesFilePath);

        public static void Save(List<RecentFiles> recentFiles)
        {
            try
            {
                Directory.CreateDirectory(RecentFilesPath);
                JsonHelper.SerializeObject(recentFiles, RecentFilesFile);
                Logger.Log.Info("RecentFiles Saved");
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message);
                throw;
            }
        }

        public static List<RecentFiles> Load()
        {
            try
            {
                if (File.Exists(RecentFilesFile))
                {
                    return JsonHelper.DeserializeObject<List<RecentFiles>>(RecentFilesFile);
                }
                else
                {
                    return new List<RecentFiles>();
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error(ex.Message);

                return new List<RecentFiles>();
            }
        }
    }
}