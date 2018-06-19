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
                ScanEchoPacketStruct ScanEchoPacket = new ScanEchoPacketStruct();
                ScanEchoPacket = (ScanEchoPacketStruct)StructChangeClass.BytesToStruct(ScanCmdPacketResultByte , ScanEchoPacket.GetType());

                Console.WriteLine("ScanEchoPacket.ID->{0}",ScanEchoPacket.ID);
                Console.WriteLine("ScanEchoPacket.Sz->{0}",ScanEchoPacket.Sz);
                Console.WriteLine("ScanEchoPacket.Cmd->{0}",ScanEchoPacket.Cmd);
                Console.WriteLine("ScanEchoPacket.Count->{0}",ScanEchoPacket.Count);
                Console.WriteLine("ScanEchoPacket.Sum->{0}",ScanEchoPacket.Sum);


                ////2            
                //MachIDCmdPacket(ScanCmdPacketResultByte, out byte[] MachIDCmdPacketResultByte);

                ////3
                //MachConnectCmdPacket(MachIDCmdPacketResultByte, out byte[] MachConnectCmdPacketResultByte);

                ////4
                //MachDataCmdPacket(MachConnectCmdPacketResultByte, out byte[] MachDataCmdPacketResultByte);

                Destructor();

                Thread.Sleep(5000);
            } while (true);

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
