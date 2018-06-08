using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using STGLinkUDP.STGLinkUDPBase;


namespace STGLinkUDP
{
    public class STGLinkUDPLib : STGLinkUDPBaseLib
    {
        private static string _FILENAME = "STGLinkUDPLib Log.txt";
        public void RunClient(string IP, int Port)
        {
            Open(IP, Port);
            LogHeadCreate(IP, Port);
            ScanCmdPacket(out byte[] ScanCmdPacketResultByte);

            var MachIDCmdPacketDataByte = ScanCmdPacketResultByte;
            if(Connected) MachIDCmdPacket(MachIDCmdPacketDataByte, out byte[] MachIDCmdPacketResultByte);




            Destructor();
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
