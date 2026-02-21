using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Utils.Logger;
using System.Text.Json;

namespace RecoilReworkServer.Helpers
{
    [Injectable]
    public class LoadHelper(SptLogger<LoadHelper> logger, ModHelper modHelper)
    {
        public List<T> LoadAllFromDirectory<T>(string path) where T : class
        {
            List<T> list = [];
            string[] directories = Directory.GetDirectories(path);
            string[] files = Directory.GetFiles(path);
            
            logger.LogWithColor($"Loading {files.Length} files from directory {new DirectoryInfo(path).Name} with type {typeof(T).Name}");

            foreach (string directory in directories)
            {
                List<T> recursiveList = LoadAllFromDirectory<T>(directory);
                list.AddRange(recursiveList);
            }

            foreach (string filePath in files)
            {
                string? fullPath = Path.GetDirectoryName(filePath);
                string fileName = Path.GetFileName(filePath);

                if (fullPath != null)
                {
                    T item = modHelper.GetJsonDataFromFile<T>(fullPath, fileName);

                    list.Add(item);
                }
            }

            return list;
        }
    }
}
