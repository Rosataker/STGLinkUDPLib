using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using STUPBaseStruct;
using STGLinkUDP.STUDPBase;

namespace UDPConsoleServer
{
    class UDPConsoleServer
    {
        public static IPEndPoint _IPEP;
        public static UdpClient _UC;



        static void Main(string[] args)
        {
            STUDPBaseLib.PacketSeting();
            Console.WriteLine("這是伺服器...\n");


            string MachIDCmdPacketSendBytes = string.Empty;


            _IPEP = new IPEndPoint(IPAddress.Any, 0x869C);
            _UC = new UdpClient(_IPEP.Port);
            do
            {
                ScanPacketServer();
                MachIDPacketServer();
                MachConnectPacketServer();
                MachDataPacketServer();

            } while (true);


        }

        private static void ScanPacketServer()
        {
            #region ScanCmdPacketServer
            //1.ID < 2 > 0
            //2.Sz < 2 > 0
            //3.Cmd < 1 > 0x30
            //4.Count < 2 > 自定
            //5.Sum < 1 > CheckSum 之數值，此封包數值總合為 = 0;
            #endregion
            PacketSeting();
            byte[] sendBytes = StructChangeClass.StructToBytes(ScanEchoPack);

            bool _Close = false;
            while (!_Close)
            {
                byte[] buffer = _UC.Receive(ref _IPEP);
                ScanCmdPacketStruct ScanCmdPacket = new ScanCmdPacketStruct();
                ScanCmdPacket = (ScanCmdPacketStruct)StructChangeClass.BytesToStruct(buffer, ScanCmdPacket.GetType());

                Console.WriteLine("ScanCmdPacketServer 接收");              
                if (ScanCmdPacket.Cmd == 0x20)
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
        private static void MachIDPacketServer()
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

            PacketSeting();
            byte[] sendBytes = StructChangeClass.StructToBytes(MachIDEchoPack);



            bool _Close = false;
            while (!_Close)
            {
                byte[] buffer = _UC.Receive(ref _IPEP);
                
                MachIDCmdPacketStruct MachIDCmdPacket = new MachIDCmdPacketStruct();
                MachIDCmdPacket = (MachIDCmdPacketStruct)StructChangeClass.BytesToStruct(buffer, MachIDCmdPacket.GetType());

                Console.WriteLine("MachIDCmdPacketServer 接收");

                if (MachIDCmdPacket.Cmd == 0x21)
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

        private static void MachConnectPacketServer()
        {
            PacketSeting();
            byte[] sendBytes = StructChangeClass.StructToBytes(MachConnectEchoPack);



            bool _Close = false;
            while (!_Close)
            {
                byte[] buffer = _UC.Receive(ref _IPEP);
                MachConnectCmdPacketStruct MachConnectCmdPacket = new MachConnectCmdPacketStruct();
                MachConnectCmdPacket = (MachConnectCmdPacketStruct)StructChangeClass.BytesToStruct(buffer, MachConnectCmdPacket.GetType());


                Console.WriteLine("MachConnectCmdPacketServer 接收");
                if (MachConnectCmdPacket.Cmd == 0x22)
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

        private static void MachDataPacketServer()
        {
            PacketSeting();
            byte[] sendBytes = StructChangeClass.StructToBytes(MachDataEchoPack);

            bool _Close = false;
            while (!_Close)
            {
                byte[] buffer = _UC.Receive(ref _IPEP);
                MachDataCmdPacketStruct MachDataCmdPacket = new MachDataCmdPacketStruct();
                MachDataCmdPacket = (MachDataCmdPacketStruct)StructChangeClass.BytesToStruct(buffer, MachDataCmdPacket.GetType());

                Console.WriteLine("MachDataEchoPacketServer 接收");


                Console.WriteLine("MachDataEchoPacketServer 回傳");

                _UC.Send(sendBytes, sendBytes.Length, _IPEP);
                _Close = true;

            }
        }


        public static ScanEchoPacketStruct ScanEchoPack;
        public static MachIDEchoPacketStruct MachIDEchoPack;
        public static MachConnectEchoPacketStruct MachConnectEchoPack;
        public static MachDataEchoPacketStruct MachDataEchoPack;
        public static void PacketSeting()
        {
            #region ScanEchoPack
            ScanEchoPack.ID = 0x0;
            ScanEchoPack.Sz = 0x0;
            ScanEchoPack.Cmd = 0x30;
            ScanEchoPack.Count = 0x00;
            ScanEchoPack.Sum = 0x00;
            #endregion

            #region MachIDEchoPack
            MachIDEchoPack.ID = 0x0;
            MachIDEchoPack.Sz = 0x43;
            MachIDEchoPack.Cmd = 0x33;
            MachIDEchoPack.Count = 0x0;
            MachIDEchoPack.ID0 = 0x0;
            MachIDEchoPack.Ver1 = 0x04;
            MachIDEchoPack.Ver2 = 0x01;
            MachIDEchoPack.BugFix = 0x01;
            MachIDEchoPack.TypeID = 0x02;
            MachIDEchoPack.SubTypeID = 0x0;
            MachIDEchoPack.UserDef = "MachIDEchoPack Test UserDef";
            MachIDEchoPack.Sum = 0x0;
            #endregion


            #region MachConnectEchoPack
            MachConnectEchoPack.ID = 0x01;
            MachConnectEchoPack.Sz = 0x0C;
            MachConnectEchoPack.Cmd = 0x31;
            MachConnectEchoPack.Count = 0x00;
            MachConnectEchoPack.DataSz0 = 0x04;
            MachConnectEchoPack.DataCmd0 = 0x00;
            MachConnectEchoPack.DataCmd1 = 0x00;
            MachConnectEchoPack.Part = 0x00;
            MachConnectEchoPack.Security = 0x01;
            MachConnectEchoPack.MachID = 0x01;
            MachConnectEchoPack.Sum = 0x00;
            #endregion

            #region MachDataEchoPack
            
            MachDataEchoPack.ID0 = 0x01;
            MachDataEchoPack.ID1 = 0x0;
            MachDataEchoPack.Sz = 0x303;
            MachDataEchoPack.Cmd = 0x01;
            MachDataEchoPack.Count = 0x0;
            MachDataEchoPack.DataSz0 = 0x328;
            MachDataEchoPack.DataCmd0 = 0x50;
            MachDataEchoPack.DataCmd1 = 0x0;
            MachDataEchoPack.Part = 0x0;
            MachDataEchoPack.Code = 0x0;
            MachDataEchoPack.Len = 0x71c;
            MachDataEchoPack.ActctLen = 0x0;

            for (int i = 0; i < 800; i++)
            {
                MachDataEchoPack.DataBuf += i;
            }


            MachDataEchoPack.Sum = 0x0;
            #endregion

        }
    }

}
