using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace SbBitConverter.SourceGenerator.Emitting;

/// <summary>
///   线程安全的 StringBuilder 池。生成器每次 Emit 都构造大字符串，
///   复用底层缓冲可显著降低构建期分配与 GC 压力。
/// </summary>
internal static class StringBuilderPool
{
  [ThreadStatic]
  private static StringBuilder? Cached;

  /// <summary>取出一个已清空的 StringBuilder，并保证容量不低于 <paramref name="minCapacity" />。</summary>
  public static StringBuilder Rent(int minCapacity)
  {
    var sb = Cached;
    Cached = null;

    if (sb is null)
    {
      sb = new StringBuilder(minCapacity);
    }
    else
    {
      sb.Clear();
      if (sb.Capacity < minCapacity) sb.EnsureCapacity(minCapacity);
    }

    return sb;
  }

  /// <summary>导出字符串并归还缓冲。</summary>
  public static string Return(StringBuilder sb)
  {
    var result = sb.ToString();

    // 只缓存一个；容量过大时丢弃，避免长期占用
    if (sb.Capacity <= 512 * 1024)
    {
      Cached ??= sb;
    }

    return result;
  }
}
