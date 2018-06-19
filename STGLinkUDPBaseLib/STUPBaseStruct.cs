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
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public char UserDef;
        public byte Sum;
    }

    
    public struct MachConnectCmdPacketStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] ID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Sz;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] Cmd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] DataSz0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] DataCmd0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] DataCmd1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Part;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] Ver1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] Ver2;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] BugFix;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] TypeID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public byte[] Password;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] Sum;
    }

    public struct MachDataCmdPacketStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] ID0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] ID1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] Cmd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] DataSz0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] DataCmd0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] DataCmd1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Part;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Code;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Len;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 800)]
        public byte[] DataBuf;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] Sum;

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
