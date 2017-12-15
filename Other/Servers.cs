using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LoginServer
{
    class ServersInformations
    {
        public static Dictionary<int, Server> collected = new Dictionary<int, Server>();

        public static void loadServers()
        {	 // >>> Lee el server de la DB
            DataTable dt = DB.runRead("SELECT * FROM servers WHERE visible='1' ORDER BY serverid ASC");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];
                int serverId = int.Parse(row["serverid"].ToString());
                string name = row["name"].ToString();
                string ip = row["ip"].ToString();
                int flag = int.Parse(row["flag"].ToString());
                int minrank = int.Parse(row["minrank"].ToString());
                int slot = int.Parse(row["slot"].ToString());

                Server s = new Server(serverId, name, ip, flag, minrank, slot);

                collected.Add(i, s);
            }
            Log.WriteLine("Loaded " + dt.Rows.Count+ " (visible) servers!");
        }
    }

    /// <summary>
    /// Server Informations such as ip, flag, min rank for access are stored here
    /// </summary>
    class Server
    {
        public int id;
        public string name;
        public string ip;
        public int flag;
        public int minrank;
        public int slot;

        public Server(int serverid, string name, string ip, int flag, int rank, int slot)
        {
            this.id = serverid;
            this.name = name;
            this.ip = ip;
            this.flag = flag;
            this.minrank = rank;
            this.slot = slot;
        }
    }
}
