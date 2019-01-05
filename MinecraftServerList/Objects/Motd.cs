using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace MinecraftServerList
{
    public class Motd
    {
        [JsonProperty(PropertyName = "text")]
        public string RawText { get; set; }

        private string _plainText;

        public string PlainText
        {
            get
            {
                if (_plainText == null)
                    _plainText = Regex.Replace(RawText, "§.{1}", string.Empty);

                return _plainText;
            }
        }
    }
}
