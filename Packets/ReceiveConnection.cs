using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoginServer.Packets
{
    class SP_ReceiveConnection : Packet
    {
        public SP_ReceiveConnection()
        {
            newPacket(4608);
            addBlock(1);
            addBlock(77);
        }
    }
}
