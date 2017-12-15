using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoginServer.Configs
{
    class Server
    {
        public static int MaxSessions = 1000;
        public static int MaxInventorySlot = 50;
        public static int MaxCostumeSlot = 50;
        public static byte[] incomingBuffer = (new Packets.SP_ReceiveConnection()).getBytes();
    }
}
