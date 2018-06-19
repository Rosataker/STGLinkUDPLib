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
            switch (status)
            {
                case "Result":
                    history = Title + status + " content:  \r\n";
                    byte[] tempMachineName = new byte[60];
                    for (int i = 0; i < sendBytes.Length; i++)
                    {
                        if (i <= 1)
                        {
                            history += "ID->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 3)
                        {
                            history += "Sz->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 4)
                        {
                            history += "Cmd->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 6)
                        {
                            history += "Count->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 7)
                        {
                            history += "ID0->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 8)
                        {
                            history += "Ver1->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 9)
                        {
                            history += "Ver2->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 11)
                        {
                            history += "BugFix->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 12)
                        {
                            history += "TypeID->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 13)
                        {
                            history += "SubTypeID->" + sendBytes[i] + "\r\n";
                        }
                        else if (i >= 14 && i < sendBytes.Length - 1)
                        {
                            tempMachineName[i - 14] = sendBytes[i];
                        }
                        else
                        {
                            history += "UserDef->" + Encoding.UTF8.GetString(tempMachineName) + "\r\n";
                            history += "Sum->" + sendBytes[i] + "\r\n";
                        }
                    }

                    Console.WriteLine(Title + status + " Log save ");
                    break;
                default:
                    history = Title + status + " content:  \r\n";
                    for (int i = 0; i < sendBytes.Length; i++)
                    {
                        if (i <= 1)
                        {
                            history += "ID->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 3)
                        {
                            history += "Sz->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 4)
                        {
                            history += "Cmd->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 6)
                        {
                            history += "Count->" + sendBytes[i] + "\r\n";
                        }
                        else
                        {
                            history += "Sum->" + sendBytes[i] + "\r\n";
                        }

                    }

                    Console.WriteLine(Title + status + " Log save ");
                    break;
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
                    IMachIDCmdPacketInterface.Log(sendBytes, "Send");


                    ResultByte = UC.Receive(ref IPEP);
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
            switch (status)
            {
                case "Result":
                    history = Title + status + " content:  \r\n";
                    byte[] tempMachineName = new byte[60];
                    for (int i = 0; i < sendBytes.Length; i++)
                    {
                        if (i <= 1)
                        {
                            history += "ID->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 3)
                        {
                            history += "Sz->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 4)
                        {
                            history += "Cmd->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 6)
                        {
                            history += "Count->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 7)
                        {
                            history += "ID0->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 8)
                        {
                            history += "Ver1->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 9)
                        {
                            history += "Ver2->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 11)
                        {
                            history += "BugFix->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 12)
                        {
                            history += "TypeID->" + sendBytes[i] + "\r\n";
                        }
                        else if (i <= 13)
                        {
                            history += "SubTypeID->" + sendBytes[i] + "\r\n";
                        }
                        else if (i >= 14 && i < sendBytes.Length - 1)
                        {
                            tempMachineName[i - 14] = sendBytes[i];
                        }
                        else
                        {
                            history += "UserDef->" + Encoding.UTF8.GetString(tempMachineName) + "\r\n";
                            history += "Sum->" + sendBytes[i] + "\r\n";
                        }
                    }

                    Console.WriteLine(Title + status + " Log save ");
                    break;
                default:
                    history = Title + status + " content:  \r\n";
                    #region Title Set
                    string[] TitleAry = new string[] {
                            "ID","ID"
                            ,"Sz","Sz"
                            ,"Cmd"
                            ,"Count","Count"
                            ,"DataSz0","DataSz0"
                            ,"DataCmd0"
                            ,"DataCmd1"
                            ,"Part","Part","Part","Part"
                            ,"Ver1"
                            ,"Ver2"
                            ,"BugFix","BugFix"
                            ,"TypeID"
                        };
                    Array.Resize(ref TitleAry, TitleAry.Length + 61);
                    for (int i = 20; i < TitleAry.Length - 1; i++)
                    {
                        TitleAry[i] = "Password";
                    }
                    TitleAry[TitleAry.Length - 1] = "Sum";
                    #endregion


                    for (int i = 0; i < sendBytes.Length; i++)
                    {
                        history += TitleAry[i] + "->" + sendBytes[i] + "\r\n";
                    }

                    Console.WriteLine(Title + status + " Log save ");
                    break;
            }
            LogLoopCreate(history);
        }
        public void MachConnectCmdPacket(byte[] DataByte, out byte[] ResultByte)
        {
            ResultByte = new byte[] { };
            #region MachConnectCmdPacket
            //1.ID < 2 > 1 非零辨識碼
            //2.Sz < 2 > 0x4A 封包長度
            //3.Cmd < 1 > 0x22 連線要求。
            //4.Count < 2 > 自定
            //5.DataSz0 < 2 > 0x42 封包長度[< Ver1 > +< Ver2 > +…+< Password >]
            //6.DataCmd0 < 1 > 0x03
            //7.DataCmd1 < 1 > 0
            //8.Part < 4 > 0
            //9.Ver1 < 1 > 4
            //10.Ver2 < 1 > 3
            //11.BugFix < 2 > 7
            //12.TypeID < 1 > 0x10 Win32
            //13.Password < 60 > “0000” 密碼字串“0000”
            //14.Sum < 1 > Check Sum 之數值，此封包數值總合為 = 0
            #endregion

            try
            {
                if (DataByte[4] == 0x33)
                {
                    PacketSeting();
                    byte[] sendBytes = StructChangeByte.MachConnectCmdPack(MachConnectCmdPack);




                    UC.Send(sendBytes, sendBytes.Length, IPEP);
                    IMachConnectCmdPacketInterface.Log(sendBytes, "Send");


                    ResultByte = UC.Receive(ref IPEP);
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
            string[] TitleAry = new string[] { };
            switch (status)
            {
                case "Result":

                    history = Title + status + " content:  \r\n";
                    TitleAry = new string[] {
                            "ID0"
                            ,"ID1"
                            ,"Sz","Sz"
                            ,"Cmd"
                            ,"Count","Count"
                            ,"DataSz0","DataSz0"
                            ,"DataCmd0"
                            ,"DataCmd1"
                            ,"Part","Part","Part","Part"
                            ,"Code","Code","Code","Code"
                            ,"Len","Len","Len","Len"
                            ,"ActctLen","ActctLen","ActctLen","ActctLen"
                        };
                    Array.Resize(ref TitleAry, TitleAry.Length + 801);
                    TitleAry[TitleAry.Length - 1] = "Sum";

                    for (int i = 0; i < sendBytes.Length; i++)
                    {
                        history += TitleAry[i] + "->" + sendBytes[i] + "\r\n";
                    }

                    Console.WriteLine(Title + status + " Log save ");
                    break;
                default:

                    history = Title + status + " content:  \r\n";
                    #region Title Set
                    TitleAry = new string[] {
                            "ID0"
                            ,"ID1"
                            ,"Cmd"
                            ,"Count","Count"
                            ,"DataSz0","DataSz0"
                            ,"DataCmd0"
                            ,"DataCmd1"
                            ,"Part","Part","Part","Part"
                            ,"Code","Code","Code","Code"
                            ,"Len","Len","Len","Len"
                        };
                    Array.Resize(ref TitleAry, TitleAry.Length + 801);
                    TitleAry[TitleAry.Length - 1] = "Sum";
                    #endregion


                    for (int i = 0; i < sendBytes.Length; i++)
                    {
                        history += TitleAry[i] + "->" + sendBytes[i] + "\r\n";
                    }

                    Console.WriteLine(Title + status + " Log save ");
                    break;
            }
            LogLoopCreate(history);
        }
        public void MachDataCmdPacket(byte[] DataByte, out byte[] ResultByte)
        {
            ResultByte = new byte[] { };


            try
            {
                if (DataByte[17] == 1 || DataByte[18] == 1)
                {
                    PacketSeting();
                    byte[] sendBytes = StructChangeByte.MachDataCmdPack(MachDataCmdPack);


                    UC.Send(sendBytes, sendBytes.Length, IPEP);
                    IMachDataCmdPacketInterface.Log(sendBytes, "Send");


                    ResultByte = UC.Receive(ref IPEP);
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
            ScanCmdPack.ID = 0x0;
            ScanCmdPack.Sz = 0x0;
            ScanCmdPack.Cmd = 0x20;
            ScanCmdPack.Count = 0x0;
            ScanCmdPack.Sum = 0x0;
            #endregion


            #region MachIDCmdPack
            MachIDCmdPack.ID = 0x0;
            MachIDCmdPack.Sz = 0x0;
            MachIDCmdPack.Cmd = 0x21;
            MachIDCmdPack.Count = 0x0;
            MachIDCmdPack.Sum = 0x0;
            #endregion


            #region MachConnectCmdPack
            MachConnectCmdPack.ID = new byte[] { 1, 1 };
            MachConnectCmdPack.Sz = new byte[] { 0x4A, 0 };
            MachConnectCmdPack.Cmd = new byte[] { 0x22 };
            MachConnectCmdPack.Count = new byte[] { 0, 0 };
            MachConnectCmdPack.DataSz0 = new byte[] { 0x42, 0 };
            MachConnectCmdPack.DataCmd0 = new byte[] { 0x03 };
            MachConnectCmdPack.DataCmd1 = new byte[] { 0 };
            MachConnectCmdPack.Part = new byte[] { 0, 0, 0, 0 };
            MachConnectCmdPack.Ver1 = new byte[] { 4 };
            MachConnectCmdPack.Ver2 = new byte[] { 3 };
            MachConnectCmdPack.BugFix = new byte[] { 7, 0 };
            MachConnectCmdPack.TypeID = new byte[] { 0x10 };
            MachConnectCmdPack.Password = new byte[] { 0x30, 0x30, 0x30, 0x30 };
            Array.Resize(ref MachConnectCmdPack.Password, MachConnectCmdPack.Password.Length + 56);
            MachConnectCmdPack.Sum = new byte[] { 0 };
            #endregion

            #region MachDataCmdPack
            MachDataCmdPack.ID0 = new byte[] { 1 };
            MachDataCmdPack.ID1 = new byte[] { 0 };
            MachDataCmdPack.Cmd = new byte[] { 0x01 };
            MachDataCmdPack.Count = new byte[] { 0, 0 };
            MachDataCmdPack.DataSz0 = new byte[] { 0x32, 0x08 };
            MachDataCmdPack.DataCmd0 = new byte[] { 0x50 };
            MachDataCmdPack.DataCmd1 = new byte[] { 0 };
            MachDataCmdPack.Part = new byte[] { 0, 0, 0, 0 };
            MachDataCmdPack.Code = new byte[] { 0x7A, 0x01, 0, 0 };
            MachDataCmdPack.Len = new byte[] { 0x32, 0x00 };
            MachDataCmdPack.DataBuf = new byte[] { };
            Array.Resize(ref MachDataCmdPack.DataBuf, MachDataCmdPack.DataBuf.Length + 800);
            MachDataCmdPack.Sum = new byte[] { 0 };
            #endregion


        }
    }


    /// <summary>
    ///     把STUPBaseStruct的struct轉成可送到UDP的格式
    /// </summary>
    public class StructChangeByte
    {
     

        public static byte[] MachConnectCmdPack(MachConnectCmdPacketStruct ChgStruct)
        {
            List<byte> retByteList = new List<byte>();
            retByteList.AddRange(ChgStruct.ID);
            retByteList.AddRange(ChgStruct.Sz);
            retByteList.AddRange(ChgStruct.Cmd);
            retByteList.AddRange(ChgStruct.Count);
            retByteList.AddRange(ChgStruct.DataSz0);
            retByteList.AddRange(ChgStruct.DataCmd0);
            retByteList.AddRange(ChgStruct.DataCmd1);
            retByteList.AddRange(ChgStruct.Part);
            retByteList.AddRange(ChgStruct.Ver1);
            retByteList.AddRange(ChgStruct.Ver2);
            retByteList.AddRange(ChgStruct.BugFix);
            retByteList.AddRange(ChgStruct.TypeID);
            retByteList.AddRange(ChgStruct.Password);
            retByteList.AddRange(ChgStruct.Sum);
            byte[] retByte = retByteList.ToArray();
            return retByte;
        }
        public static byte[] MachDataCmdPack(MachDataCmdPacketStruct ChgStruct)
        {
            List<byte> retByteList = new List<byte>();
            retByteList.AddRange(ChgStruct.ID0);
            retByteList.AddRange(ChgStruct.ID1);
            retByteList.AddRange(ChgStruct.Cmd);
            retByteList.AddRange(ChgStruct.Count);
            retByteList.AddRange(ChgStruct.DataSz0);
            retByteList.AddRange(ChgStruct.DataCmd0);
            retByteList.AddRange(ChgStruct.DataCmd1);
            retByteList.AddRange(ChgStruct.Part);
            retByteList.AddRange(ChgStruct.Code);
            retByteList.AddRange(ChgStruct.Len);
            retByteList.AddRange(ChgStruct.DataBuf);
            retByteList.AddRange(ChgStruct.Sum);
            byte[] retByte = retByteList.ToArray();
            return retByte;
        }



 
    }
}
