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
        
            IDictionary<string, string> configDic = new Dictionary<string, string>();
            configDic["_IP"] = ConfigurationManager.AppSettings["_IP"];
            configDic["_PORT"] = Convert.ToInt32(ConfigurationManager.AppSettings["_PORT"], 16).ToString();
            configDic["_Code"] = Convert.ToInt32(ConfigurationManager.AppSettings["_Code"], 16).ToString(); //.4.a MachDataCmdPacket
            configDic["_Cmd"] = Convert.ToInt32(ConfigurationManager.AppSettings["_Cmd"], 16).ToString(); //.4.a MachDataCmdPacket
            configDic["_TIMEOUT_MS"] = "1500";
            configDic["_RETRY_COUNT"] = "5";
            STGLinkUDPLib STGLinkUDPLib = new STGLinkUDPLib(configDic);
            STGLinkUDPLib.RunClient();


            Console.Read();
        }
    }
}
