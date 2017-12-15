using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;

namespace LoginServer
{
    /// <summary>
    /// This class is used for initialize the start up
    /// </summary>
    class Program
    {
        public static bool running = false;
        public static LookupService ipLookup;

        static void Main(string[] args)
        {
            Console.Title = Configs.Console.title;
            Console.WriteLine(@"  _______ ______   _______ _____ _____ _  _____      _____ ____  _____  ______ ");
            Console.WriteLine(@" |__   __/ __ \ \ / |_   _|_   _/ ____( )/ ____|    / ____/ __ \|  __ \|  ____|");
            Console.WriteLine(@"    | | | |  | \ V /  | |   | || |    |/| (___     | |   | |  | | |__) | |__   ");
            Console.WriteLine(@"    | | | |  | |> <   | |   | || |       \___ \    | |   | |  | |  _  /|  __|  ");
            Console.WriteLine(@"    | | | |__| / . \ _| |_ _| || |____   ____) |   | |___| |__| | | \ \| |____ ");
            Console.WriteLine(@"    |_|  \____/_/ \_|_____|_____\_____| |_____/     \_____\____/|_|  \_|______|");
            Console.WriteLine(@"|______________________________________________________________________________|");
            Log.WriteBlank();
                                             
            Console.WindowWidth = Configs.Console.width; // Estaban como comentarios
            Console.WindowHeight = Configs.Console.heigth; // Los activo para coger los datos de Console.cs

            // Credits part
            Console.WriteLine(" - Wrote by ToXiiC - Version TiraChinas");
            Console.WriteLine(" - Thanks to CodeDragon");
            Log.WriteBlank(2);
            //Log.setup("X_Login " + DateTime.Now.ToString("dd-MM H-mm") + ".log"); //>--- Descomentar para que se guarden los logs
            Type t = Type.GetType("Mono.Runtime");

            if (t != null)
            {
                Log.WriteLine("This Login Server is running under Mono VM!");
            }
            //

            running = initializeStartup();
            Console.ReadKey(true);
            new AutoResetEvent(false).WaitOne();
        }

        static bool initializeStartup()
        {
            string settingsFile = Application.StartupPath + "/loginserver.xml";
            if (!System.IO.File.Exists(settingsFile))
            {
                Log.WriteError("Error: Cannot find loginserver.xml");
                Console.ReadKey();
                Console.ReadKey();
                return false;
            }

            string GeoIP = IO.workingDirectory + @"/GeoIP.dat";
            if (System.IO.File.Exists(GeoIP) == false)
            {
                Log.WriteError("Error: Cannot find GeoIP.dat");
                return false;
            }

            ipLookup = new LookupService(GeoIP, LookupService.GEOIP_MEMORY_CACHE);

            IO.path = settingsFile;

            Configs.Main.setup();
            
            string host = IO.ReadValue("Database", "host");
            int port = int.Parse(IO.ReadValue("Database", "port"));
            string username = IO.ReadValue("Database", "user");
            string password = IO.ReadValue("Database", "password");
            string database = IO.ReadValue("Database", "database");
            int poolsize = int.Parse(IO.ReadValue("Database", "poolsize"));

            DB.openConnection(host, port, database, username, password, poolsize);

            ServersInformations.loadServers();

            Managers.Packet_Manager.setup();

            if (!Networking.NetworkSocket.InitializeSocket(5330))
            {
                Log.WriteError("Error: Cannot Initialize a new socket on the port 5330");
                Console.ReadKey();
                Console.ReadKey();
                return false;
            }
            
            return true;
        }
    }
}
