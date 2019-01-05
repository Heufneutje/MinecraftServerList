using System.Collections.Generic;
using Newtonsoft.Json;

namespace MinecraftServerList
{
    public class PlayerSample
    {
        [JsonProperty(PropertyName = "max")]
        public int Max { get; set; }

        [JsonProperty(PropertyName = "online")]
        public int Online { get; set; }

        [JsonProperty(PropertyName = "sample")]
        public List<Player> PlayerCollection { get; set; }
    }
}
