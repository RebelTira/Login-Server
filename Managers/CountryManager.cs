using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LoginServer.Managers
{
    class CountryManager
    {
        private static List<string> countries = new List<string>();
        public static void Load()
        {
            countries.Clear();
            string[] c = IO.ReadValue("Settings", "LockedCountries").Split(',');
            foreach (string country in c)
            {
                if (country.Length > 0)
                {
                    if (country.Length == 2)
                    {
                        countries.Add(country.Trim().ToUpper());
                    }
                    else
                    {
                        Log.WriteError(country + " is not a valid country");
                    }
                }
            }
        }

        public static bool IsLockedCountry(string countryCode)
        {
            return countries.Contains(countryCode.ToUpper());
        }
    }
}
