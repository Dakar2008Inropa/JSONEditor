using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONEditor.Classes.RecentFolders
{
    public static class RecentFoldersHelper
    {
        private static string RecentFoldersPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Inropa", "JSONEditor");
        private static string RecentFoldersFilePath { get; } = $@"RecentFolders.json";
        private static string RecentFoldersFile { get; } = Path.Combine(RecentFoldersPath, RecentFoldersFilePath);

        public static void Save(List<RecentFolders> recentFolders)
        {
            try
            {
                Directory.CreateDirectory(RecentFoldersPath);
                JsonHelper.SerializeObject(recentFolders, RecentFoldersFile);
                Log4net.Log.Info("RecentFolders Saved");
            }
            catch (Exception ex)
            {
                Log4net.Log.Error(ex.Message);
                throw;
            }
        }

        public static List<RecentFolders> Load()
        {
            try
            {
                if (File.Exists(RecentFoldersFile))
                {
                    return JsonHelper.DeserializeObject<List<RecentFolders>>(RecentFoldersFile);
                }
                else
                {
                    return new List<RecentFolders>();
                }
            }
            catch (Exception ex)
            {
                Log4net.Log.Error(ex.Message);

                return new List<RecentFolders>();
            }
        }
    }
}