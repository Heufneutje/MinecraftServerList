using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace MinecraftServerList
{
    public class ServerPingResult
    {
        [JsonProperty(PropertyName = "description")]
        public Motd Motd { get; set; }

        [JsonProperty(PropertyName = "players")]
        public PlayerSample PlayerSample { get; set; }

        [JsonProperty(PropertyName = "version")]
        public Version Version { get; set; }

        [JsonProperty(PropertyName = "favicon")]
        public string Favicon { get; set; }

        public bool Succesful { get; set; }
        public string ErrorMessage { get; set; }

        public BitmapImage FaviconImage
        {
            get
            {
                if (Favicon == null)
                    return null;

                return ImageHelper.GetBitmapImageFromBase64(Favicon);
            }
        }

        public ServerPingResult()
        {
            Succesful = true;
        }

        public ServerPingResult(string error)
        {
            Succesful = false;
            ErrorMessage = error;
        }
    }
}
