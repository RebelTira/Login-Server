using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using LoginServer.Packets;

namespace LoginServer
{
    /// <summary>
    /// This class stores all informations such as level, exp, clan
    /// </summary>
    class User : IDisposable
    {
        // Account Informations
        public int userId = 0;
        public int sessionId = 0;
        public int rank = 1;
        public int clanid = -1;
        public int clanrank = 0;
        public long claniconid = 0;
        public string username = "#";
        public string nickname = "#";
        public string clanname = "NULL";
        public bool firstlogin = false;

        // Connection Data
        
        Socket socket;
        byte[] buffer = new byte[1024];
        bool disconnected = false;
        internal string password;

        public string ip { get { return socket.RemoteEndPoint.ToString().Split(':')[0]; } }

        public User(int sessionId, Socket socket)
        {
            this.socket = socket;
            this.sessionId = sessionId;

            this.SendBuffer(Configs.Server.incomingBuffer);

            this.socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
        }
        
        private void OnReceive(IAsyncResult iAr)
        {
            try
            {
                int length = socket.EndReceive(iAr);

                if (length > 0)
                {
                    byte[] packetBuffer = new byte[length];
                    Array.Copy(buffer, 0, packetBuffer, 0, length);

                    /* Default - for Windows */
                    /* Windows-1250 - for Windows & Linux mono */

                    packetBuffer = Packets.Cryption.XOR.decrypt(packetBuffer);

                    string[] packetStr = Encoding.Default.GetString(packetBuffer).Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string packet in packetStr)
                    {
                        if (packet.Length > 5)
                        {
                            Log.WriteDebug(">---Login_USR-71 packet IN:: " + packet);
                            try
                            {
                                Handler handler = Managers.Packet_Manager.parsePacket(packet);

                                if (handler != null)
                                {
                                    Thread t = new Thread(() => { try { handler.Handle(this); } catch { } });
                                    t.Start();
                                }
                            }
                            catch { }
                        }
                    }
                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
                }
                else
                {
                    disconnect();
                }
            }
            catch
            {
                disconnect();
            }
        }

        public void send(Packet p)
        {
            try { byte[] sendBuffer = p.getBytes(); if (sendBuffer != null) { socket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(sendCallBack), null); } }
            catch { disconnect(); }
        }

        public void SendBuffer(byte[] buffer)
        {
            try { if (buffer != null) { socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(sendCallBack), null); } }
            catch { disconnect(); }
        }

        private void sendCallBack(IAsyncResult iAr)
        {
            try { socket.EndSend(iAr); }
            catch { disconnect(); }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void disconnect()
        {
            if (disconnected) return;
            disconnected = true;
            
            try { socket.Close(); }
            catch { }

            this.socket = null;

            this.Dispose();
        }
    }
}