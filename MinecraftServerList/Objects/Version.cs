using Newtonsoft.Json;

namespace MinecraftServerList
{
    public class Version
    {
        [JsonProperty(PropertyName = "protocol")]
        public int Protocol { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
