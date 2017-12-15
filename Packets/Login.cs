using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LoginServer.Packets
{
    class SP_LoginPacket : Packet
    {
        public enum ErrorCodes
        {
            Nickname = 72000,
            WrongUser = 72010,
            WrongPW = 72020,
            AlreadyLoggedIn = 72030,
            Banned = 73050,
            BannedTime = 73020,
            AlreadyUsedNick = 74070,
            InvalidArea = 110040,
            ChargebackBan = 110043
        }

        public SP_LoginPacket(ErrorCodes errcode, params object[] blocks)
        {
            newPacket(4352);
            addBlock((int)errcode);
            foreach (object obj in blocks)
            {
                addBlock(obj);
            }
        }

        public SP_LoginPacket(User usr)
        {
            newPacket(4352);
            addBlock(1); // error code
            addBlock(usr.userId);
            addBlock(0);
            addBlock(usr.username);
            addBlock("NULL"); // password
            addBlock(usr.nickname);
            addBlock(usr.sessionId); // ticket id
            addBlock(1); // Unique ID 1 - Default = 1
            addBlock(0); // Unique ID 2 - Default = 0
            addBlock(usr.rank > 2 ? 5 : 0); // AccessLevel
            addBlock("WarRock"); // Passport
            if (usr.clanid != -1)
            {
                addBlock(usr.clanid); // Clan ID
                addBlock(usr.clanname); // Clan Name
                addBlock(usr.clanrank); // Clan Rank
                addBlock(usr.claniconid); // Clan Icon ID
            }
            else
            {
                addBlock(-1);
                addBlock("NULL");
                Fill(-1, 2); // Clan blocks
            }
            Fill(0, 4);

            var servers = ServersInformations.collected.Values.Where(s => s.minrank <= usr.rank);
            addBlock(servers.Count());
            
            foreach (Server s in servers)
            {
                addBlock(s.id);
                addBlock(s.name);
                addBlock(s.ip);
                addBlock(10375);
                addBlock(Generic.getOnlinePlayers(s.id) * Generic.ServerSlots(s.slot));
                addBlock(s.flag);
            }
            //
            Fill(0, 2);
        }
    }

    class CP_LoginHandler : Handler
    {
        public override void Handle(User usr)
        {
            //2209773189 0 gigino asdasd123 0 0
            int userId = 0;
            string username = getBlock(2).Trim();
            if (username.Length > 0 && username.Length <= 20)
            {
                string password = getBlock(3).Trim();
                try
                {
                    userId = int.Parse(DB.runReadOnce("id", "SELECT * FROM users WHERE username='" + username + "'").ToString());
                }
                catch { userId = 0; }

                if (userId > 0)
                {
                    DataTable dt = DB.runRead("SELECT id, username, password, salt, online, nickname, rank, firstlogin, banned, bantime, clanid, clanrank FROM users WHERE id='" + userId + "'");
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];

                        string salt = row["salt"].ToString();
                        string md5Password = Generic.convertToMD5(Generic.convertToMD5(password) + Generic.convertToMD5(salt)); 
                        //usr.password = row["password"].ToString();
                        //if (usr.password == row["password"].ToString()) 
                        if (md5Password == row["password"].ToString())
                        {
                            usr.userId = int.Parse(row["id"].ToString());
                            usr.username = row["username"].ToString();
                            usr.nickname = row["nickname"].ToString();
                            usr.rank = int.Parse(row["rank"].ToString());
                            usr.clanid = int.Parse(row["clanid"].ToString());
                            usr.clanrank = int.Parse(row["clanrank"].ToString());
                            bool online = (row["online"].ToString() == "1");
                            usr.firstlogin = (row["firstlogin"].ToString() == "0");
                            bool banned = (usr.rank == 0 || row["banned"].ToString() == "1");
                            string bantime = row["bantime"].ToString();
                            if (!banned)
                            {
                                if (!online)
                                {
                                    if (usr.clanid != -1)
                                    { // Tiene clan
                                        DataTable mydt = DB.runRead("SELECT name, iconid FROM clans WHERE id='" + usr.clanid + "'");
                                        if (mydt.Rows.Count > 0)
                                        {
                                            DataRow myrow = mydt.Rows[0];
                                            usr.clanname = myrow["name"].ToString();
                                            usr.claniconid = long.Parse(myrow["iconid"].ToString());
                                        }
                                    }
                                    DB.runQuery("UPDATE users SET ticketid='" + usr.sessionId + "' WHERE id='" + usr.userId + "'");
                                    if (usr.firstlogin || usr.nickname.Length <= 0) // Si es la primera vez o no tiene nick...
                                    {
                                        usr.send(new Packets.SP_LoginPacket(Packets.SP_LoginPacket.ErrorCodes.Nickname));
                                        Log.WriteLine(">Login> Connection from " + usr.ip + " logged in successfully in as new user (" + username + ")"); // ... Logueado con el user
                                    }
                                    else
                                    {
                                        usr.send(new Packets.SP_LoginPacket(usr)); //>--- Salen los Servers
                                        Log.WriteLine(">Login> Connection from " + usr.ip + " logged in successfully as " + usr.nickname); // ... Logueado con el nick
                                    }
                                }
                                else
                                { // Ya esta online 
                                    usr.send(new Packets.SP_LoginPacket (Packets.SP_LoginPacket.ErrorCodes.AlreadyLoggedIn));
                                    Log.WriteError(">Login> Connection from " + usr.ip + " tried to log on " + username + " but he is logged in");
                                }
                            }
                            else
                            { // Banned
                                DateTime Time;
                                DateTime.TryParseExact(bantime, "yyMMddHH", null, System.Globalization.DateTimeStyles.None, out Time);
                                DateTime Today = DateTime.Now;
                                if (Time.Year < Today.Year)
                                {
                                    usr.send(new Packets.SP_LoginPacket(Packets.SP_LoginPacket.ErrorCodes.Banned));
                                }
                                else
                                {
                                    TimeSpan ts = (Time - Today);
                                    usr.send(new Packets.SP_LoginPacket(Packets.SP_LoginPacket.ErrorCodes.Banned, ts.Minutes));
                                }
                                Log.WriteError(">Login> Connection from " + usr.ip + " as " + username + " but he/she is banned");
                            }
                        }
                        else
                        {
                            usr.send(new Packets.SP_LoginPacket(Packets.SP_LoginPacket.ErrorCodes.WrongPW));
                            Log.WriteError(">Login> Connection from " + usr.ip + " tried to log on as " + username + " with a wrong password");
                        }
                    }
                    else
                    {
                        usr.send(new Packets.SP_LoginPacket(Packets.SP_LoginPacket.ErrorCodes.WrongUser));
                        Log.WriteError(">Login> Connection from " + usr.ip + " failed to log on as " + username);
                    }
                }
                else
                {
                    usr.send(new Packets.SP_LoginPacket(Packets.SP_LoginPacket.ErrorCodes.WrongUser));
                    Log.WriteError(">Login> Connection from " + usr.ip + " failed to log on as " + username);
                }
            }
        }
    }
}
