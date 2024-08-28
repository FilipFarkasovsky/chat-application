using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks.Dataflow;

namespace ChatServer
{
    class Program
    {
        static List<Client> users;
        static TcpListener listener;
        static void Main(string[] args)
        {
            users = new List<Client>();
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            listener.Start();

            while (true)
            {
                Client client = new Client(listener.AcceptTcpClient());
                users.Add(client);

                /* Broadcast the connection to everyoneon on the server */
                BroacastConnection();
            }
        }
        static void BroacastConnection()
        {
            foreach (Client client in users)
            {
                foreach (Client clt in users)
                {
                    PacketBuilder packetBuilder = new PacketBuilder();
                    packetBuilder.WriteOpCode(1);
                    packetBuilder.WriteMessage(clt.Username);
                    packetBuilder.WriteMessage(clt.UID.ToString());
                    client.ClientSocket.Client.Send(packetBuilder.GetPacketBytes());
                }
            }
        }

        public static void BroadcastMessage(string message)
        {
            foreach (Client client in users)
            {
                var msgPacket = new PacketBuilder();
                msgPacket.WriteOpCode(5);
                msgPacket.WriteMessage(message);
                client.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            }
        }

        public static void BroadcastDisconnect(string uid)
        {
            var disconnectedUser = users.Where( x => x.UID.ToString() == uid ).FirstOrDefault();
            users.Remove(disconnectedUser);
            foreach (Client client in users)
            {
                var broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10);
                broadcastPacket.WriteMessage(uid);
                client.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }

            BroadcastMessage($"[{disconnectedUser.Username}] Disconnected!");
        }
    }
}