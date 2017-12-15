using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoginServer.Configs
{
    class Main
    {
        public static void setup()
        {
            try
            {
                Patch.Load();
                Managers.CountryManager.Load();
                Log.WriteLine("Configs loaded successfully");
            }
            catch (Exception ex)
            {
                Log.WriteError("Couldn't setup configs (" + ex.Message + ") @ " + ex.StackTrace);
            }
        }
    }
}
