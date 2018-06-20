using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using STGLinkUDP.STUDPBase;
using STUPBaseStruct;


namespace STGLinkUDP
{
    public class STGLinkUDPLib : STUDPBaseLib
    {
        private static string _FILENAME = "STGLinkUDPLib Log.txt";


        public void RunClient(string IP, int Port)
        {


            do
            {
                Open(IP, Port);
                LogHeadCreate(IP, Port);

                ScanCmdPacket(out byte[] ScanCmdPacketResultByte);
                MachIDCmdPacket(ScanCmdPacketResultByte, out byte[] MachIDCmdPacketResultByte);
                MachConnectCmdPacket(MachIDCmdPacketResultByte, out byte[] MachConnectCmdPacketResultByte);
                MachDataCmdPacket(MachConnectCmdPacketResultByte, out byte[] MachDataCmdPacketResultByte);
                Destructor();

                OpenMachDataPacke(MachDataCmdPacketResultByte);


                


                Thread.Sleep(5000);
            } while (true);

        }

        private static void OpenMachDataPacke(byte[] MachDataCmdPacketResultByte)
        {
            WIRE_MMI1_INFO MMI_INFO = new WIRE_MMI1_INFO();
            MachDataEchoPacketStruct MachDataEchoPack = new MachDataEchoPacketStruct();
            MachDataEchoPack = (MachDataEchoPacketStruct)StructChangeClass.BytesToStruct(MachDataCmdPacketResultByte, MachDataEchoPack.GetType());

            byte[] BeforeDataBuf = Encoding.ASCII.GetBytes(MachDataEchoPack.DataBuf);

            //BeforeDataBuf
            try
            {
                MMI_INFO = (WIRE_MMI1_INFO)StructChangeClass.BytesToStruct(BeforeDataBuf, MMI_INFO.GetType());

                for (int i = 0; i < MMI_INFO.FLAG.Length; i++)
                {
                    Console.WriteLine("=>FLAG: {0} ", MMI_INFO.FLAG[i]);
                }
                
                Console.WriteLine("=>time: {0} sec", MMI_INFO.DATE_TIME);
                //Console.WriteLine("=>GAP: {0} v", MMI_INFO.GAP);
                //Console.WriteLine("=>機台進機率: {0} mm/min ", MMI_INFO.FEED);
                //Console.WriteLine("=>放電時間比: {0} ", MMI_INFO.HIPWR);
                //Console.WriteLine("=>水阻值: {0} ", MMI_INFO.WATER_RESIST);
                //Console.WriteLine("=>目前加工主程式: {0}", MMI_INFO.MFN);
                //Console.WriteLine("=>目前加工程式: {0}", MMI_INFO.FN);
                //Console.WriteLine("=>目前加工行: {0}", MMI_INFO.BN);
                //Console.WriteLine("=>目前已放電時間: {0} ms", MMI_INFO.CUT_TM);
                //Console.WriteLine("=>預計完成時間: {0} ms", MMI_INFO.EST_TM);
                //Console.WriteLine("=>顯示錯誤號碼: {0} ", MMI_INFO.ALARM_CODE);
                //Console.WriteLine("=>當前訊息字串: {0} ", MMI_INFO.MESSAGE);
                //Console.WriteLine("=>當前放電碼: {0} ", MMI_INFO.SCODE);


                //Console.WriteLine("=>下伸臂溫度: {0} ", MMI_INFO.TEMP[0]);
                //Console.WriteLine("=>機體溫度: {0} ", MMI_INFO.TEMP[1]);
                //Console.WriteLine("=>室內溫度: {0} ", MMI_INFO.TEMP[2]);
                //Console.WriteLine("=>冷郤機環境: {0} ", MMI_INFO.TEMP[10]);
                //Console.WriteLine("=>目標溫度: {0} ", MMI_INFO.TEMP[11]);

                //Console.WriteLine("=>機台狀態: {0} ", MMI_INFO.MACH_STATE);
                //Console.WriteLine("=>機台錯誤訊息: {0} ", MMI_INFO.MACH_ALARM);
                //Console.WriteLine("=>程式工作中的警告號碼: {0} ", MMI_INFO.MEM_ALARM);
                //Console.WriteLine("=>程式工作中的錯誤號碼: {0} ", MMI_INFO.MEM_ERROR);

                //Console.WriteLine("=>機台狀態: {0} ", MMI_INFO.MEM_STATE);
                //Console.WriteLine("=>程式目前加工長度: {0} ", MMI_INFO.TOTAL_LEN);
                //Console.WriteLine("=>程式剩餘長度: {0}", MMI_INFO.REST_LEN);
                //Console.WriteLine("=>系統狀態: {0}", MMI_INFO.SysStatus);
                //Console.WriteLine("=>放電開始時數: {0} ms", MMI_INFO.SparkIOCount);
                //Console.WriteLine("=>加工時速度: {0} mm*mm / min", MMI_INFO.SquareSpeed);


            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR]Can't trasnlate message to WIRE_MMI1_INFO format!!\n");
            }




            //byte[] aaa = Encoding.ASCII.GetBytes(MachDataEchoPack.DataBuf);
            //for (int i = 0; i < aaa.Length; i++)
            //{
            //    Console.WriteLine("{0}->{1}",i, Convert.ToChar(aaa[i]));
            //    Thread.Sleep(500);
            //}
            //Console.Read();
        }


        private static void LogHeadCreate(string IP, int Port)
        {

            DateTime Date = DateTime.Now;
            string TodyMillisecond = Date.ToString("yyyy-MM-dd HH:mm:ss");
            File.AppendAllText(_FILENAME, "\r\n");
            File.AppendAllText(_FILENAME, TodyMillisecond + "\r\n");
            File.AppendAllText(_FILENAME, "IP" + "：" + IP + "\r\n");
            File.AppendAllText(_FILENAME, "PORT" + "：" + Port + "\r\n");
            File.AppendAllText(_FILENAME, "\r\n");
        }
    }
}
