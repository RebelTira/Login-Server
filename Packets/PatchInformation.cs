using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoginServer.Packets
{
    class SP_PatchInformationPacket : Packet
    {
        public SP_PatchInformationPacket()
        {
            newPacket(4112);
            addBlock(Configs.Patch.Format); // Format
            addBlock(Configs.Patch.Launcher); // Launcher Version
            addBlock(Configs.Patch.Updater); // Updater Version
            addBlock(Configs.Patch.Client); // Client Version
            addBlock(Configs.Patch.Sub); // Sub Version
            addBlock(Configs.Patch.Option); // Option
            addBlock(Configs.Patch.UpdateUrl);
        }
    }

    class CP_PatchInformationHandler : Handler
    {
        public override void Handle(User usr)
        {
            usr.send(new SP_PatchInformationPacket());
            Log.WriteLine(">PatchInformation> Replying to updater with informations [" + Configs.Patch.UpdateUrl + "]");
        }
    }
}
