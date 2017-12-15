using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoginServer.Packets
{
    class SP_PatchInformationUpdatePacket : Packet
    {
        public SP_PatchInformationUpdatePacket()
        {
            newPacket(4252);
            addBlock(ServersInformations.collected.Count);
            int count = 0;
            foreach (Server s in ServersInformations.collected.Values)
            {
                count += Generic.getOnlinePlayers(s.id);
            }
            addBlock(count);
        }
    }

    class CP_PatchInformationUpdateHandler : Handler
    {
        public override void Handle(User usr)
        {
            usr.send(new SP_PatchInformationUpdatePacket());
        }
    }
}
