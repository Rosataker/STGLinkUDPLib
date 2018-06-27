using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using STGLinkUDP;
using System.IO;

namespace ConsoleApp
{
    class Program
    {        
        

        static void Main(string[] args) {
            //Console.WriteLine("args->{0} ,{1} ,{2}", args[0], args[1], args[2]);
            //Console.Read();
            
            
            IDictionary<string, string> configDic = new Dictionary<string, string>();
            configDic["_IP"] = args[0];
            configDic["_PORT"] = Convert.ToInt32(args[1], 16).ToString();
            configDic["_Code"] = Convert.ToInt32(args[2], 16).ToString(); //.4.a MachDataCmdPacket
            configDic["_Cmd"] = Convert.ToInt32(args[3], 16).ToString(); //.4.a MachDataCmdPacket
            configDic["_TIMEOUT_MS"] = args[4];
            configDic["_RETRY_COUNT"] = args[5];
            //configDic["_IP"] = ConfigurationManager.AppSettings["_IP"];
            //configDic["_PORT"] = Convert.ToInt32(ConfigurationManager.AppSettings["_PORT"], 16).ToString();
            //configDic["_Code"] = Convert.ToInt32(ConfigurationManager.AppSettings["_Code"], 16).ToString(); //.4.a MachDataCmdPacket
            //configDic["_Cmd"] = Convert.ToInt32(ConfigurationManager.AppSettings["_Cmd"], 16).ToString(); //.4.a MachDataCmdPacket
            //configDic["_TIMEOUT_MS"] = ConfigurationManager.AppSettings["_TIMEOUT_MS"];
            //configDic["_RETRY_COUNT"] = ConfigurationManager.AppSettings["_RETRY_COUNT"];
            STGLinkUDPLib STGLinkUDPLib = new STGLinkUDPLib(configDic);
            STGLinkUDPLib.RunClient();


            Console.Read();
        }
    }
}
