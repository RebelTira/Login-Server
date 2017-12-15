using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LoginServer.Packets
{
    class CP_NewUserHandler : Handler
    {
        public override void Handle(User usr)
        {
            int userId = usr.userId;

            if (userId > 0)
            {
                string nickname = DB.Stripslash(getBlock(0));
                DataTable nickAlreadyInUse = DB.runRead("SELECT * FROM users WHERE nickname='" + nickname + "'");
                bool alphanumeric = Generic.isAlphaNumeric(nickname);
                usr.firstlogin = true;
                if (alphanumeric)
                {
                    if (nickAlreadyInUse.Rows.Count == 0)
                    {
                        usr.nickname = nickname;
                        DB.runQuery("UPDATE users SET nickname='" + nickname + "', firstlogin='2', ticketid='" + usr.sessionId + "', serverid='-1' WHERE id='" + usr.userId + "'");
                        usr.send(new SP_LoginPacket(usr));
                    }
                    else
                    {
                        usr.send(new SP_LoginPacket(SP_LoginPacket.ErrorCodes.AlreadyUsedNick));
                    }
                }
                else
                {
                    usr.send(new SP_LoginPacket(SP_LoginPacket.ErrorCodes.Nickname));
                }
            }
            else
            {
                usr.disconnect();
            }
        }
    }
}
