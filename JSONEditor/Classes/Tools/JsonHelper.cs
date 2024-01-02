using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace JSONEditor.Classes.Tools
{
    public static class JsonHelper
    {
        public static void SerializeObject<T>(T objectToSerialize, string fileName)
        {
            string json = JsonConvert.SerializeObject(objectToSerialize, Formatting.Indented);
            File.WriteAllText(fileName, json);
        }

        public static T DeserializeObject<T>(string fileName)
        {
            string json = File.ReadAllText(fileName);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string GetGlobalMutexName(this string filePath)
        {
            string globalMutexName = filePath.GetHashCode().ToString();
            return globalMutexName;
        }

        public static JToken LoadJsonData(string FilePath)
        {
            JToken root = null;
            try
            {
                if (File.Exists(FilePath))
                {
                    using (Mutex mutex = new Mutex(false, FilePath.GetGlobalMutexName()))
                    {
                        try
                        {
                            mutex.WaitOne();
                            using (var reader = new StreamReader(FilePath))
                            using (var jsonReader = new JsonTextReader(reader))
                            {
                                root = JToken.Load(jsonReader);
                            }
                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log4net.Log.Error(ex.Message);
                Log4net.Log.Error($"Following File Caused The Error: {Path.GetFileName(FilePath)}");
            }
            return root;
        }

        public static void WriteToJsonFile(JToken root, string path, bool isClassification = false)
        {
            string data = JsonConvert.SerializeObject(root, Formatting.Indented);
            using (Mutex mutex = new Mutex(false, path.GetGlobalMutexName()))
            {
                try
                {
                    mutex.WaitOne();
                    File.WriteAllText(path, data);
                }
                catch (Exception ex)
                {
                    Log4net.Log.Error(ex.Message);
                    throw;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }
    }
}