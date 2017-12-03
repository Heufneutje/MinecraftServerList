namespace MinecraftServerList
{
    public class Server
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public ushort Port { get; set; }

        public Server()
        {
            Port = 25565;
        }

        public Server(string description, string address, ushort port)
        {
            Description = description;
            Address = address;
            Port = port;
        }
    }
}
