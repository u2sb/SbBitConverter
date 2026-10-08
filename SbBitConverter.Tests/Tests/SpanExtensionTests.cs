// Ported from 参考/Best.Extensions.Tests (Best.Extensions)
using Sb.Extensions.System;

namespace SbBitConverter.Tests.Tests;

/// <summary>
///   测试 SpanExtension 的内存工具方法
/// </summary>
public class SpanExtensionTests
{
  // ── CreateSpan ──

  [Fact]
  public void CreateSpan_SingleElement_LengthOne()
  {
    var value = 42;
    var span = SpanExtension.CreateSpan(ref value);
    Assert.Equal(1, span.Length);
    Assert.Equal(42, span[0]);
  }

  [Fact]
  public void CreateSpan_StructArray()
  {
    var values = new[] { 1, 2, 3, 4 };
    var span = SpanExtension.CreateSpan(ref values[0], 4);
    Assert.Equal(4, span.Length);
    Assert.Equal(1, span[0]);
    Assert.Equal(4, span[3]);
  }

  [Fact]
  public void CreateSpan_DefaultLength()
  {
    var value = 99;
    var span = SpanExtension.CreateSpan(ref value);
    Assert.Equal(1, span.Length);
  }

  // ── CreateReadOnlySpan ──

  [Fact]
  public void CreateReadOnlySpan_SingleElement_LengthOne()
  {
    var value = 42;
    var span = SpanExtension.CreateReadOnlySpan(in value);
    Assert.Equal(1, span.Length);
    Assert.Equal(42, span[0]);
  }

  [Fact]
  public void CreateReadOnlySpan_DefaultLength()
  {
    var value = 99;
    var span = SpanExtension.CreateReadOnlySpan(in value);
    Assert.Equal(1, span.Length);
  }

  [Fact]
  public void CreateReadOnlySpan_StructArray()
  {
    var values = new[] { 10, 20, 30 };
    var span = SpanExtension.CreateReadOnlySpan(in values[0], 3);
    Assert.Equal(3, span.Length);
    Assert.Equal(10, span[0]);
    Assert.Equal(30, span[2]);
  }

  // ── Span/ReadOnlySpan 互操作 ──

  [Fact]
  public void CreateSpan_Modify_VisibleAfterModification()
  {
    var value = 0;
    var span = SpanExtension.CreateSpan(ref value);
    span[0] = 123;
    Assert.Equal(123, value);
  }

  [Fact]
  public void CreateSpan_ZeroLength_Works()
  {
    var value = 42;
    var span = SpanExtension.CreateSpan(ref value, 0);
    Assert.Equal(0, span.Length);
  }
}
