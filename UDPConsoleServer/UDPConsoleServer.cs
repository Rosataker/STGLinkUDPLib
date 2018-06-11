using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UDPConsoleServer
{
    class UDPConsoleServer
    {
        public static IPEndPoint _IPEP;
        public static UdpClient _UC;
        static void Main(string[] args)
        {
            Console.WriteLine("這是伺服器...\n");

            string MachIDCmdPacketSendBytes = string.Empty;


            _IPEP = new IPEndPoint(IPAddress.Any, 0x869C);
            _UC = new UdpClient(_IPEP.Port);
            do
            {
                ScanCmdPacketServer();
                MachIDCmdPacketServer();
                MachConnectCmdPacketServer();
                MachDataEchoPacketServer();
                //Thread.Sleep(1)


            } while (true);


        }
        private static void ScanCmdPacketServer()
        {
            #region ScanCmdPacketServer
            //1.ID < 2 > 0
            //2.Sz < 2 > 0
            //3.Cmd < 1 > 0x30
            //4.Count < 2 > 自定
            //5.Sum < 1 > CheckSum 之數值，此封包數值總合為 = 0;
            #endregion
            Byte[] sendBytes = new byte[] {
                     0,0
                    ,0,0
                    ,0x30
                    ,0,4
                    ,0
                    };

            bool _Close = false;
            while (!_Close)
            {
                byte[] buffer = _UC.Receive(ref _IPEP);
                Console.WriteLine("ScanCmdPacketServer 接收");
                if (buffer[4] == 0x20)
                {
                    #region Debug
                    //Console.WriteLine("接收 ScanCmdPacket : ");
                    //foreach (byte buf in buffer)
                    //{
                    //    Console.WriteLine("buf->{0}", buf);
                    //}


                    //Console.WriteLine("回傳 ScanCmdPacket : ");


                    //foreach (byte buf in sendBytes)
                    //{
                    //    Console.WriteLine("buf->{0}", buf);
                    //}
                    #endregion
                    Console.WriteLine("ScanCmdPacketServer 回傳");

                    _UC.Send(sendBytes, sendBytes.Length, _IPEP);
                    _Close = true;
                }
            }
        }
        //設定回傳參數
        private static void MachIDCmdPacketBytesSet(out Byte[] sendBytes)
        {
            sendBytes = new Byte[] {
                0,0,
                0,0x43,
                0x33,
                0,0,
                0,
                4,
                1,
                1,1,
                0x02,
                0
            };
            Array.Resize(ref sendBytes, sendBytes.Length + 61);
            byte[] MachineName = Encoding.UTF8.GetBytes(" it is MachineName ");
            int sb = 14;
            for (int i = 1; i <= 60; i++)
            {
                try
                {
                    sendBytes[sb + i] = MachineName[i];
                }
                catch (Exception)
                {
                    sendBytes[sb + i] = 0;
                }
            }
            sendBytes[74] = 0;
        }
        private static void MachIDCmdPacketServer()
        {
            #region MachIDCmdPacketServer
            //1.ID < 2 > 0
            //2.Sz < 2 > 0x43 封包長度[< ID0 > +< Ver1 > +…+< UserDef >]
            //3.Cmd < 1 > 0x33 回應連線要求,表示同意連線要求。
            //4.Count < 2 > 自定
            //5.ID0 < 1 > 0
            //6.Ver1 < 1 > 4
            //7.Ver2 < 1 > 1
            //8.BugFix < 2 > 1
            //9.TypeIdoD < 1 > 線切割機為 0x02，深孔機為 0x04
            //10.SubTypeID < 1 >
            //11.UserDef < 60 > 機台名稱
            //12.Sum < 1 > Check Sum 之數值，此封包數值總合為 = 0;
            #endregion

            byte[] sendBytes = new byte[] { };
            MachIDCmdPacketBytesSet(out sendBytes);
            
            
            bool _Close = false;
            while (!_Close)
            {
                byte[] buffer = _UC.Receive(ref _IPEP);
                Console.WriteLine("MachIDCmdPacketServer 接收");

                if (buffer[4] == 0x21)
                {
                    #region Debug
                    //Console.WriteLine("接收 MachIDCmdPacket : ");
                    //foreach (byte buf in buffer)
                    //{
                    //    Console.WriteLine("buf->{0}", buf);
                    //}


                    //Console.WriteLine("回傳 MachIDCmdPacket : ");


                    //foreach (byte buf in sendBytes)
                    //{
                    //    Console.WriteLine("buf->{0}", buf);
                    //}
                    #endregion
                    Console.WriteLine("MachIDCmdPacketServer 回傳");

                    _UC.Send(sendBytes, sendBytes.Length, _IPEP);
                    _Close = true;
                }
            }
        }

        private static void MachConnectCmdPacketBytesSet(out Byte[] sendBytes)
        {
            sendBytes = new Byte[] {
                1,1
                ,0x0C,0x0C
                ,0x31
                ,0,0
                ,0x04,0x04
                ,0
                ,0
                ,0,0,0,0
                ,1,1
                ,1,1
                ,0
            };       
        }
        private static void MachConnectCmdPacketServer()
        {
            byte[] sendBytes = new byte[] { };
            MachConnectCmdPacketBytesSet(out sendBytes);


            bool _Close = false;
            while (!_Close)
            {
                byte[] buffer = _UC.Receive(ref _IPEP);
                Console.WriteLine("MachConnectCmdPacketServer 接收");

                if (buffer[4] == 0x22)
                {
                    #region Debug
                    //Console.WriteLine("接收 MachConnectCmdPacketServer : ");
                    //foreach (byte buf in buffer)
                    //{
                    //    Console.WriteLine("buf->{0}", buf);
                    //}


                    //Console.WriteLine("回傳 MachConnectCmdPacketServer : ");


                    //foreach (byte buf in sendBytes)
                    //{
                    //    Console.WriteLine("buf->{0}", buf);
                    //}
                    #endregion
                    Console.WriteLine("MachConnectCmdPacketServer 回傳");

                    _UC.Send(sendBytes, sendBytes.Length, _IPEP);
                    _Close = true;
                }
            }
        }

        private static void MachDataEchoPacketBytesSet(out Byte[] sendBytes)
        {
            sendBytes = new Byte[] {
               0
               ,1
               ,255,255
               ,0x01
               ,0,0
               ,255,255
               ,0x50
               ,0
               ,0,0,0,0
               ,1,8,2,0
               ,0,0,0,0
            };
            Array.Resize(ref sendBytes, sendBytes.Length + 801);
            sendBytes[sendBytes.Length - 1] = 0;
        }
        private static void MachDataEchoPacketServer()
        {
            byte[] sendBytes = new byte[] { };
            MachConnectCmdPacketBytesSet(out sendBytes);


            bool _Close = false;
            while (!_Close)
            {
                byte[] buffer = _UC.Receive(ref _IPEP);
                Console.WriteLine("MachDataEchoPacketServer 接收");

                if (buffer[4] == 0x22)
                {
                    #region Debug
                    //Console.WriteLine("接收 MachDataEchoPacketServer : ");
                    //foreach (byte buf in buffer)
                    //{
                    //    Console.WriteLine("buf->{0}", buf);
                    //}


                    //Console.WriteLine("回傳 MachDataEchoPacketServer : ");


                    //foreach (byte buf in sendBytes)
                    //{
                    //    Console.WriteLine("buf->{0}", buf);
                    //}
                    #endregion
                    Console.WriteLine("MachDataEchoPacketServer 回傳");

                    _UC.Send(sendBytes, sendBytes.Length, _IPEP);
                    _Close = true;
                }
            }
        }
    }
}
