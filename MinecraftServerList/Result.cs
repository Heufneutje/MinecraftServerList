using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace MinecraftServerList
{
    public class Players
    {
        public int online { get; set; }
        public int max { get; set; }
    }

    public class Motds
    {
        public string ingame { get; set; }
        public string html { get; set; }
        public string clean { get; set; }
    }

    public class Hover
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class GameApiResult
    {
        public bool status { get; set; }
        public string hostname { get; set; }
        public int port { get; set; }
        public int ping { get; set; }
        public string version { get; set; }
        public string protocol { get; set; }
        public Players players { get; set; }
        public Motds motds { get; set; }
        public string favicon { get; set; }
        public IList<Hover> hover { get; set; }
        public bool cached { get; set; }
        public string error { get; set; }

        public BitmapImage FaviconImage
        {
            get
            {
                if (favicon == null)
                    return null;

                return ImageHelper.GetBitmapImageFromBase64(favicon);
            }
        }
    }
}
