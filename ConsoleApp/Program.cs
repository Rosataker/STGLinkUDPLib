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
        private static int _Code = Int32.Parse(ConfigurationManager.AppSettings["_Code"], System.Globalization.NumberStyles.HexNumber);
        private static int _Cmd = Int32.Parse(ConfigurationManager.AppSettings["_Cmd"], System.Globalization.NumberStyles.HexNumber);


        static void Main(string[] args)
        {
            STGLinkUDPLib STGLinkUDPLib = new STGLinkUDPLib();

            STGLinkUDPBaseLib._Code = _Code;
            STGLinkUDPBaseLib._Cmd = _Cmd;

            STGLinkUDPLib.RunClient(_IP, _PORT);
            //STGLinkUDPLib.RunClient(_IP, 5555);


            //foreach (string key in ConfigurationManager.AppSettings)
            //{
            //    string value = ConfigurationManager.AppSettings[key];
            //    Console.WriteLine("Key: {0}, Value: {1}", key, value);
            //}



            Console.Read();
        }



    }
}
