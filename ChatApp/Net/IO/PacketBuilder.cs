using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net
{
    class PacketBuilder
    {
        MemoryStream memoryStream;
        public PacketBuilder() {  memoryStream = new MemoryStream(); } 
        public void WriteOpCode(byte opcode)
        {
            memoryStream.WriteByte(opcode);
        }
        public void WriteMessage(string msg)
        {
            int msgLength = msg.Length;
            memoryStream.Write(BitConverter.GetBytes(msgLength));
            memoryStream.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketBytes()
        {
            return memoryStream.ToArray();
        }
    }
}
