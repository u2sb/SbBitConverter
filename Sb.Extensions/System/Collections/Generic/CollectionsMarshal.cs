using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#if !NET7_0_OR_GREATER

#pragma warning disable CS1591 // 本文件为 polyfill，省略部分公共成员的 XML 注释

namespace Sb.Extensions.System.Collections.Generic;

/// <summary>
///   提供 <see cref="List{T}" /> 的 <c>AsSpan</c> polyfill（等价于 .NET 5+ 的
///   <c>System.Runtime.InteropServices.CollectionsMarshal.AsSpan</c>）。
/// </summary>
public static class CollectionsMarshal
{
  /// <summary>
  ///   返回覆盖 <see cref="List{T}" /> 全部有效元素的定长 <see cref="Span{T}" />。
  ///   <para>
  ///     ⚠️ 仅依赖 <see cref="List{T}" /> 的**首个引用字段**（内部数组）做叠加视图，
  ///     元素个数取自公开的 <see cref="List{T}.Count" /> —— 不对 int 字段做布局假设
  ///     （.NET Framework CLR 的 auto-layout 会重排/填充 int 字段，曾实测导致静默错位）。
  ///   </para>
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="list"></param>
  /// <returns></returns>
  extension<T>(List<T>? list)
  {
    public Span<T> AsSpan()
    {
      if (list is null) return default;

      ref var view = ref Unsafe.As<List<T>, ItemsView<T>>(ref list!);
      return view.Items.AsSpan(0, list.Count);
    }
  }

  /// <summary>
  ///   仅承载首个引用字段（内部数组）的叠加视图。
  ///   内部类型：勿在库外依赖。
  /// </summary>
  /// <typeparam name="T"></typeparam>
  internal sealed class ItemsView<T>
  {
    /// <summary>与 <see cref="List{T}" />._items 对应的内部数组。</summary>
    public T[] Items = default!;
  }
}

#endif
