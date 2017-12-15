using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoginServer.Configs
{
    class Patch
    {
        public static string Format, Launcher, Updater, Client, Sub, Option, UpdateUrl;

        public static void Load()
        {
            Format = IO.ReadValue("UpdaterInformation", "Format");
            Launcher = IO.ReadValue("UpdaterInformation", "Launcher");
            Updater = IO.ReadValue("UpdaterInformation", "Updater");
            Client = IO.ReadValue("UpdaterInformation", "Client");
            Sub = IO.ReadValue("UpdaterInformation", "Sub");
            Option = IO.ReadValue("UpdaterInformation", "Option");
            UpdateUrl = IO.ReadValue("UpdaterInformation", "UpdaterURL");
        }
    }
}
