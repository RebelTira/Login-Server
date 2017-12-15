using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace LoginServer
{
    /// <summary>
    /// This class is used to print out logs in console
    /// </summary>
    class Log
    {
        
        private static object writeObj = new object();
        public static void WriteLine(string str)

        {
            writeline(str, ConsoleColor.DarkGreen);
        }

        public static void WriteError(string str)
        {
            writeline(str, ConsoleColor.DarkRed);
        }

        public static void WriteDebug(string str)
        {
            writeline(str, ConsoleColor.DarkMagenta);
        }

        public static void WriteBlank(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("");
            }
        }
        
        private static void writeline(string str, ConsoleColor c)
        {
            lock (writeObj)

            {
                
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("[" + DateTime.Now.ToString("hh:mm:ss.fff - dd/MM/yyyy") + "] > ");
                StackFrame frame = new StackTrace().GetFrame(2); //>--- Añadido para Ver el metodo
                Console.Write(frame.GetMethod().ReflectedType.Name + "." + frame.GetMethod().Name + "-");  //>--- Añadido para Ver el metodo
                Console.ForegroundColor = c;
                Console.Write(str);
                Console.WriteLine("");
            }
        }
    }
}
