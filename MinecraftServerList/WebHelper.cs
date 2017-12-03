using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;

namespace MinecraftServerList
{
    public static class WebHelper
    {
        public static GameApiResult GetGameApiResult(Server server)
        {
            Uri uri = new Uri($"https://use.gameapis.net/mc/query/info/{server.Address}:{server.Port}");

            WebRequest webRequest = WebRequest.Create(uri);
            string json;
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                    json = reader.ReadToEnd();

                return JsonConvert.DeserializeObject<GameApiResult>(json);
            }
        }
    }
}
