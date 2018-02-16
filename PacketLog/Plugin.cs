using System;
using System.Collections.Generic;
using System.Linq;
using Lib_K_Relay;
using Lib_K_Relay.Utilities;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;

namespace PacketLog
{
    public class Plugin : IPlugin
    {
        public string GetAuthor() => "natiiix";

        public string[] GetCommands() => new string[0];

        public string GetDescription() => string.Empty;

        public string GetName() => "Packet Log";

        public void Initialize(Proxy proxy)
        {
            proxy.ClientPacketRecieved += Proxy_ClientPacketRecieved;
            proxy.ServerPacketRecieved += Proxy_ServerPacketRecieved;
        }

        private void Proxy_ClientPacketRecieved(Client client, Packet packet)
        {
            Proxy_PacketRecieved(client, packet, "Client");
        }

        private void Proxy_ServerPacketRecieved(Client client, Packet packet)
        {
            Proxy_PacketRecieved(client, packet, "Server");
        }

        private void Proxy_PacketRecieved(Client client, Packet packet, string source)
        {
            if (new PacketType[]
            {
                //PacketType.PING,
                //PacketType.PONG,
                PacketType.NEWTICK,
                PacketType.MOVE,
                //PacketType.TEXT,
                //PacketType.UPDATE,
                //PacketType.UPDATEACK,
                //PacketType.SHOWEFFECT
            }.Contains(packet.Type))
            {
                return;
            }

            PluginUtils.Log(
                source + " Packet",
                packet.Type.ToString() + "  |  " + DateTime.Now.ToString("HH:mm:ss.fff") + Environment.NewLine +
                packet.GetFields() + Environment.NewLine);
        }
    }

    public static class ExtensionMethods
    {
        public static string GetFields(this Packet packet)
        {
            List<string> data = new List<string>();

            foreach (var f in packet.GetType().GetFields().Where(x => x.Name != "Send" && x.Name != "Id"))
            {
                data.Add(f.Name + ": " + f.GetValue(packet));
            }

            return string.Join(Environment.NewLine, data);
        }
    }
}
