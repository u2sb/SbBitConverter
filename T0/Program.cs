using System;
using Sb.Extensions.System;

namespace T0;

internal class Program
{
  private static void Main(string[] args)
  {
    TestBool8();
    TestInt4();
    TestShort6();
    TestDouble2();
    TestByte16();
    TestMixedStruct();
    TestUShort4();
    TestFloat3();

    Console.WriteLine("All tests passed!");
  }

  private static void TestBool8()
  {
    var bytes = new byte[] { 0b10101010, 0b01010101, 0b11110000, 0b00001111, 1, 0, 1, 1 };
    var v = new Bool8(bytes);
    Console.WriteLine($"[Bool8] [0]={v[0]} [1]={v[1]} [7]={v[7]} Length={v.Length}");

    var roundTrip = v.ToByteArray();
    for (var i = 0; i < bytes.Length; i++)
      if (bytes[i] != roundTrip[i])
        throw new Exception($"Bool8 round-trip failed at byte {i}");
  }

  private static void TestInt4()
  {
    var v = new Int4();
    var span = v.AsSpan();
    span[0] = 42;
    span[1] = -1;
    span[2] = int.MaxValue;
    span[3] = int.MinValue;

    var bytes = v.ToByteArray();
    var v2 = new Int4(bytes);
    var span2 = v2.AsSpan();
    for (var i = 0; i < 4; i++)
      if (span[i] != span2[i])
        throw new Exception($"Int4 round-trip failed at [{i}]: {span[i]} != {span2[i]}");
    Console.WriteLine($"[Int4] {span[0]} {span[1]} {span[2]} {span[3]}");
  }

  private static void TestShort6()
  {
    var bytes = new byte[12];
    var v = new Short6(bytes, BigAndSmallEndianEncodingMode.BADC);
    var span = v.AsSpan();
    span[0] = 100;
    span[5] = -200;

    var roundTrip = v.ToByteArray(BigAndSmallEndianEncodingMode.BADC);
    var v2 = new Short6(roundTrip, BigAndSmallEndianEncodingMode.BADC);
    Console.WriteLine($"[Short6] [0]={v2[0]} [5]={v2[5]} Length={v2.Length}");
  }

  private static void TestDouble2()
  {
    var bytes = new byte[16];
    var v = new Double2(bytes);
    var span = v.AsSpan();
    span[0] = 3.14;
    span[1] = -2.718;

    var roundTrip = v.ToByteArray();
    var v2 = new Double2(roundTrip);
    Console.WriteLine($"[Double2] [0]={v2[0]} [1]={v2[1]}");
  }

  private static void TestByte16()
  {
    var bytes = new byte[16];
    for (var i = 0; i < 16; i++) bytes[i] = (byte)(i * 17);
    var v = new Byte16(bytes);
    Console.WriteLine($"[Byte16] [0]={v[0]} [15]={v[15]} Count={v.Count}");

    var roundTrip = v.ToByteArray();
    for (var i = 0; i < 16; i++)
      if (bytes[i] != roundTrip[i])
        throw new Exception($"Byte16 round-trip failed at [{i}]");
  }

  private static void TestMixedStruct()
  {
    var v = new MixedStruct
    {
      B = 0xAB,
      Flag = true,
      S = -1000,
      I = 12345678
    };
    var bytes = v.ToByteArray();
    var v2 = new MixedStruct(bytes);
    if (v2.B != 0xAB || !v2.Flag || v2.S != -1000 || v2.I != 12345678)
      throw new Exception("MixedStruct round-trip failed");
    Console.WriteLine($"[MixedStruct] B=0x{v2.B:X2} Flag={v2.Flag} S={v2.S} I={v2.I}");
  }

  private static void TestUShort4()
  {
    var v = new UShort4();
    var roSpan = v.AsSpan(); // readonly struct 返回 ReadOnlySpan
    Console.WriteLine($"[UShort4] Length={v.Length} Count={v.Count} AsSpan={roSpan.Length}");
  }

  private static void TestFloat3()
  {
    var bytes = new byte[12];
    var v = new Float3(bytes);
    var span = v.AsSpan();
    span[0] = 1.0f;
    span[1] = 2.0f;
    span[2] = 3.0f;

    var roundTrip = v.ToByteArray();
    var v2 = new Float3(roundTrip);
    Console.WriteLine($"[Float3] [0]={v2[0]} [1]={v2[1]} [2]={v2[2]}");
  }
}
