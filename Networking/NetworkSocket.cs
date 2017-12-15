using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace LoginServer.Networking
{
    /// <summary>
    /// This class listen's connections
    /// </summary>
    class NetworkSocket
    {
        private static Socket socket;
        private static int acceptedConnections = 0;

        public static bool InitializeSocket(int port)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(IPAddress.Any, port));
                socket.Listen(0);
                socket.BeginAccept(new AsyncCallback(OnReceive), socket);
                Log.WriteLine("> --- NetworkSocket-26 Listening connections from port: " + port);  // port=5330
                return true;
            }
            catch { } 
            return false;
        }

        private static void OnReceive(IAsyncResult iAr)
        {
            socket.BeginAccept(new AsyncCallback(OnReceive), socket);
            try
            {
                Socket remoteSocket = ((Socket)iAr.AsyncState).EndAccept(iAr);
                string ip = remoteSocket.RemoteEndPoint.ToString().Split(':')[0];
                Log.WriteLine("> --- NetworkSocket-40 Accepted connection from IP: " + ip);
                acceptedConnections++;
                if (acceptedConnections >= Configs.Server.MaxSessions) acceptedConnections = 1;
                Country c = Program.ipLookup.getCountry(ip);
                string country = c.getCode();
                if (!Managers.CountryManager.IsLockedCountry(country))
                {
                    User user = new User(acceptedConnections, remoteSocket);
                }
                else
                {
                    Log.WriteError(ip + " has been refused [" + country + "]");
                }
            }
            catch
            {
            }
        }
    }
}
