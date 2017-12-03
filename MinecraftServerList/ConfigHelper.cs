using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace MinecraftServerList
{
    public static class ConfigHelper
    {
        private const string _path = "servers.json";

        public static List<Server> LoadConfig()
        {
            if (File.Exists(_path))
                return JsonConvert.DeserializeObject<List<Server>>(File.ReadAllText(_path));

            return new List<Server>();
        }

        public static void SaveConfig(List<Server> servers)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(servers));
        }
    }
}
