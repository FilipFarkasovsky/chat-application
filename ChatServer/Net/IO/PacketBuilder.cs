using System.Text;

class PacketBuilder
{
    MemoryStream memoryStream;
    public PacketBuilder() { memoryStream = new MemoryStream(); }
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