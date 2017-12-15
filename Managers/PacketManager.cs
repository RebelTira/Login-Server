using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LoginServer.Packets;

namespace LoginServer.Managers
{
    /// <summary>
    /// This class handles and contains all used packets
    /// </summary>
    class Packet_Manager
    {
        private static Dictionary<int, Handler> packets = new Dictionary<int, Handler>();

        public static void setup()
        {
            addPacket(4112, new CP_PatchInformationHandler());
            addPacket(4352, new CP_LoginHandler());
            addPacket(4353, new CP_NewUserHandler());
            addPacket(4609, new CP_ServerRefresh());

            // Custom Packets
            addPacket(4252, new CP_PatchInformationUpdateHandler());

            Log.WriteLine("Loaded " + packets.Count + " packet handlers");
        }

        public static Handler parsePacket(string packetStr)
        {
            try
            {
                string[] packetBlocks = packetStr.Split(' ');
                long timeGetTime = 0;
                long.TryParse(packetBlocks[0], out timeGetTime);
                int packetId = int.Parse(packetBlocks[1]);

                if (packets.ContainsKey(packetId))
                {
                    string[] resizedBlocks = new string[packetBlocks.Length - 2];
                    Array.Copy(packetBlocks, 2, resizedBlocks, 0, packetBlocks.Length - 2);
                    Handler handler = (Handler)packets[packetId];
                    handler.FillData(timeGetTime, packetId, resizedBlocks);
                    return handler;
                }
            }
            catch
            {
            }
            return null;
        }

        private static void addPacket(int id, Handler handler)
        {
            if (!packets.ContainsKey(id))
            {
                packets.Add(id, handler);
            }
            else
            {
                Log.WriteError("Packet Manager already contains packetID: " + id);
            }
        }
    }
}
