using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net
{
    internal class PacketReader : BinaryReader
    {
        private NetworkStream networkStream;
        public PacketReader(NetworkStream networkStream) : base(networkStream)
        {
            this.networkStream = networkStream;
        }

        public string ReadMessage()
        {
            byte[] msgBuffer;
            int length = ReadInt32();
            msgBuffer = new byte[length];
            networkStream.Read(msgBuffer, 0, length);

            string msg = Encoding.ASCII.GetString(msgBuffer);
            return msg;
        }
    }
}
