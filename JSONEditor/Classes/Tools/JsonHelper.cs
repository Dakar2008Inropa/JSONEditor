using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Logging;

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

        public static JToken LoadJsonData(string Path)
        {
            JToken root = null;
            if (File.Exists(Path))
            {
                using (Mutex mutex = new Mutex(false, Path.GetGlobalMutexName()))
                {
                    try
                    {
                        mutex.WaitOne();
                        using (var reader = new StreamReader(Path))
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
                    Logger.Log.Error(ex.Message);
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