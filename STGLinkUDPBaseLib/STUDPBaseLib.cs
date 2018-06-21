using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using ISTUDP;
using System.Runtime.InteropServices;
using System.Globalization;

namespace STUDPBase
{
    public class STUDPBaseLib : ISTUDPInterface
    {
        public static UdpClient _UdpClient;
        public static IPEndPoint _IPEndPoint;
        public static string _IP { get; private set; }
        public static int _PORT { get; private set; }
        public static int _Code { get; private set; }
        public static byte _Cmd { get; private set; }
        public static int _Timeout_ms { get; private set; }
        public static int _Retry_count { get; private set; }
        public static bool _configDicSetError { get; private set; }

        public STUDPBaseLib(IDictionary<string, string> configDic)
        {
            _configDicSetError = true;
            if (!configDic.ContainsKey("_IP")) _configDicSetError = false;
            if (!configDic.ContainsKey("_PORT")) _configDicSetError = false;
            if (!configDic.ContainsKey("_Code")) _configDicSetError = false;
            if (!configDic.ContainsKey("_Cmd")) _configDicSetError = false;
            if (!configDic.ContainsKey("_Timeout_ms")) _configDicSetError = false;
            if (!configDic.ContainsKey("_Retry_count")) _configDicSetError = false;

            if (!_configDicSetError)
            {
                Console.WriteLine("configDic Value Error!");
                Console.Read();
            }


            _IP = configDic["_IP"];
            _PORT = int.Parse(configDic["_PORT"]);
            _Code = int.Parse(configDic["_Code"]);
            _Cmd = byte.Parse(configDic["_Cmd"]);
            _Timeout_ms = int.Parse(configDic["_Timeout_ms"]);
            _Retry_count = int.Parse(configDic["_Retry_count"]);
        }

        public bool Connected { get; set; }

        public void Open()
        {
            Connected = true;
            _IPEndPoint = new IPEndPoint(IPAddress.Parse(_IP), _PORT);
            _UdpClient = new UdpClient();
        }

        public void Destructor()
        {
            Connected = false;
            _UdpClient.Close();
        }



    }


}
