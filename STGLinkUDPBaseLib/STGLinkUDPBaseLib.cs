using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;


namespace STGLinkUDP.STGLinkUDPBase
{
    public class STGLinkUDPBaseLib : ISTGLinkUDPInterface
    {
        public UdpClient UC;
        public IPEndPoint IPEP;
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

        public void ScanCmdPacket(out byte[] ResultByte)
        {
            #region ScanCmdPacket
            //1.ID < 2 > 0
            //2.Sz < 2 > 0
            //3.Cmd < 1 > 0x20
            //4.Count < 2 > 自定
            //5.Sum < 1 > Check Sum 之數值，此封包數值總合為 = 0;
            #endregion
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
                ScanCmdPacketLog(sendBytes, "Send");

                ResultByte = UC.Receive(ref IPEP);
                ScanCmdPacketLog(ResultByte, "Result");
            }
            catch (Exception)
            {
                Connected = false;
                Console.WriteLine("ScanCmdPacket connect error . ");
                Console.WriteLine("IP->{0}", _IP);
                Console.WriteLine("PORT->{0}", _PORT);



            }

        }

        private void ScanCmdPacketLog(byte[] sendBytes, string status)
        {
            //1.ID < 2 > 0
            //2.Sz < 2 > 0
            //3.Cmd < 1 > 0x20
            //4.Count < 2 > 自定
            //5.Sum < 1 > Check Sum 之數值，此封包數值總合為 = 0;
            string history = "ScanCmdPacket " + status + " content:  \r\n";
            for (int i = 0; i < sendBytes.Length; i++)
            {
                if(i <= 1)
                {
                    history += "ID->" + sendBytes[i] + "\r\n";
                }
                else if(i <= 3){
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
                    MachIDCmdPacketLog(sendBytes, "Send");


                    ResultByte = UC.Receive(ref IPEP);
                    MachIDCmdPacketLog(ResultByte, "Result");
                }
            }
            catch (Exception)
            {
                byte[] sendBytes = new byte[] { 0 };
                MachIDCmdPacketLog(sendBytes, "Error");
            }
        }

        private void MachIDCmdPacketLog(byte[] sendBytes, string status)
        {


//11.UserDef < 60 > 機台名稱
//12.Sum < 1 > Check Sum 之數值，此封包數值總合為 = 0;
            string history = string.Empty;
            switch (status)
            {
                case "Result":
                    history = "MachIDCmdPacket " + status + " content:  \r\n";
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
                        else if (i >= 14 && i < sendBytes.Length-1)
                        {
                            tempMachineName[i-14] = sendBytes[i];                             
                        }
                        else
                        {
                            history += "UserDef->" + Encoding.UTF8.GetString(tempMachineName) + "\r\n";
                            history += "Sum->" + sendBytes[i] + "\r\n";
                        }

                    }


                    break;
                default:
                    history = "MachIDCmdPacket " + status + " content:  \r\n";
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
                    break;
            }
            LogLoopCreate(history);

        }

        public void MachConnectCmdPacket()
        {

        }

        public void MachDataCmdPacket()
        {

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
        void MachConnectCmdPacket();
        void MachDataCmdPacket();
        void Destructor();
    }



}
