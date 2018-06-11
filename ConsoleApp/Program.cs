using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using STGLinkUDP;
using STGLinkUDP.STGLinkUDPBase;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        private static string _IP = ConfigurationManager.AppSettings["_IP"];
        private static int _PORT = Int32.Parse(ConfigurationManager.AppSettings["_PORT"], System.Globalization.NumberStyles.HexNumber);

         
        static void Main(string[] args)
        {

            STGLinkUDPLib STGLinkUDPLib = new STGLinkUDPLib();
            
            STGLinkUDPLib.RunClient(_IP, _PORT);
            

            //foreach (string key in ConfigurationManager.AppSettings)
            //{
            //    string value = ConfigurationManager.AppSettings[key];
            //    Console.WriteLine("Key: {0}, Value: {1}", key, value);
            //}

            //Console.WriteLine("_Code->{0}", Convert.ToInt32(ConfigurationManager.AppSettings["_Code"], 16));
            //Console.WriteLine("_Cmd->{0}", Convert.ToInt32(ConfigurationManager.AppSettings["_Cmd"], 16));

            Console.Read();
        }



    }
}
