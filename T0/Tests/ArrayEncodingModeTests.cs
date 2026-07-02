using Sb.Extensions.System;
using T0.Models;

namespace T0.Tests;

/// <summary>
///   测试 SbBitConverterArray 的字节编码模式 (DCBA/ABCD/BADC/CDAB)
/// </summary>
public class ArrayEncodingModeTests
{
    // ── DCBA（小端）──

    [Fact]
    public void DCBA_Int_SameAsLittleEndian()
    {
        var bytes = new byte[] { 0x78, 0x56, 0x34, 0x12, 0xEF, 0xBE, 0xAD, 0xDE, 0, 0, 0, 0, 0, 0, 0, 0 };
        var v = new Int4DCBA(bytes);
        Assert.Equal(0x12345678, v[0]);
        Assert.Equal(unchecked((int)0xDEADBEEF), v[1]);
    }

    [Fact]
    public void DCBA_RoundTrip_PreservesValues()
    {
        var v = new Int4DCBA();
        v[0] = 0x12345678;
        v[1] = -12345678;
        v[2] = 0;
        v[3] = int.MaxValue;

        var bytes = v.ToByteArray();
        var v2 = new Int4DCBA(bytes);

        Assert.Equal(v[0], v2[0]);
        Assert.Equal(v[1], v2[1]);
        Assert.Equal(v[2], v2[2]);
        Assert.Equal(v[3], v2[3]);
    }

    // ── ABCD（大端）──

    [Fact]
    public void ABCD_Int_SameAsBigEndian()
    {
        var bytes = new byte[] { 0x12, 0x34, 0x56, 0x78, 0xDE, 0xAD, 0xBE, 0xEF, 0, 0, 0, 0, 0, 0, 0, 0 };
        var v = new Int4ABCD(bytes);
        Assert.Equal(0x12345678, v[0]);
        Assert.Equal(unchecked((int)0xDEADBEEF), v[1]);
    }

    [Fact]
    public void ABCD_RoundTrip_PreservesValues()
    {
        var v = new Int4ABCD();
        v[0] = 0x12345678;
        v[1] = -1;
        v[2] = 0;
        v[3] = int.MinValue;

        var bytes = v.ToByteArray();
        var v2 = new Int4ABCD(bytes);

        Assert.Equal(v[0], v2[0]);
        Assert.Equal(v[1], v2[1]);
        Assert.Equal(v[2], v2[2]);
        Assert.Equal(v[3], v2[3]);
    }

    // ── BADC（字内翻转，字间不翻转）──

    [Fact]
    public void BADC_Short_RoundTrip()
    {
        var bytes = new byte[12];
        var v = new Short6(bytes, BigAndSmallEndianEncodingMode.BADC);
        var span = v.AsSpan();
        span[0] = 0x0102;
        span[5] = unchecked((short)0xFFFE);

        var roundTrip = v.ToByteArray(BigAndSmallEndianEncodingMode.BADC);
        var v2 = new Short6(roundTrip, BigAndSmallEndianEncodingMode.BADC);

        Assert.Equal(span[0], v2[0]);
        Assert.Equal(span[5], v2[5]);
    }

    [Fact]
    public void BADC_Int_RoundTrip()
    {
        var v = new Int4BADC();
        v[0] = 0x12345678;
        v[1] = -100;
        v[2] = 0;
        v[3] = 987654321;

        var bytes = v.ToByteArray();
        var v2 = new Int4BADC(bytes);

        Assert.Equal(v[0], v2[0]);
        Assert.Equal(v[1], v2[1]);
        Assert.Equal(v[2], v2[2]);
        Assert.Equal(v[3], v2[3]);
    }

    // ── CDAB（字间翻转，字内不翻转）──

    [Fact]
    public void CDAB_Int_RoundTrip()
    {
        var v = new Int4CDAB();
        v[0] = 0x12345678;
        v[1] = -1;
        v[2] = 42;
        v[3] = int.MaxValue;

        var bytes = v.ToByteArray();
        var v2 = new Int4CDAB(bytes);

        Assert.Equal(v[0], v2[0]);
        Assert.Equal(v[1], v2[1]);
        Assert.Equal(v[2], v2[2]);
        Assert.Equal(v[3], v2[3]);
    }

    // ── 模式交叉验证 ──

    [Fact]
    public void AllModes_RoundTrip_Consistent()
    {
        var modes = new[]
        {
            (BigAndSmallEndianEncodingMode)BigAndSmallEndianEncodingMode.DCBA,
            BigAndSmallEndianEncodingMode.ABCD,
            BigAndSmallEndianEncodingMode.BADC,
            BigAndSmallEndianEncodingMode.CDAB
        };

        foreach (var mode in modes)
        {
            var v = new Int4();
            v[0] = 0x12345678;
            v[1] = -999;
            v[2] = 42;
            v[3] = 0x7FFFFFFF;

            var bytes = v.ToByteArray(mode);
            var v2 = new Int4(bytes, mode);

            Assert.Equal(v[0], v2[0]);
            Assert.Equal(v[1], v2[1]);
            Assert.Equal(v[2], v2[2]);
            Assert.Equal(v[3], v2[3]);
        }
    }

    [Fact]
    public void DefaultMode_Equals_DCBA()
    {
        // 默认构造使用 DCBA
        var bytes = new byte[16];
        var vDcba = new Int4DCBA(bytes);
        var vDefault = new Int4(bytes); // 默认 mode=DCBA

        Assert.Equal(vDcba[0], vDefault[0]);
        Assert.Equal(vDcba[1], vDefault[1]);
    }
}
