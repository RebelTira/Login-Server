using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoginServer.Packets
{
    class CP_ServerRefresh : Handler
    {
        public override void Handle(User usr)
        {
            usr.send(new SP_ServerRefresh(usr));
        }
    }

    class SP_ServerRefresh :Packet
    {
        public SP_ServerRefresh(User usr)
        {
            newPacket(4609);
            var servers = ServersInformations.collected.Values.Where(s => s.minrank <= usr.rank);
            addBlock(servers.Count());
            addBlock(""); // ?

            foreach (Server s in servers)
            {
                addBlock(s.id);
                addBlock(s.name);
                addBlock(s.ip);
                addBlock(5340);
                addBlock(Generic.getOnlinePlayers(s.id) * Generic.ServerSlots(s.slot));
                addBlock(s.flag);
            }
        }
    }
}
