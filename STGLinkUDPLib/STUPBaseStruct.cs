﻿using System;
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
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 800)]
        public string DataBuf;
        public byte Sum;

    }

    public struct MachDataEchoPacketStruct
    {
        public byte ID0;
        public byte ID1;
        public short Sz;
        public byte Cmd;
        public short Count;
        public short DataSz0;
        public byte DataCmd0;
        public byte DataCmd1;
        public int Part;
        public int Code;
        public int Len;
        public int ActctLen;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 800)]
        public string DataBuf;
        public byte Sum;
    }


    public enum MACHINE_STATE
    {
        M_NOT_READY = 0, //尚未初始化(RESET)
        M_READY, //準備完成
        M_BUSY, //忙碌中
        M_IDLE, //閒置中
        M_ALARM, //發生(錯誤)警告
        M_FINISH //加工完成
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WIRE_SCODE_DATA//size=50 //WIRE SPARK SCODE
    {
        public ushort OV; //OV:OPEN VAL.(0~15)
        public ushort LP; //LP:OPEN VOL.(0~15)
        public ushort ON; //ON:MACH CURRENT(1~15)
        public ushort OFF; //OFF:OFF TIME(3~16)
        public ushort AN; //AN:AUX. MACH. CURRENT(1~15)
        public ushort AF; //AF:AUX. OFF TIME(3~60)
        public ushort SV; //SV:SERVO VOLTAGE(5~125)
        public ushort FR; //FR:FEEDRATE OVERRIDE(0~250)
        public ushort WF; //WF:WIRE FEEDRATE(0~15)
        public ushort WT; //WT:WIRE TENSION(0~15)
        public ushort FL; //FL:WATER FLOW RATE(0~7)
        public ushort FM; //FM:SERVO FEED MODE(0~3)
        public double FMAX; //MAX FEEDRATE
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I2)]
        public short[] OFT; //oft. array 
        public short RM; //rought
        public short SN; //預留
        public short SF; //預留
        public char NR_DT; //預留
        public char NR_ON; //預留
        public char DT; //預留
        public char AT; //預留
    }

    
    public struct WIRE_PT5DL
    {//size=20
        public int X, Y, Z, U, V;
    }

    public struct WIRE_MMI_INF01
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        char FLAG;
        long DATA_TIME;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 776)]
        char OrtherData;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct WIRE_MMI1_INFO
    {//size=784
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string FLAG;
        public uint DATE_TIME; //系統時間(unit:秒)
        public double GAP; //GAP電壓(unit:V)
        public double FEED; //機台進幾率(unit:mm/min)
        public double HIPWR; //加工時間比
        public double WATER_RESIST; //水阻值
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string MFN; //目前加工主程式
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string FN; //目前加工程式
        public uint BN; //目前加工行
        public uint CUT_TM; //目前已加工時間(unit:ms)
        public uint EST_TM; //預計完成時間(unit:ms)
        public ushort ALARM_CODE; //顯示錯誤號碼        

        public WIRE_PT5DL MP, PGM, G92, LOCAL;//
        //                                      //顯示座標(MP:機械座標, PGM:程式座標, G92:G92座標, LOCAL: 區域座標)
        //                                      //詳見資料結構 typedef struct tagPT5DL

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string MESSAGE; //顯示當前訊息字串
        public ushort SCODE; //當前放電碼
                             //WIRE_SCODE_DATA SDATA; //當前加工資料
                             //詳見typedef struct tagSCODE_DATA

        public WIRE_SCODE_DATA SDATA; //當前加工資料
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.I2)]
        public ushort[] TEMP; //顯示溫度0:下伸臂溫度1:機體溫度2:室內溫度10:冷卻機	環境11:目標溫度//unit:0.01C        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.I2)]
        public ushort[] WATT; //預留


        public ushort MEM_ALARM; //程式工作中的警告號碼
        public ushort MEM_ERROR; //程式工作中的錯誤號碼
        public ushort MEM_STATE; //機台動作(參考MEM_STATE定義)
        public ushort MACH_STATE; //機台狀態(參考MACHINE_STATE)
        public ushort MACH_ALARM; //機台錯誤訊息(參考ERROR.TXT)
        public ushort MAIN_FUNCID; //主視窗位置(參考OP_MODE)
        public ushort SUB_FUNC_PAGE; //子視窗位置,Page1:0, Page2:1
        public ushort FUNC_KEYID; //子視窗的功能鍵
        public ushort MEM_NC; //當前N碼
        public uint IDLE_TM; //機台閒置時間
        public uint ZLOCK_PT; //Z LOCK 座標
        public double AR_ANGLE; //旋轉角度

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6, ArraySubType = UnmanagedType.Struct)]
        public WIRE_PT5DL[] WP; //顯示座標
        //(0:G54座標, 1:G55座標, 2:G56座標, 3:G57座標, 4:G58座標, 5:G59座標)
        //                 詳見資料結構 typedef struct tagPT5DL

        public double TOTAL_LEN; //程式目前加工長度
        public double REST_LEN; //程式剩餘長度
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string COMP_STR; //預留
        public ushort SysStatus; //系統狀態(參考SysStatus定義)
        public long SparkIOCount; //加工開始時數(單位ms)
        public double SquareSpeed; //加工時速度(面積速度mm*mm/min)
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 180)]
        public string resv;
    }


    // MEM_STATE 數值
    public enum MEM_STATE
    {
        MEM_RESET0 = 0, //程式重置狀況。
        MEM_NONE = 1, //程式重置狀況。
        MEM_START0 = 2, //程式等待啟動中。
        MEM_NORMAL = 3, //程式執行中。
        MEM_NORMAL1 = 4, //程式執行中。
        MEM_STOP0 = 5, //程式暫停中。
        MEM_FINISH = 6, //程式加工完成。
        MEM_BACK = 7,//機台後退移動中。
        MEM_AWT_HANDLE_WB = 10,//整理廢線中。
        MEM_AWT_HOLD_WB = 11, //整理廢線暫停。
        MEM_THRU_WIRE = 12, //穿線中。
        MEM_HOLD_THRU_WIRE = 13,//穿線暫停。
        MEM_CUT_WIRE = 14,//剪線中。
        MEM_HOLD_CUT_WIRE = 15, //剪線暫停。
        MEM_SEARCH_M20 = 30, //搜尋M20程式位置。
        MEM_SEARCH_M21 = 31, //搜尋M21程式位置。
        MEM_WAIT_MOVE_IDLE = 32, //程式加工定位中。
        MEM_HOLD_MOVE = 33, //程式加工定位中，暫停移動。
        MEM_RESTART = 40, //再加工搜尋上次中斷位置。
        MEM_RESTART_ABORT = 41, //預留。
        MEM_REHOLE = 42, //預留。
        MEM_REHOLE_ABORT = 43,//預留。
        MEM_NCODE = 50, //搜尋N碼位置中。
        MEM_POWER_DOWN = 51,//系統週邊電源消失
        MEM_WAIT_WATER = 52,//等待工作水漕內的水至高水位時。
        MEM_HOLD_WATER = 53,//在等待工作水漕內的水至高水位時，按STOP鍵暫停。
        MEM_G92_MOVE_OPEN = 80,//在G92命令下，機台移動使極間電壓開路。
        MEM_WAIT_PLC_IN = 81, //等待PLC-IN訊號，PLCType=10-TANK232-I16 PLCType==1:IO232-TB8IO-I8
        MEM_WAIT_THRU_WIRE_WATER_OFF = 82,//在穿線狀態下，等待水漕內的水洩至低水位。
        MEM_WAIT_SLUICE = 83, //M42等待洩水中。
        MEM_RECUT = 90, //模孔再加工搜尋中。
        MEM_RECUT1 = 91 //模孔再加工搜尋中。
    }


    // SysStatus 數值
    public enum SysStatus
    {
        SYS_JOG_NORMAL = 1, //當JOG操作超過5sec，系統判定機台移動中
        SYS_MEM_NORMAL = 2, //當系統在AUTO加工畫面，且正在執行程式加工中，系統判定程式加工中
        SYS_EDIT_NORMAL = 3, //當系統在EDIT編輯畫面，並且按鍵超過5sec，系統判定為編輯中
        SYS_MEM_STOP_NONE = 20, //系統程式未知暫停
        SYS_MEM_STOP_ERROR = 21, //系統程式錯誤暫停
        SYS_MEM_STOP_ALARM = 22, //系統程式警告暫停
        SYS_MEM_STOP_IDLE = 23, //系統程式閒置暫停
        SYS_POWER_OFF = 100, //週邊裝置未通電
        SYS_OPER_IDLE = 101, //系統操作閒置
    }

    //// ERROR 數值
    //unsigned short ALARM_CODE; //顯示錯誤號碼
    //unsigned short MEM_ALARM; //程式工作中的警告號碼
    //unsigned short MEM_ERROR; //程式工作中的錯誤號碼
    //unsigned short MACH_ALARM; //機台錯誤訊息(參考ERROR.TXT)


}
