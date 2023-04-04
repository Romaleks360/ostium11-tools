using System.Runtime.InteropServices;
using UnityEngine;

namespace Ostium11
{
    public static class Bytes
    {
        static ConvertColor32 color32 = new ConvertColor32();

        public static BytesInt Get(int value) => new BytesInt(value);
        public static BytesLong Get(long value) => new BytesLong(value);

        public static Color32 ToColor32(int value)
        {
            color32.Int = value;
            return color32.Color32;
        }

        public static int ToInt(Color32 value)
        {
            color32.Color32 = value;
            return color32.Int;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct BytesInt
    {
        [FieldOffset(0)] public int Value;
        [FieldOffset(0)] public byte Byte1;
        [FieldOffset(1)] public byte Byte2;
        [FieldOffset(2)] public byte Byte3;
        [FieldOffset(3)] public byte Byte4;

        public BytesInt(int value) : this() => Value = value;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct BytesLong
    {
        [FieldOffset(0)] public long Value;
        [FieldOffset(0)] public byte Byte1;
        [FieldOffset(1)] public byte Byte2;
        [FieldOffset(2)] public byte Byte3;
        [FieldOffset(3)] public byte Byte4;
        [FieldOffset(4)] public byte Byte5;
        [FieldOffset(5)] public byte Byte6;
        [FieldOffset(6)] public byte Byte7;
        [FieldOffset(7)] public byte Byte8;

        public BytesLong(long value) : this() => Value = value;
    }

    [StructLayout(LayoutKind.Explicit)]
    struct ConvertColor32
    {
        [FieldOffset(0)] public int Int;
        [FieldOffset(0)] public Color32 Color32;
    }
}