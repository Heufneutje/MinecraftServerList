namespace MinecraftServerList
{
    public class Server
    {
        public string Address { get; set; }
        public ushort Port { get; set; }

        public string ListFormat
        {
            get
            {
                return $"{Address}:{Port}";
            }
        }

        public Server()
        {
            Port = 25565;
        }

        public Server(string address, ushort port)
        {
            Address = address;
            Port = port;
        }
    }
}
