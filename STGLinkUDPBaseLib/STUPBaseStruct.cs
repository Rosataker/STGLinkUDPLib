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

    public struct WIRE_SCODE_DATA//size=50 //WIRE SPARK SCODE
    {
        public short OV; //OV:OPEN VAL.(0~15)
        public short LP; //LP:OPEN VOL.(0~15)
        public short ON; //ON:MACH CURRENT(1~15)
        public short OFF; //OFF:OFF TIME(3~16)
        public short AN; //AN:AUX. MACH. CURRENT(1~15)
        public short AF; //AF:AUX. OFF TIME(3~60)
        public short SV; //SV:SERVO VOLTAGE(5~125)
        public short FR; //FR:FEEDRATE OVERRIDE(0~250)
        public short WF; //WF:WIRE FEEDRATE(0~15)
        public short WT; //WT:WIRE TENSION(0~15)
        public short FL; //FL:WATER FLOW RATE(0~7)
        public short FM; //FM:SERVO FEED MODE(0~3)
        public double FMAX; //MAX FEEDRATE
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
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
        long X, Y, Z, U, V;
    }


    public struct WIRE_MMI_INF01
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        char FLAG;
        long DATA_TIME;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 776)]
        char OrtherData;
    }

    
    public struct WIRE_MMI1_INFO
    {//size=784
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public char[] FLAG;
        public long DATE_TIME; //系統時間(unit:秒)
        //public double GAP; //GAP電壓(unit:V)
        //public double FEED; //機台進幾率(unit:mm/min)
        //public double HIPWR; //加工時間比
        //public double WATER_RESIST; //水阻值
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        //public char MFN; //目前加工主程式
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        //public char FN; //目前加工程式
        //public long BN; //目前加工行
        //public long CUT_TM; //目前已加工時間(unit:ms)
        //public long EST_TM; //預計完成時間(unit:ms)
        //public short ALARM_CODE; //顯示錯誤號碼
        //WIRE_PT5DL MP, PGM, G92, LOCAL;
        ////顯示座標(MP:機械座標, PGM:程式座標, G92:G92座標, LOCAL: 區域座標)
        ////詳見資料結構 typedef struct tagPT5DL
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        //public char MESSAGE; //顯示當前訊息字串
        //public short SCODE; //當前加工碼
        //WIRE_SCODE_DATA SDATA; //當前加工資料
        //                       //詳見typedef struct tagSCODE_DATA

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        //public short[] TEMP; //顯示溫度0:下伸臂溫度1:機體溫度2:室內溫度10:冷卻機	環境11:目標溫度//unit:0.01C        
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        //public short[] WATT; //預留


        //public short MEM_ALARM; //程式工作中的警告號碼
        //public short MEM_ERROR; //程式工作中的錯誤號碼
        //public short MEM_STATE; //機台動作(參考MEM_STATE定義)
        //public short MACH_STATE; //機台狀態(參考MACHINE_STATE)
        //public short MACH_ALARM; //機台錯誤訊息(參考ERROR.TXT)
        //public short MAIN_FUNCID; //主視窗位置(參考OP_MODE)
        //public short SUB_FUNC_PAGE; //子視窗位置,Page1:0, Page2:1
        //public short FUNC_KEYID; //子視窗的功能鍵
        //public short MEM_NC; //當前N碼
        //public long IDLE_TM; //機台閒置時間
        //public long ZLOCK_PT; //Z LOCK 座標
        //public double AR_ANGLE; //旋轉角度
        ////WIRE_PT5DL[] WP; //顯示座標
        //                 //(0:G54座標, 1:G55座標, 2:G56座標, 3:G57座標, 4:G58座標, 5:G59座標)
        //                 //詳見資料結構 typedef struct tagPT5DL
        //public double TOTAL_LEN; //程式目前加工長度
        //public double REST_LEN; //程式剩餘長度
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        //public char COMP_STR; //預留
        //public short SysStatus; //系統狀態(參考SysStatus定義)
        //public long SparkIOCount; //加工開始時數(單位ms)
        //public double SquareSpeed; //加工時速度(面積速度mm*mm/min)
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 180)]
        //public char resv;
    }


    // MEM_STATE 數值
    public enum MEM_STATE
    {
        MEM_RESET0 = 0, //程式重置狀況。
        MEM_NONE, //程式重置狀況。
        MEM_START0, //程式等待啟動中。
        MEM_NORMAL, //程式執行中。
        MEM_NORMAL1, //程式執行中。
        MEM_STOP0, //程式暫停中。
        MEM_FINISH, //程式加工完成。
        MEM_BACK,//機台後退移動中。
        MEM_AWT_HANDLE_WB = 10,//整理廢線中。
        MEM_AWT_HOLD_WB, //整理廢線暫停。
        MEM_THRU_WIRE, //穿線中。
        MEM_HOLD_THRU_WIRE,//穿線暫停。
        MEM_CUT_WIRE,//剪線中。
        MEM_HOLD_CUT_WIRE, //剪線暫停。
        MEM_SEARCH_M20 = 30, //搜尋M20程式位置。
        MEM_SEARCH_M21, //搜尋M21程式位置。
        MEM_WAIT_MOVE_IDLE, //程式加工定位中。
        MEM_HOLD_MOVE, //程式加工定位中，暫停移動。
        MEM_RESTART = 40, //再加工搜尋上次中斷位置。
        MEM_RESTART_ABORT, //預留。
        MEM_REHOLE, //預留。
        MEM_REHOLE_ABORT,//預留。
        MEM_NCODE = 50, //搜尋N碼位置中。
        MEM_POWER_DOWN,//系統週邊電源消失
        MEM_WAIT_WATER,//等待工作水漕內的水至高水位時。
        MEM_HOLD_WATER,//在等待工作水漕內的水至高水位時，按STOP鍵暫停。
        MEM_G92_MOVE_OPEN = 80,//在G92命令下，機台移動使極間電壓開路。
        MEM_WAIT_PLC_IN, //等待PLC-IN訊號，PLCType=10-TANK232-I16 PLCType==1:IO232-TB8IO-I8
        MEM_WAIT_THRU_WIRE_WATER_OFF,//在穿線狀態下，等待水漕內的水洩至低水位。
        MEM_WAIT_SLUICE, //M42等待洩水中。
        MEM_RECUT = 90, //模孔再加工搜尋中。
        MEM_RECUT1  //模孔再加工搜尋中。
    }


    // SysStatus 數值
    public enum SysStatus
    {
        SYS_JOG_NORMAL = 1, //當JOG操作超過5sec，系統判定機台移動中
        SYS_MEM_NORMAL, //當系統在AUTO加工畫面，且正在執行程式加工中，系統判定程式加工中
        SYS_EDIT_NORMAL, //當系統在EDIT編輯畫面，並且按鍵超過5sec，系統判定為編輯中
        SYS_MEM_STOP_NONE = 20, //系統程式未知暫停
        SYS_MEM_STOP_ERROR, //系統程式錯誤暫停
        SYS_MEM_STOP_ALARM, //系統程式警告暫停
        SYS_MEM_STOP_IDLE, //系統程式閒置暫停
        SYS_POWER_OFF = 100, //週邊裝置未通電
        SYS_OPER_IDLE, //系統操作閒置
    }

    //// ERROR 數值
    //unsigned short ALARM_CODE; //顯示錯誤號碼
    //unsigned short MEM_ALARM; //程式工作中的警告號碼
    //unsigned short MEM_ERROR; //程式工作中的錯誤號碼
    //unsigned short MACH_ALARM; //機台錯誤訊息(參考ERROR.TXT)


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
