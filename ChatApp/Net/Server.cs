using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net
{
    internal class Server
    {
        TcpClient client;
        public PacketReader packetReader;

        public event Action connectedEvent;
        public event Action msgReceivedEvent;
        public event Action userDisconnectedEvent;
        public Server()
        {
            client = new TcpClient();
        }

        public void ConnectToServer(string username)
        {
            Console.WriteLine("Connect To Server");
            if (!client.Connected) 
            {
                client.Connect("127.0.0.1", 7891);
                packetReader = new PacketReader(client.GetStream());

                if (!string.IsNullOrEmpty(username))
                {
                    PacketBuilder packetBuilder = new PacketBuilder();
                    packetBuilder.WriteOpCode(0);
                    packetBuilder.WriteMessage(username);
                    client.Client.Send(packetBuilder.GetPacketBytes());
                }
                ReadPackets();
            }
        }

        private void ReadPackets()
        {
            Task.Run(() => 
            {
                while (true)
                {
                    int opcode = packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 1:
                            connectedEvent?.Invoke();
                            break;
                        case 5:
                            msgReceivedEvent?.Invoke();
                            break;
                        case 10:
                            userDisconnectedEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine("ah..yes");
                            break;
                    }
                }
            });
        }

        public void SendMesssageToServer(string message)
        {
            var messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5);
            messagePacket.WriteMessage(message);
            client.Client.Send(messagePacket.GetPacketBytes());
        }
    }
}
