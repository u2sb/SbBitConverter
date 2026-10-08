using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Bench.Models;
using Sb.Extensions.System;

if (args.Length > 0)
{
  BenchmarkRunner.Run(typeof(Program).Assembly);
  return;
}

// 无参数时打印提示
Console.WriteLine("用法: dotnet run -c Release --project Bench -- --filter *");
Console.WriteLine("基准类: GeneratedArrayBenchmarks / EndiannessBenchmarks");

/// <summary>基准入口。</summary>
public static partial class Program
{
}

/// <summary>
///   P3.2 特化效果基准：1 字节元素容器的序列化路径（特化前为逐元素 WriteTo&lt;T&gt; 调用）。
/// </summary>
[MemoryDiagnoser]
public class GeneratedArrayBenchmarks
{
  private Byte64 _byte64;
  private Int4 _int4;
  private byte[] _buffer = new byte[64];

  [GlobalSetup]
  public void Setup()
  {
    _byte64 = new Byte64(stackalloc byte[64]);
    _int4 = new Int4([1, 2, 3, 4]);
  }

  [Benchmark(Baseline = true)]
  public void Byte64_WriteTo()
  {
    for (var i = 0; i < 1000; i++) _byte64.WriteTo(_buffer);
  }

  [Benchmark]
  public void Byte64_ToByteArray()
  {
    for (var i = 0; i < 100; i++) _ = _byte64.ToByteArray();
  }

  [Benchmark]
  public void Int4_WriteTo()
  {
    for (var i = 0; i < 1000; i++) _int4.WriteTo(_buffer);
  }
}

/// <summary>
///   字节序转换运行时开销：生成容器的 ToByteArray vs 手写 BitConverter 路径。
/// </summary>
[MemoryDiagnoser]
public class EndiannessBenchmarks
{
  private Int4DCBA _dCBA;
  private Int4ABCD _aBCD;

  [GlobalSetup]
  public void Setup()
  {
    _dCBA = new Int4DCBA([1, 2, 3, 4]);
    _aBCD = new Int4ABCD([1, 2, 3, 4]);
  }

  [Benchmark]
  public void Generated_DCBA_ToByteArray()
  {
    for (var i = 0; i < 100; i++) _ = _dCBA.ToByteArray();
  }

  [Benchmark]
  public void Generated_ABCD_ToByteArray()
  {
    for (var i = 0; i < 100; i++) _ = _aBCD.ToByteArray();
  }

  [Benchmark]
  public void Manual_BitConverter_LittleEndian()
  {
    Span<byte> span = stackalloc byte[16];
    for (var i = 0; i < 100; i++)
    {
      BitConverter.TryWriteBytes(span, 1);
      BitConverter.TryWriteBytes(span.Slice(4), 2);
      BitConverter.TryWriteBytes(span.Slice(8), 3);
      BitConverter.TryWriteBytes(span.Slice(12), 4);
    }
  }
}
