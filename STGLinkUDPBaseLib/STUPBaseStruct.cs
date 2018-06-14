using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace STUPBaseStruct
{

    public struct ScanCmdPacketStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] ID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Sz;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] Cmd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Count;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] Sum;
    }

    public struct MachIDCmdPacketStruct
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] ID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Sz;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] Cmd;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Count;    
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public byte[] Sum;
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

    }
   
}
