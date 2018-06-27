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
        public UdpClient _UdpClient;
        public IPEndPoint _IPEndPoint;
        public string _IP { get; private set; }
        public int _PORT { get; private set; }
        public int _Code { get; private set; }
        public byte _Cmd { get; private set; }
        public int _TIMEOUT_MS { get; private set; }
        public int _RETRY_COUNT { get; private set; }
        public bool _configDicSetError { get; private set; }
        
        public STUDPBaseLib(IDictionary<string, string> configDic)
        {
            _configDicSetError = true;
            if (!configDic.ContainsKey("_IP")) _configDicSetError = false;
            if (!configDic.ContainsKey("_PORT")) _configDicSetError = false;
            if (!configDic.ContainsKey("_Code")) _configDicSetError = false;
            if (!configDic.ContainsKey("_Cmd")) _configDicSetError = false;
            if (!configDic.ContainsKey("_TIMEOUT_MS")) _configDicSetError = false;
            if (!configDic.ContainsKey("_RETRY_COUNT")) _configDicSetError = false;

            if (!_configDicSetError)
            {
                Console.WriteLine("configDic Value Error!");
                Console.Read();
            }


            _IP = configDic["_IP"];
            _PORT = int.Parse(configDic["_PORT"]);
            _Code = int.Parse(configDic["_Code"]);
            _Cmd = byte.Parse(configDic["_Cmd"]);
            _TIMEOUT_MS = int.Parse(configDic["_TIMEOUT_MS"]);
            _RETRY_COUNT = int.Parse(configDic["_RETRY_COUNT"]);
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
