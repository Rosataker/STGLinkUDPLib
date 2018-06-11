using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;


namespace STGLinkUDP.STGLinkUDPBase
{
    public class STGLinkUDPBaseLib : ISTGLinkUDPInterface, IScanCmdPacketInterface, IMachIDCmdPacketInterface, IMachConnectCmdPacketInterface, IMachDataCmdPacketInterface
    {
        public UdpClient UC;
        public IPEndPoint IPEP;
        private static STGLinkUDPBaseLib ISTGLinkUDPBaseLib = new STGLinkUDPBaseLib();
        IScanCmdPacketInterface IScanCmdPacketInterface = ISTGLinkUDPBaseLib;
        IMachIDCmdPacketInterface IMachIDCmdPacketInterface = ISTGLinkUDPBaseLib;
        IMachConnectCmdPacketInterface IMachConnectCmdPacketInterface = ISTGLinkUDPBaseLib;
        IMachDataCmdPacketInterface IMachDataCmdPacketInterface = ISTGLinkUDPBaseLib;
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
            string[] TitleAry = new string[] { };

            TitleAry = new string[] {
            "ID","ID"
            ,"Sz","Sz"
            ,"Cmd"
            ,"Count","Count"
            ,"Sum"
            };

            for (int i = 0; i < sendBytes.Length; i++)
            {
                history += TitleAry[i] + "->" + sendBytes[i] + "\r\n";
            }

            LogLoopCreate(history);
        }
        public void ScanCmdPacket(out byte[] ResultByte)
        {
            ResultByte = new byte[] { };
            try
            {
                Byte[] sendBytes = new byte[] {
                     0,0
                    ,0,0
                    ,0x20
                    ,0,4
                    ,0
                };

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

            #region MachIDCmdPacket
            //1.ID < 2 > 0
            //2.Sz < 2 > 0
            //3.Cmd < 1 > 0x21
            //4.Count < 2 > 自定
            //5.Sum < 1 > CheckSum 之數值，此封包數值總合為 = 0;
            #endregion

            ResultByte = new byte[] { };

            try
            {

                if (DataByte[4] == 0x30)
                {
                    Byte[] sendBytes = new byte[] {
                     0,0
                    ,0,0
                    ,0x21
                    ,0,0
                    ,0
                    };
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
                    Byte[] sendBytes = new byte[] {
                     1,1
                    ,0x4A,0x4A
                    ,0x22
                    ,0,0
                    ,0x42,0x42
                    ,0x03
                    ,0
                    ,0,0,0,0
                    ,4
                    ,3
                    ,7,7
                    ,0x10
                    };
                    Array.Resize(ref sendBytes, sendBytes.Length + 61);

                    for (int i = 20; i < sendBytes.Length - 1; i++)
                    {
                        sendBytes[i] = 0x30;
                    }
                    sendBytes[sendBytes.Length - 1] = 0;




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
            #region MachDataCmdPacket
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

                if (DataByte[17] == 1 || DataByte[18] == 1)
                {
                    Byte[] sendBytes = new byte[] {
                     1,1
                    ,0x4A,0x4A
                    ,0x22
                    ,0,0
                    ,0x42,0x42
                    ,0x03
                    ,0
                    ,0,0,0,0
                    ,4
                    ,3
                    ,7,7
                    ,0x10
                    };
                    Array.Resize(ref sendBytes, sendBytes.Length + 61);

                    for (int i = 20; i < sendBytes.Length - 1; i++)
                    {
                        sendBytes[i] = 0x30;
                    }
                    sendBytes[sendBytes.Length - 1] = 0;




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
    }

    interface ISTGLinkUDPInterface
    {
        bool Connected { get; }
        void Open(string IP, int Port);
        void ScanCmdPacket(out byte[] ResultByte);
        void MachIDCmdPacket(byte[] DataByte, out byte[] ResultByte);
        void MachConnectCmdPacket(byte[] DataByte, out byte[] ResultByte);
        void MachDataCmdPacket(byte[] DataByte, out byte[] ResultByte);
        void Destructor();
    }


    interface IScanCmdPacketInterface
    {
        void Log(byte[] sendBytes, string status);
    }
    interface IMachIDCmdPacketInterface
    {
        void Log(byte[] sendBytes, string status);
    }
    interface IMachConnectCmdPacketInterface
    {
        void Log(byte[] sendBytes, string status);
    }
    interface IMachDataCmdPacketInterface
    {
        void Log(byte[] sendBytes, string status);
    }

}
