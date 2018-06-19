using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace STUPBaseStruct
{
 
    public struct ScanCmdPacketStruct
    {
        public short ID;
        public short Sz;
        public byte Cmd;
        public short Count;
        public byte Sum;
    }

    public struct ScanEchoPacketStruct
    {
        public short ID;
        public short Sz;
        public byte Cmd;
        public short Count;
        public byte Sum;
    }

    public struct MachIDCmdPacketStruct
    {
        public short ID;
        public short Sz;
        public byte Cmd;
        public short Count;
        public byte Sum;
    }

    public struct MachIDEchoPacketStruct
    {
        public short ID;
        public short Sz;
        public byte Cmd;
        public short Count;
        public byte ID0;
        public byte Ver1;
        public byte Ver2;
        public short BugFix;
        public byte TypeID;
        public byte SubTypeID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 60)]
        public string UserDef;
        public byte Sum;
    }


    public struct MachConnectCmdPacketStruct
    {
        public short ID;
        public short Sz;
        public byte Cmd;
        public short Count;
        public short DataSz0;
        public byte DataCmd0;
        public byte DataCmd1;
        public int Part;
        public byte Ver1;
        public byte Ver2;
        public short BugFix;
        public byte TypeID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 60)]
        public string Password;
        public byte Sum;
    }


    public struct MachConnectEchoPacketStruct
    {
        public short ID;
        public short Sz;
        public byte Cmd;
        public short Count;
        public short DataSz0;
        public byte DataCmd0;
        public byte DataCmd1;
        public int Part;
        public short Security;
        public short MachID;
        public byte Sum;
    }


    public struct MachDataCmdPacketStruct
    {
        public byte ID0;
        public byte ID1;
        public byte Cmd;
        public short Count;
        public short DataSz0;
        public byte DataCmd0;
        public byte DataCmd1;
        public int Part;
        public int Code;
        public int Len;
        public string DataBuf;
        public byte Sum;

    }



    public static class StructChangeClass
    {

        //struct to byte[]
        public static byte[] StructToBytes(object structObj)
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
        public static object BytesToStruct(byte[] bytes, Type strcutType)
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
