﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using STUPBaseStruct;
using STUDPBase;
using System.Threading;
using System.Runtime.InteropServices;
using System.Globalization;




namespace STGLinkUDP
{
    public class STGLinkUDPLib : STUDPBaseLib
    {
        private string _FILENAME { get; set; }
        private ScanCmdPacketStruct ScanCmdPack;
        private MachIDCmdPacketStruct MachIDCmdPack;
        private MachConnectCmdPacketStruct MachConnectCmdPack;
        private MachDataCmdPacketStruct MachDataCmdPack;

        public STGLinkUDPLib(IDictionary<string, string> configDic) : base(configDic) { _FILENAME = "STGLinkUDPLib Log.txt"; }

        public void RunClient()
        {
            PacketSeting();


            int i = 1;
            do
            {
                Open();
                _UdpClient.Client.ReceiveTimeout = _TIMEOUT_MS;

                LogHeadCreate(_IP, _PORT);
                Console.WriteLine("run->{0}", i);

                ScanCmdPacket(out byte[] ScanCmdPacketResultByte);
                if (Connected)
                {
                    MachIDCmdPacket(ScanCmdPacketResultByte, out byte[] MachIDCmdPacketResultByte);
                    if (Connected)
                    {
                        MachConnectCmdPacket(MachIDCmdPacketResultByte, out byte[] MachConnectCmdPacketResultByte);
                        if (Connected)
                        {
                            MachDataCmdPacket(MachConnectCmdPacketResultByte, out byte[] MachDataCmdPacketResultByte);
                            if (Connected)
                            {
                                OpenMachDataPacke(MachDataCmdPacketResultByte);
                            }
                        }
                    }
                }


                
                

                



                Thread.Sleep(5000);
                Destructor();
                i++;
            } while (true);

        }


        public void ScanCmdPacket(out byte[] ResultByte)
        {
            ResultByte = new byte[] { };
            try
            {

                byte[] sendBytes = StructToBytes(ScanCmdPack);

                
                _UdpClient.Send(sendBytes, sendBytes.Length, _IPEndPoint);
                ResultByte = _UdpClient.Receive(ref _IPEndPoint);


            }
            catch (Exception)
            {
                Connected = false;
                Console.WriteLine("ScanCmdPacket connect error . ");
                Console.WriteLine("IP->{0}", _IP);
                Console.WriteLine("PORT->{0}", _PORT);



            }
        }


        public void MachIDCmdPacket(byte[] DataByte, out byte[] ResultByte)
        {
            ResultByte = new byte[] { };
            try
            {
                //ScanEchoPacketStruct ScanEchoPacket = new ScanEchoPacketStruct();
                //ScanEchoPacket = (ScanEchoPacketStruct)BytesToStruct(DataByte, ScanEchoPacket.GetType());


                //PacketSeting();
                byte[] sendBytes = StructToBytes(MachIDCmdPack);


                _UdpClient.Send(sendBytes, sendBytes.Length, _IPEndPoint);
                ResultByte = _UdpClient.Receive(ref _IPEndPoint);


            }
            catch (SocketException)
            {
                Console.WriteLine("MachIDCmdPacket send error");
                Connected = false;
            }
        }


        public void MachConnectCmdPacket(byte[] DataByte, out byte[] ResultByte)
        {
            ResultByte = new byte[] { };

            try
            {
                //MachIDEchoPacketStruct MachIDEchoPacket = new MachIDEchoPacketStruct();
                //MachIDEchoPacket = (MachIDEchoPacketStruct)BytesToStruct(DataByte, MachIDEchoPacket.GetType());

                byte[] sendBytes = StructToBytes(MachConnectCmdPack);


                _UdpClient.Send(sendBytes, sendBytes.Length, _IPEndPoint);
                ResultByte = _UdpClient.Receive(ref _IPEndPoint);


            }
            catch (Exception)
            {
                Console.WriteLine("MachConnectCmdPacket send error");
                Connected = false;
            }


        }


        public void MachDataCmdPacket(byte[] DataByte, out byte[] ResultByte)
        {
            ResultByte = new byte[] { };

            try
            {
                MachConnectEchoPacketStruct MachConnectEchoPack = new MachConnectEchoPacketStruct();
                MachConnectEchoPack = (MachConnectEchoPacketStruct)BytesToStruct(DataByte, MachConnectEchoPack.GetType());

                MachDataCmdPack.ID0 = Convert.ToByte(MachConnectEchoPack.MachID);
                MachDataCmdPack.Cmd = _Cmd;
                MachDataCmdPack.Code = _Code;

                byte[] sendBytes = StructToBytes(MachDataCmdPack);

                _UdpClient.Send(sendBytes, sendBytes.Length, _IPEndPoint);
                ResultByte = _UdpClient.Receive(ref _IPEndPoint);

            }
            catch (Exception)
            {
                Console.WriteLine("MachDataCmdPacket send error");
                Connected = false;
            }
        }

        private void OpenMachDataPacke(byte[] MachDataCmdPacketResultByte)
        {


            WIRE_MMI1_INFO MMI_INFO = new WIRE_MMI1_INFO();
            MachDataEchoPacketStruct MachDataEchoPack = new MachDataEchoPacketStruct();
            MachDataEchoPack = (MachDataEchoPacketStruct)BytesToStruct(MachDataCmdPacketResultByte, MachDataEchoPack.GetType());

            byte[] BeforeDataBuf = Encoding.UTF8.GetBytes(MachDataEchoPack.DataBuf);

            try
            {
                MMI_INFO = (WIRE_MMI1_INFO)BytesToStruct(BeforeDataBuf, MMI_INFO.GetType());

                string history = string.Empty;

                history += "=>time: " + MMI_INFO.DATE_TIME + " sec \r\n";
                history += "=>GAP: " + MMI_INFO.GAP + " v \r\n";
                history += "=>機台進機率: " + MMI_INFO.FEED + " mm/min \r\n";
                history += "=>放電時間比: " + MMI_INFO.HIPWR + "\r\n";
                history += "=>水阻值: " + MMI_INFO.WATER_RESIST + "\r\n";
                history += "=>目前加工主程式: " + MMI_INFO.MFN + "\r\n";
                history += "=>目前加工程式: " + MMI_INFO.FN + "\r\n";
                history += "=>目前加工行: " + MMI_INFO.BN + "\r\n";
                history += "=>目前已放電時間: " + MMI_INFO.CUT_TM + " ms \r\n";
                history += "=>預計完成時間: " + MMI_INFO.EST_TM + " ms \r\n";
                history += "=>顯示錯誤號碼: " + MMI_INFO.ALARM_CODE + "  \r\n";
                history += "=>當前訊息字串: " + MMI_INFO.MESSAGE + "  \r\n";
                history += "=>當前放電碼: " + MMI_INFO.SCODE + "  \r\n";

                history += "=>下伸臂溫度: " + MMI_INFO.TEMP[0] + "  \r\n";
                history += "=>機體溫度: " + MMI_INFO.TEMP[1] + "  \r\n";
                history += "=>室內溫度: " + MMI_INFO.TEMP[2] + "  \r\n";

                history += "=>冷郤機環境: " + MMI_INFO.TEMP[10] + "  \r\n";
                history += "=>目標溫度: " + MMI_INFO.TEMP[11] + "  \r\n";

                history += "=>機台狀態: " + MMI_INFO.MACH_STATE + "  \r\n";
                history += "=>機台錯誤訊息: " + MMI_INFO.MACH_ALARM + "  \r\n";

                history += "=>程式工作中的警告號碼: " + MMI_INFO.MEM_ALARM + "  \r\n";
                history += "=>程式工作中的錯誤號碼: " + MMI_INFO.MEM_ERROR + "  \r\n";

                history += "=>機台狀態: " + MMI_INFO.MEM_STATE + "  \r\n";
                history += "=>程式目前加工長度: " + MMI_INFO.TOTAL_LEN + "  \r\n";

                history += "=>程式剩餘長度: " + MMI_INFO.REST_LEN + "  \r\n";
                history += "=>系統狀態: " + MMI_INFO.SysStatus + "  \r\n";

                history += "=>放電開始時數: " + MMI_INFO.SparkIOCount + "  ms \r\n";
                history += "=>加工時速度: " + MMI_INFO.SquareSpeed + " mm*mm / min \r\n";

                Console.WriteLine(history);
                LogLoopCreate(history);
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR]Can't trasnlate message to WIRE_MMI1_INFO format!!\n");
            }


        }

        private void LogHeadCreate(string IP, int Port)
        {

            DateTime Date = DateTime.Now;
            string TodyMillisecond = Date.ToString("yyyy-MM-dd HH:mm:ss");
            File.AppendAllText(_FILENAME, "\r\n");
            File.AppendAllText(_FILENAME, TodyMillisecond + "\r\n");
            File.AppendAllText(_FILENAME, "IP" + "：" + IP + "\r\n");
            File.AppendAllText(_FILENAME, "PORT" + "：" + Port + "\r\n");
            File.AppendAllText(_FILENAME, "\r\n");
        }

        private void LogLoopCreate(string history)
        {
            File.AppendAllText(_FILENAME, history + "\r\n");
        }


        /// <summary>
        ///     設定
        /// </summary>
        public void PacketSeting()
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
            MachDataCmdPack.DataBuf = "";
            MachDataCmdPack.Sum = 0x0;
            #endregion


        }


        //struct to byte[]
        private byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);

            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structObj, buffer, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        //byte[] to struct
        private object BytesToStruct(byte[] bytes, Type strcutType)
        {
            int size = Marshal.SizeOf(strcutType);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                return Marshal.PtrToStructure(buffer, strcutType);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }

}
