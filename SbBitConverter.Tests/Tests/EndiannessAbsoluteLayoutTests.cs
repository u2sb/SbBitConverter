// Ported from 参考/Best.Extensions.Tests (Best.Extensions)
using Sb.Extensions.System;
using SbBitConverter.Tests.Models;

namespace SbBitConverter.Tests.Tests;

/// <summary>
///   4 种编码模式的<b>绝对字节布局</b>断言：
///   以「规范大端序 01,02,…,0N」为基准，
///   ABCD = 规范序、DCBA = 全翻转、BADC = 对内翻转、CDAB = 对序翻转。
/// </summary>
public class EndiannessAbsoluteLayoutTests
{
  private const BigAndSmallEndianEncodingMode Dcba = BigAndSmallEndianEncodingMode.DCBA;
  private const BigAndSmallEndianEncodingMode Abcd = BigAndSmallEndianEncodingMode.ABCD;
  private const BigAndSmallEndianEncodingMode Badc = BigAndSmallEndianEncodingMode.BADC;
  private const BigAndSmallEndianEncodingMode Cdab = BigAndSmallEndianEncodingMode.CDAB;

  private static string Hex(byte[] bytes) => string.Join(" ", bytes.Select(static b => b.ToString("X2")));

  /// <summary>断言实际字节与期望<b>逐字节</b>相同，失败时以十六进制输出两方，便于定位。</summary>
  private static void AssertBytes(byte[] expected, byte[] actual, string context)
    => Assert.True(
      expected.SequenceEqual(actual),
      $"{context}{Environment.NewLine}  期望: {Hex(expected)}{Environment.NewLine}  实际: {Hex(actual)}");

  // ═══════════════════════════════════════════════════════════════════
  // 一、基元类型：1/2/4/8/16 字节元素的绝对布局
  // ═══════════════════════════════════════════════════════════════════

  public static TheoryData<BigAndSmallEndianEncodingMode, byte[]> Byte1Cases() => new()
  {
    { Dcba, [0x01] },
    { Abcd, [0x01] },
    { Badc, [0x01] },
    { Cdab, [0x01] }
  };

  public static TheoryData<BigAndSmallEndianEncodingMode, byte[]> Int16Cases() => new()
  {
    { Dcba, [0x02, 0x01] },
    { Abcd, [0x01, 0x02] },
    { Badc, [0x02, 0x01] }, // 2 字节元素：BADC ≡ DCBA（字内翻转即整体翻转）
    { Cdab, [0x01, 0x02] }  // 2 字节元素：CDAB ≡ ABCD（只有 1 个「字」，对序翻转是恒等）
  };

  public static TheoryData<BigAndSmallEndianEncodingMode, byte[]> Int32Cases() => new()
  {
    { Dcba, [0x04, 0x03, 0x02, 0x01] },
    { Abcd, [0x01, 0x02, 0x03, 0x04] },
    { Badc, [0x02, 0x01, 0x04, 0x03] },
    { Cdab, [0x03, 0x04, 0x01, 0x02] }
  };

  public static TheoryData<BigAndSmallEndianEncodingMode, byte[]> Int64Cases() => new()
  {
    { Dcba, [0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01] },
    { Abcd, [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08] },
    { Badc, [0x02, 0x01, 0x04, 0x03, 0x06, 0x05, 0x08, 0x07] },
    { Cdab, [0x07, 0x08, 0x05, 0x06, 0x03, 0x04, 0x01, 0x02] }
  };

  public static TheoryData<BigAndSmallEndianEncodingMode, byte[]> Int128Cases() => new()
  {
    { Dcba, [0x10, 0x0F, 0x0E, 0x0D, 0x0C, 0x0B, 0x0A, 0x09, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01] },
    { Abcd, [0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10] },
    { Badc, [0x02, 0x01, 0x04, 0x03, 0x06, 0x05, 0x08, 0x07, 0x0A, 0x09, 0x0C, 0x0B, 0x0E, 0x0D, 0x10, 0x0F] },
    { Cdab, [0x0F, 0x10, 0x0D, 0x0E, 0x0B, 0x0C, 0x09, 0x0A, 0x07, 0x08, 0x05, 0x06, 0x03, 0x04, 0x01, 0x02] }
  };

  /// <summary>Int128 取值 0x0102030405060708090A0B0C0D0E0F10（规范大端字节为 01…10）。</summary>
  private static Int128 Int128Value => ((Int128)0x0102030405060708L << 64) + (Int128)0x090A0B0C0D0E0F10UL;

  [Theory]
  [MemberData(nameof(Byte1Cases))]
  public void Byte_Absolute_Layout(BigAndSmallEndianEncodingMode mode, byte[] expected)
    => AssertBytes(expected, ((byte)0x01).ToByteArray(mode), $"byte / {mode}");

  [Theory]
  [MemberData(nameof(Int16Cases))]
  public void Int16_Absolute_Layout(BigAndSmallEndianEncodingMode mode, byte[] expected)
    => AssertBytes(expected, ((short)0x0102).ToByteArray(mode), $"short / {mode}");

  [Theory]
  [MemberData(nameof(Int32Cases))]
  public void Int32_Absolute_Layout(BigAndSmallEndianEncodingMode mode, byte[] expected)
    => AssertBytes(expected, 0x01020304.ToByteArray(mode), $"int / {mode}");

  [Theory]
  [MemberData(nameof(Int64Cases))]
  public void Int64_Absolute_Layout(BigAndSmallEndianEncodingMode mode, byte[] expected)
    => AssertBytes(expected, 0x0102030405060708L.ToByteArray(mode), $"long / {mode}");

  [Theory]
  [MemberData(nameof(Int128Cases))]
  public void Int128_Absolute_Layout(BigAndSmallEndianEncodingMode mode, byte[] expected)
    => AssertBytes(expected, Int128Value.ToByteArray(mode), $"Int128 / {mode}");

  /// <summary>bool API 必须与 mode API 一致：false ≡ DCBA，true ≡ ABCD。</summary>
  [Fact]
  public void Bool_Api_Matches_Dcba_And_Abcd()
  {
    const int value = 0x01020304;
    AssertBytes([0x04, 0x03, 0x02, 0x01], value.ToByteArray(false), "ToByteArray(false) 应等于 DCBA");
    AssertBytes([0x01, 0x02, 0x03, 0x04], value.ToByteArray(true), "ToByteArray(true) 应等于 ABCD");
  }

  // ═══════════════════════════════════════════════════════════════════
  // 二、生成代码路径：定长数组容器的绝对布局
  // ═══════════════════════════════════════════════════════════════════

  [Fact]
  public void Generated_Int4DCBA_Writes_LittleEndian_First_Element()
  {
    var value = new Int4DCBA();
    value[0] = 0x01020304;

    AssertBytes([0x04, 0x03, 0x02, 0x01], value.ToByteArray()[..4], "Int4DCBA / 默认(0=DCBA)");
  }

  [Fact]
  public void Generated_Int4ABCD_Writes_BigEndian_First_Element()
  {
    var value = new Int4ABCD();
    value[0] = 0x01020304;

    AssertBytes([0x01, 0x02, 0x03, 0x04], value.ToByteArray()[..4], "Int4ABCD / 默认(1=ABCD)");
  }

  [Fact]
  public void Generated_Int4BADC_Writes_Pair_Swapped_First_Element()
  {
    var value = new Int4BADC();
    value[0] = 0x01020304;

    AssertBytes([0x02, 0x01, 0x04, 0x03], value.ToByteArray()[..4], "Int4BADC / 默认(2=BADC)");
  }

  [Fact]
  public void Generated_Int4CDAB_Writes_Pair_Reversed_First_Element()
  {
    var value = new Int4CDAB();
    value[0] = 0x01020304;

    AssertBytes([0x03, 0x04, 0x01, 0x02], value.ToByteArray()[..4], "Int4CDAB / 默认(3=CDAB)");
  }

  /// <summary>
  ///   BADC 与 CDAB 在 4 字节元素上必须不同 —— 若有人把两个分支写反，它会失败。
  /// </summary>
  [Fact]
  public void Generated_Badc_And_Cdab_Differ_For_4Byte_Elements()
  {
    var badc = new Int4BADC();
    var cdab = new Int4CDAB();
    badc[0] = 0x01020304;
    cdab[0] = 0x01020304;

    Assert.NotEqual(badc.ToByteArray()[..4], cdab.ToByteArray()[..4]);
  }

  /// <summary>
  ///   2 字节元素（Short6，模式 BADC）下 BADC ≡ DCBA：只有两种可能的字节序，4 个名字必然塌缩。
  /// </summary>
  [Fact]
  public void Generated_Short6_Badc_Collapses_To_LittleEndian()
  {
    var value = new Short6();
    value[0] = 0x0102;

    AssertBytes([0x02, 0x01], value.ToByteArray()[..2], "Short6 / BADC（2 字节元素）");
  }

  // ═══════════════════════════════════════════════════════════════════
  // 三、往返（交叉确认，避免绝对布局与往返同时失效）
  // ═══════════════════════════════════════════════════════════════════

  [Theory]
  [InlineData(Dcba)]
  [InlineData(Abcd)]
  [InlineData(Badc)]
  [InlineData(Cdab)]
  public void Int32_RoundTrip_AllModes(BigAndSmallEndianEncodingMode mode)
  {
    const int value = 0x01020304;
    var restored = value.ToByteArray(mode).AsSpan().ToT<int>(mode);

    Assert.Equal(value, restored);
  }
}
