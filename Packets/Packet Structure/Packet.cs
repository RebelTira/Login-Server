using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace LoginServer.Packets
{
    /// <summary>
    /// This class defines the packet structure
    /// </summary>
    class Packet
    {
        /* Causes crash on Linux - Mono */
        /*[DllImport("winmm.dll")]
        public static extern long timeGetTime();*/

        int packetId = -1;
        StringBuilder packet = new StringBuilder();

        public byte[] getBytes()
        {
            string fullPacket = this.packet.ToString().Remove(packet.Length - 1, 1);

            Log.WriteDebug(">---PKT- OUT :: " + packet); //>--- +++
            
            /* Default - for Windows */
            /* Windows-1250 - for Windows & Linux mono */

            return Packets.Cryption.XOR.encrypt(Encoding.GetEncoding("Windows-1250").GetBytes(fullPacket.ToString() + (char)0x20 + (char)0x0A));
        }

        protected void newPacket(int packetId)
        {
            if (this.packetId == -1)
            {
                this.packetId = packetId;
                this.packet.Append(Environment.TickCount);
                this.packet.Append(" ");
                this.packet.Append(packetId);
                this.packet.Append(" ");
            }
            else
            {
                Log.WriteError("Coudln't re-declare packetId!");
            }
        }

        protected void addBlock(object block)
        {
            block = block.ToString().Replace((char)0x20, (char)0x1D);
            this.packet.Append(block.ToString());
            this.packet.Append(" ");
        }

        protected void Fill(object block, int length)
        {
            for (int i = 0; i < length; i++)
            {
                addBlock(block);
            }
        }
    }
}
