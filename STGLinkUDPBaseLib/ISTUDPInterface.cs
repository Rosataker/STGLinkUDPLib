using System;
using System.Collections.Generic;
using System.Text;

namespace ISTUDP
{
    public interface ISTUDPInterface
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
