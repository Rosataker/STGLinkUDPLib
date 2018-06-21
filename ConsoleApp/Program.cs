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


        static void Main(string[] args)
        {
            IDictionary<string, string> configDic = new Dictionary<string, string>();
            configDic["_IP"] = "127.0.0.1";
            configDic["_PORT"] = 0x869C.ToString();
            configDic["_Code"] = 0x7A1.ToString(); //.4.a MachDataCmdPacket
            configDic["_Cmd"] = 0x01.ToString(); //.4.a MachDataCmdPacket
            configDic["_Timeout_ms"] = "1500";
            configDic["_Retry_count"] = "5";
            STGLinkUDPLib STGLinkUDPLib = new STGLinkUDPLib(configDic);
            STGLinkUDPLib.RunClient();


            Console.Read();
        }
    }
}
