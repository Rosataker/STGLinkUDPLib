using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using ISTUDP;
using System.Runtime.InteropServices;
using STUPBaseStruct;
using System.Globalization;

namespace STGLinkUDP.STUDPBase
{
    public class STUDPBaseLib : ISTUDPInterface, IScanCmdPacketInterface, IMachIDCmdPacketInterface, IMachConnectCmdPacketInterface, IMachDataCmdPacketInterface
    {
        public UdpClient UC;
        public IPEndPoint IPEP;
        public static int _Code;
        public static int _Cmd;
        private static STUDPBaseLib ISTUDPBaseLib = new STUDPBaseLib();
        IScanCmdPacketInterface IScanCmdPacketInterface = ISTUDPBaseLib;
        IMachIDCmdPacketInterface IMachIDCmdPacketInterface = ISTUDPBaseLib;
        IMachConnectCmdPacketInterface IMachConnectCmdPacketInterface = ISTUDPBaseLib;
        IMachDataCmdPacketInterface IMachDataCmdPacketInterface = ISTUDPBaseLib;
        public static ScanCmdPacketStruct ScanCmdPack;
        public static MachIDCmdPacketStruct MachIDCmdPack;
        public static MachConnectCmdPacketStruct MachConnectCmdPack;
        public static MachDataCmdPacketStruct MachDataCmdPack;

        private static string _IP { get; set; }
        private static int _PORT { get; set; }

        public bool Connected { get; set; }

        public void Open(string IP, int Port)
        {

            Connected = true;
            _IP = IP;
            _PORT = Port;
            IPEP = new IPEndPoint(IPAddress.Parse(IP), Port);
            UC = new UdpClient();
        }

        void IScanCmdPacketInterface.Log(byte[] sendBytes, string status)
        {
            #region ScanCmdPacketSet
            //1.ID < 2 > 0
            //2.Sz < 2 > 0
            //3.Cmd < 1 > 0x20
            //4.Count < 2 > 自定
            //5.Sum < 1 > Check Sum 之數值，此封包數值總合為 = 0;
            #endregion

            string history = "ScanCmdPacket " + status + " content:  \r\n";
            foreach (byte item in sendBytes)
            {
                history += item + "\r\n";
            }

            LogLoopCreate(history);
        }
        public void ScanCmdPacket(out byte[] ResultByte)
        {
            ResultByte = new byte[] { };
            try
            {
                PacketSeting();

                byte[] sendBytes = StructChangeClass.StructToBytes(ScanCmdPack);


                UC.Send(sendBytes, sendBytes.Length, IPEP);
                IScanCmdPacketInterface.Log(sendBytes, "Send");

                ResultByte = UC.Receive(ref IPEP);
                IScanCmdPacketInterface.Log(ResultByte, "Result");


            }
            catch (Exception)
            {
                Connected = false;
                Console.WriteLine("ScanCmdPacket connect error . ");
                Console.WriteLine("IP->{0}", _IP);
                Console.WriteLine("PORT->{0}", _PORT);



            }
        }

        void IMachIDCmdPacketInterface.Log(byte[] sendBytes, string status)
        {
            string Title = "MachIDCmdPacket ";
            string history = string.Empty;
            history = Title + status + " content:  \r\n";
            foreach (byte item in sendBytes)
            {
                history += item + "\r\n";
            }

            LogLoopCreate(history);
        }

        public void MachIDCmdPacket(byte[] DataByte, out byte[] ResultByte)
        {
            ResultByte = new byte[] { };
            try
            {                
                ScanEchoPacketStruct ScanEchoPacket = new ScanEchoPacketStruct();
                ScanEchoPacket = (ScanEchoPacketStruct)StructChangeClass.BytesToStruct(DataByte, ScanEchoPacket.GetType());

                if (ScanEchoPacket.Cmd == 0x30)
                {
                    PacketSeting();
                    byte[] sendBytes = StructChangeClass.StructToBytes(MachIDCmdPack);



                    UC.Send(sendBytes, sendBytes.Length, IPEP);
                    ResultByte = UC.Receive(ref IPEP);

                    IMachIDCmdPacketInterface.Log(sendBytes, "Send");
                    IMachIDCmdPacketInterface.Log(ResultByte, "Result");
                }
            }
            catch (Exception)
            {
                Connected = false;
                byte[] sendBytes = new byte[] { 0 };
                IMachIDCmdPacketInterface.Log(sendBytes, "Error");
            }
        }

        void IMachConnectCmdPacketInterface.Log(byte[] sendBytes, string status)
        {
            string Title = "MachConnectCmdPacket ";
            string history = string.Empty;
            history = Title + status + " content:  \r\n";
            foreach (byte item in sendBytes)
            {
                history += item + "\r\n";
            }

            LogLoopCreate(history);
        }
        public void MachConnectCmdPacket(byte[] DataByte, out byte[] ResultByte)
        {
            ResultByte = new byte[] { };

            try
            {
                MachIDEchoPacketStruct MachIDEchoPacket = new MachIDEchoPacketStruct();
                MachIDEchoPacket = (MachIDEchoPacketStruct)StructChangeClass.BytesToStruct(DataByte, MachIDEchoPacket.GetType());

                if (MachIDEchoPacket.Cmd == 0x33)
                {
                    PacketSeting();
                    byte[] sendBytes = StructChangeClass.StructToBytes(MachConnectCmdPack);



                    UC.Send(sendBytes, sendBytes.Length, IPEP);
                    ResultByte = UC.Receive(ref IPEP);


                    IMachConnectCmdPacketInterface.Log(sendBytes, "Send");
                    IMachConnectCmdPacketInterface.Log(ResultByte, "Result");
                }
            }
            catch (Exception)
            {
                Connected = false;
                byte[] sendBytes = new byte[] { 0 };
                IMachConnectCmdPacketInterface.Log(sendBytes, "Error");
            }


        }

        void IMachDataCmdPacketInterface.Log(byte[] sendBytes, string status)
        {
            string Title = "MachDataCmdPacket ";
            string history = string.Empty;
            history = Title + status + " content:  \r\n";
            foreach (byte item in sendBytes)
            {
                history += item + "\r\n";
            }

            LogLoopCreate(history);
        }

        
        public void MachDataCmdPacket(byte[] DataByte, out byte[] ResultByte)
        {
            ResultByte = new byte[] { };

            try
            {
                MachConnectEchoPacketStruct MachConnectEchoPack = new MachConnectEchoPacketStruct();
                MachConnectEchoPack = (MachConnectEchoPacketStruct)StructChangeClass.BytesToStruct(DataByte, MachConnectEchoPack.GetType());

                if (MachConnectEchoPack.Cmd == 0x31 && MachConnectEchoPack.MachID == 0x01 )
                {
                    PacketSeting();
                    byte[] sendBytes = StructChangeClass.StructToBytes(MachDataCmdPack);




                    UC.Send(sendBytes, sendBytes.Length, IPEP);
                    ResultByte = UC.Receive(ref IPEP);

                    IMachDataCmdPacketInterface.Log(sendBytes, "Send");
                    IMachDataCmdPacketInterface.Log(ResultByte, "Result");
                }
            }
            catch (Exception)
            {
                Connected = false;
                byte[] sendBytes = new byte[] { 0 };
                IMachDataCmdPacketInterface.Log(sendBytes, "Error");
            }
        }



        private static string _FILENAME = "STGLinkUDPLib Log.txt";
        private static void LogLoopCreate(string history)
        {
            File.AppendAllText(_FILENAME, history + "\r\n");
        }

        public void Destructor()
        {
            Connected = false;
            UC.Close();
        }


        /// <summary>
        ///     設定
        /// </summary>
        public static void PacketSeting()
        {
            #region ScanCmdPack
            ScanCmdPack.ID = 0x00;
            ScanCmdPack.Sz = 0x00;
            ScanCmdPack.Cmd = 0x20;
            ScanCmdPack.Count = 0x00;
            ScanCmdPack.Sum = 0x00;
            #endregion


            #region MachIDCmdPack
            MachIDCmdPack.ID = 0x00;
            MachIDCmdPack.Sz = 0x00;
            MachIDCmdPack.Cmd = 0x21;
            MachIDCmdPack.Count = 0x00;
            MachIDCmdPack.Sum = 0x00;
            #endregion

            #region MachConnectCmdPack
            MachConnectCmdPack.ID = 0x01;
            MachConnectCmdPack.Sz = 0x4A;
            MachConnectCmdPack.Cmd = 0x22;
            MachConnectCmdPack.Count = 0x00;
            MachConnectCmdPack.DataSz0 = 0x42;
            MachConnectCmdPack.DataCmd0 = 0x03;
            MachConnectCmdPack.DataCmd1 = 0x00;
            MachConnectCmdPack.Part = 0x00;
            MachConnectCmdPack.Ver1 = 0x04;
            MachConnectCmdPack.Ver2 = 0x03;
            MachConnectCmdPack.BugFix = 0x07;
            MachConnectCmdPack.TypeID = 0x10;
            MachConnectCmdPack.Password = "0000";
            MachConnectCmdPack.Sum = 0x00;
            #endregion
            //            封包名稱 Byte 數 數值 備註
            //1.ID0 < 1 > 3.b - 10 需與機台回應的 3.b - 10 數值相同
            //2.ID1 < 1 > 自定
            //3.Cmd < 1 > 0x01
            //4.Count < 2 > 自定
            //5.DataSz0 < 2 > 0x328 封包長度[< Code > +< Len > +< DataBuf >]
            //6.DataCmd0 < 1 > 0x50
            //7.DataCmd1 < 1 > 0
            //8.Part < 4 > 0
            //9.Code < 4 > 深孔請用 0x7A1
            //10.Len < 4 > 0x320
            //11.DataBuf < 800 >
            //12.Sum < 1 > Check Sum 之數值，此封包數值總合為 = 0

            #region MachDataCmdPack
            MachDataCmdPack.ID0 = 0x01;
            MachDataCmdPack.ID1 = 0x00;
            MachDataCmdPack.Cmd = 0x01;
            MachDataCmdPack.Count = 0x00;
            MachDataCmdPack.DataSz0 = 0x328;
            MachDataCmdPack.DataCmd0 = 0x50;
            MachDataCmdPack.DataCmd1 = 0x0;
            MachDataCmdPack.Part = 0x0;
            MachDataCmdPack.Code = 0x7A1;
            MachDataCmdPack.Len = 0x320;
            for (int i = 0; i < 800; i++)
            {
                MachDataCmdPack.DataBuf += i;
            }            
            MachDataCmdPack.Sum = 0x0;
            #endregion


        }
    }


}
