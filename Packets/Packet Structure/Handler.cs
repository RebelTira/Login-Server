using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoginServer.Packets
{
    class Handler
    {
        private long timeStamp = 0;
        private int packetId = 0;
        private string[] blocks;

        public void FillData(long timeStamp, int packetId, string[] blocks)
        {
            this.timeStamp = timeStamp;
            this.packetId = packetId;
            this.blocks = blocks;
        }

        public string[] getAllBlocks
        {
            get
            {
                return this.blocks;
            }
        }

        public string getBlock(int i)
        {
            if (blocks[i] != null)
                return blocks[i];
            return null;
        }

        public virtual void Handle(User usr)
        {
            /* Override */
        }
    }
}
