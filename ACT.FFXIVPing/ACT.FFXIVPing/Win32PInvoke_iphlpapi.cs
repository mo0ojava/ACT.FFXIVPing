﻿using System;
using System.Net;
using System.Runtime.InteropServices;

namespace ACT.FFXIVPing
{
    public static class Win32PInvoke_iphlpapi
    {
        #region Const

        public const int AF_INET = 2;    // IP_v4 = System.Net.Sockets.AddressFamily.InterNetwork
        public const int AF_INET6 = 23;  // IP_v6 = System.Net.Sockets.AddressFamily.InterNetworkV6

        #endregion

        #region Enum

        public enum TCP_TABLE_CLASS
        {
            TCP_TABLE_BASIC_LISTENER,
            TCP_TABLE_BASIC_CONNECTIONS,
            TCP_TABLE_BASIC_ALL,
            TCP_TABLE_OWNER_PID_LISTENER,
            TCP_TABLE_OWNER_PID_CONNECTIONS,
            TCP_TABLE_OWNER_PID_ALL,
            TCP_TABLE_OWNER_MODULE_LISTENER,
            TCP_TABLE_OWNER_MODULE_CONNECTIONS,
            TCP_TABLE_OWNER_MODULE_ALL
        }

        public enum MIB_TCP_STATE
        {
            MIB_TCP_STATE_CLOSED = 1,
            MIB_TCP_STATE_LISTEN = 2,
            MIB_TCP_STATE_SYN_SENT = 3,
            MIB_TCP_STATE_SYN_RCVD = 4,
            MIB_TCP_STATE_ESTAB = 5,
            MIB_TCP_STATE_FIN_WAIT1 = 6,
            MIB_TCP_STATE_FIN_WAIT2 = 7,
            MIB_TCP_STATE_CLOSE_WAIT = 8,
            MIB_TCP_STATE_CLOSING = 9,
            MIB_TCP_STATE_LAST_ACK = 10,
            MIB_TCP_STATE_TIME_WAIT = 11,
            MIB_TCP_STATE_DELETE_TCB = 12
        }

        public enum TCP_ESTATS_TYPE
        {
            TcpConnectionEstatsSynOpts,
            TcpConnectionEstatsData,
            TcpConnectionEstatsSndCong,
            TcpConnectionEstatsPath,
            TcpConnectionEstatsSendBuff,
            TcpConnectionEstatsRec,
            TcpConnectionEstatsObsRec,
            TcpConnectionEstatsBandwidth,
            TcpConnectionEstatsFineRtt,
            TcpConnectionEstatsMaximum,
        }

        #endregion

        #region Struct

        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPROW_OWNER_PID : IEquatable<MIB_TCPROW_OWNER_PID>
        {
            public uint state;
            public uint localAddr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] localPort;
            public uint remoteAddr;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] remotePort;
            public uint owningPid;

            public uint ProcessId
            {
                get { return owningPid; }
            }

            public IPAddress LocalAddress
            {
                get { return new IPAddress(localAddr); }
            }

            public ushort LocalPort
            {
                get { return BitConverter.ToUInt16(new byte[2] {localPort[1], localPort[0]}, 0); }
            }

            public IPAddress RemoteAddress
            {
                get { return new IPAddress(remoteAddr); }
            }

            public ushort RemotePort
            {
                get { return BitConverter.ToUInt16(new byte[2] {remotePort[1], remotePort[0]}, 0); }
            }

            public MIB_TCP_STATE State
            {
                get { return (MIB_TCP_STATE) state; }
            }

            public bool Equals(MIB_TCPROW_OWNER_PID other)
            {
                return localAddr == other.localAddr && Equals(localPort, other.localPort) &&
                       remoteAddr == other.remoteAddr && Equals(remotePort, other.remotePort) &&
                       owningPid == other.owningPid;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is MIB_TCPROW_OWNER_PID && Equals((MIB_TCPROW_OWNER_PID) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (int) localAddr;
                    hashCode = (hashCode * 397) ^ (localPort != null ? localPort.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (int) remoteAddr;
                    hashCode = (hashCode * 397) ^ (remotePort != null ? remotePort.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (int) owningPid;
                    return hashCode;
                }
            }

            public static bool operator ==(MIB_TCPROW_OWNER_PID left, MIB_TCPROW_OWNER_PID right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(MIB_TCPROW_OWNER_PID left, MIB_TCPROW_OWNER_PID right)
            {
                return !left.Equals(right);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPTABLE_OWNER_PID
        {
            public uint dwNumEntries;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 1)]
            public MIB_TCPROW_OWNER_PID[] table;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TCP_ESTATS_DATA_ROD_v0
        {
            public ulong DataBytesOut;
            public ulong DataSegsOut;
            public ulong DataBytesIn;
            public ulong DataSegsIn;
            public ulong SegsOut;
            public ulong SegsIn;
            public uint SoftErrors;
            public uint SoftErrorReason;
            public uint SndUna;
            public uint SndNxt;
            public uint SndMax;
            public ulong ThruBytesAcked;
            public uint RcvNxt;
            public ulong ThruBytesReceived;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TCP_ESTATS_PATH_ROD_v0
        {
            public uint FastRetran;
            public uint Timeouts;
            public uint SubsequentTimeouts;
            public uint CurTimeoutCount;
            public uint AbruptTimeouts;
            public uint PktsRetrans;
            public uint BytesRetrans;
            public uint DupAcksIn;
            public uint SacksRcvd;
            public uint SackBlocksRcvd;
            public uint CongSignals;
            public uint PreCongSumCwnd;
            public uint PreCongSumRtt;
            public uint PostCongSumRtt;
            public uint PostCongCountRtt;
            public uint EcnSignals;
            public uint EceRcvd;
            public uint SendStall;
            public uint QuenchRcvd;
            public uint RetranThresh;
            public uint SndDupAckEpisodes;
            public uint SumBytesReordered;
            public uint NonRecovDa;
            public uint NonRecovDaEpisodes;
            public uint AckAfterFr;
            public uint DsackDups;
            public uint SampleRtt;
            public uint SmoothedRtt;
            public uint RttVar;
            public uint MaxRtt;
            public uint MinRtt;
            public uint SumRtt;
            public uint CountRtt;
            public uint CurRto;
            public uint MaxRto;
            public uint MinRto;
            public uint CurMss;
            public uint MaxMss;
            public uint MinMss;
            public uint SpuriousRtoDetections;
        }

        #endregion

        #region Import

        [DllImport("iphlpapi.dll", SetLastError = true)]
        static extern uint GetExtendedTcpTable(IntPtr pTcpTable, ref int dwOutBufLen, bool sort, int ipVersion,
            TCP_TABLE_CLASS tblClass, uint reserved = 0);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        static extern uint GetPerTcpConnectionEStats(IntPtr row, TCP_ESTATS_TYPE statsType,
            IntPtr w, uint wVersion, uint wSize,
            IntPtr os, uint osVersion, uint osSize,
            IntPtr od, uint odVersion, uint odSize);

        #endregion

        public static MIB_TCPROW_OWNER_PID[] GetAllTcpConnections()
        {
            MIB_TCPROW_OWNER_PID[] tTable;
            var buffSize = 0;

            // how much memory do we need?
            var ret = GetExtendedTcpTable(IntPtr.Zero, ref buffSize, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL);
            var buffTable = Marshal.AllocHGlobal(buffSize);

            try
            {
                ret = GetExtendedTcpTable(buffTable, ref buffSize, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL);
                if (ret != 0)
                {
                    return null;
                }

                // get the number of entries in the table
                var tab = (MIB_TCPTABLE_OWNER_PID)Marshal.PtrToStructure(buffTable, typeof(MIB_TCPTABLE_OWNER_PID));
                var rowPtr = (IntPtr)((long)buffTable + Marshal.SizeOf(tab.dwNumEntries));
                tTable = new MIB_TCPROW_OWNER_PID[tab.dwNumEntries];

                for (var i = 0; i < tab.dwNumEntries; i++)
                {
                    var tcpRow = (MIB_TCPROW_OWNER_PID)Marshal.PtrToStructure(rowPtr, typeof(MIB_TCPROW_OWNER_PID));
                    tTable[i] = tcpRow;
                    rowPtr = (IntPtr)((long)rowPtr + Marshal.SizeOf(tcpRow));   // next entry
                }
            }
            finally
            {
                // Free the Memory
                Marshal.FreeHGlobal(buffTable);
            }
            return tTable;
        }

        public static bool GetPerTcpConnectionEStats<TOut>(MIB_TCPROW_OWNER_PID row, TCP_ESTATS_TYPE statsType, ref TOut stats)
        {
            var buffRow = IntPtr.Zero;
            var buffOD = IntPtr.Zero;
            try
            {
                buffRow = Marshal.AllocHGlobal(Marshal.SizeOf(row));
                Marshal.StructureToPtr(row, buffRow, false);
                buffOD = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TOut)));

                var result = GetPerTcpConnectionEStats(buffRow, statsType,
                    IntPtr.Zero, 0, 0,
                    IntPtr.Zero, 0, 0,
                    buffOD, 0, (uint) Marshal.SizeOf(typeof(TOut)));

                if (result != 0)
                {
                    return false;
                }

                stats = (TOut) Marshal.PtrToStructure(buffOD, typeof(TOut));
            }
            finally
            {
                // Free the Memory
                if (buffRow != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffRow);
                }
                if (buffOD != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(buffOD);
                }
            }
            return true;
        }
    }
}
