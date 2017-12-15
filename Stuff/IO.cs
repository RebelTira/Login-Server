using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;
using System.Linq;

namespace LoginServer
{
    public static class IO
    {
        public static string path = null;
        public static string workingDirectory { get { return System.Windows.Forms.Application.StartupPath; } }

        public static string ReadValue(string section, string value, bool takelast = true)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(path);

                XmlNode mainNode = document.DocumentElement.SelectSingleNode(section);

                var x = document.GetElementsByTagName(value);

                if (x.Count > 1)
                {
                    Log.WriteLine("There are more nodes with the name " + section + "/" + value);
                }

                return x[(takelast ? x.Count - 1 : 0)].InnerText;
            }
            catch (Exception ex)
            {
                Log.WriteError("Error while reading " + section + " [" + value + "]");
            }
            return "0";
        }

        public static string ReadAttribute(string section, string value, string subvalue)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(path);

                var elements = document.DocumentElement.SelectNodes(section + @"/" + value).Cast<XmlElement>().First().Attributes;

                return elements.Cast<XmlAttribute>().Where(r => r.Name.ToLower() == subvalue.ToLower()).First().Value;
            }
            catch (Exception ex)
            {
                Log.WriteError("Error while reading " + section + " [" + value + " _ " + subvalue + "]");
            }
            return null;
        }
    }
}
