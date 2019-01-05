using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MinecraftServerList
{
    public class MinecraftPingRequester
    {
        // Source: https://gist.github.com/csh/2480d14fbbb33b4bbae3

        private NetworkStream _stream;
        private List<byte> _buffer;
        private int _offset;
        private string _server;
        private ushort _port;

        public MinecraftPingRequester(string server, ushort port)
        {
            _server = server;
            _port = port;
        }

        public ServerPingResult GetServerPingData()
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    Task task = client.ConnectAsync(_server, _port);

                    while (!task.IsCompleted)
                        Thread.Sleep(250);

                    if (!client.Connected)
                        return new ServerPingResult("Unable to connect to the server.");

                    _buffer = new List<byte>();
                    _stream = client.GetStream();

                    SendHandshakePacket();
                    return JsonConvert.DeserializeObject<ServerPingResult>(SendStatusRequestPacket());
                }
            }
            catch (Exception ex)
            {
                return new ServerPingResult(ex.Message);
            }
        }

        private void SendHandshakePacket()
        {
            // http://wiki.vg/Server_List_Ping#Ping_Process

            WriteVarInt(47);
            WriteString(_server);
            WriteUShort(_port);
            WriteVarInt(1);
            Flush(0);
        }

        private string SendStatusRequestPacket()
        {
            Flush(0);

            byte[] buffer = new byte[short.MaxValue];
            _stream.Read(buffer, 0, buffer.Length);

            int length = ReadVarInt(buffer);
            int packet = ReadVarInt(buffer);
            int jsonLength = ReadVarInt(buffer);

            return ReadString(buffer, jsonLength);
        }

        private byte ReadByte(byte[] buffer)
        {
            byte b = buffer[_offset];
            _offset += 1;
            return b;
        }

        private byte[] Read(byte[] buffer, int length)
        {
            byte[] data = new byte[length];
            Array.Copy(buffer, _offset, data, 0, length);
            _offset += length;
            return data;
        }

        private int ReadVarInt(byte[] buffer)
        {
            int value = 0;
            int size = 0;
            int b;
            while (((b = ReadByte(buffer)) & 0x80) == 0x80)
            {
                value |= (b & 0x7F) << (size++ * 7);
                if (size > 5)
                    throw new IOException("Invalid VarInt!");
            }
            return value | ((b & 0x7F) << (size * 7));
        }

        private string ReadString(byte[] buffer, int length)
        {
            byte[] data = Read(buffer, length);
            return Encoding.UTF8.GetString(data);
        }

        private void WriteVarInt(int value)
        {
            while ((value & 128) != 0)
            {
                _buffer.Add((byte)(value & 127 | 128));
                value = (int)((uint)value) >> 7;
            }
            _buffer.Add((byte)value);
        }

        private void WriteUShort(ushort value)
        {
            _buffer.AddRange(BitConverter.GetBytes(value));
        }

        private void WriteString(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            WriteVarInt(buffer.Length);
            _buffer.AddRange(buffer);
        }

        private void Write(byte b)
        {
            _stream.WriteByte(b);
        }

        private void Flush(int id = -1)
        {
            byte[] buffer = _buffer.ToArray();
            _buffer.Clear();

            int add = 0;
            byte[] packetData = new[] { (byte)0x00 };
            if (id >= 0)
            {
                WriteVarInt(id);
                packetData = _buffer.ToArray();
                add = packetData.Length;
                _buffer.Clear();
            }

            WriteVarInt(buffer.Length + add);
            byte[] bufferLength = _buffer.ToArray();
            _buffer.Clear();

            _stream.Write(bufferLength, 0, bufferLength.Length);
            _stream.Write(packetData, 0, packetData.Length);
            _stream.Write(buffer, 0, buffer.Length);
        }
    }
}
