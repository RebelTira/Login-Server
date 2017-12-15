using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;

namespace LoginServer
{
    class Generic
    {
        public static long timestamp
        {
            get
            {
                TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
                return (uint)(ts.TotalSeconds);
            }
        }

        public static string currentDate
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss dd/MM/yy");
            }
        }

        public static string runningSince
        {
            get
            {
                TimeSpan ts = DateTime.Now - Process.GetCurrentProcess().StartTime;
                return "Running since " + ts.Days + " days, " + ts.Hours + " hours," + ts.Minutes + " minutes!";
            }
        }

        public static string convertToMD5(string Input)
        {
            try
            {
                System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Input);
                byte[] hash = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int I = 0; I < hash.Length; I++)
                {
                    sb.Append(hash[I].ToString("x2"));
                }

                return sb.ToString();
            }
            catch { return null; }
        }

        public static bool isMacAddress(string mac)
        {
            try
            {
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("^([:xdigit:]){12}$");
                return !r.IsMatch(mac);
            }
            catch
            {
                return false;
            }
        }

        public static int getOnlinePlayers(int srvid)
        {
            DataTable dt = DB.runRead("SELECT * FROM users WHERE online='1' AND serverid='" + srvid + "'");
            return dt.Rows.Count;
        }

        public static bool isAlphaNumeric(string input)
        {
            try
            {
                System.Text.RegularExpressions.Regex objAlphaNumericPattern = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");
                return !objAlphaNumericPattern.IsMatch(input);
            }
            catch
            {
                return false;
            }
        }

        public static int ServerSlots(int slots)
        {
			int count = 0;
			int.TryParse((Math.Truncate((double)(2500 / slots)).ToString()), out count);
			return count;
        }
    }
}
