using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释

namespace Sb.Extensions.System.Collections.Generic;

public static class CollectionExtensions
{
  private const int ArrayMaxLength = 0X7FFFFFC7;

  extension<TKey, TValue>(KeyValuePair<TKey, TValue> kvp)
  {
    public void Deconstruct(out TKey key, out TValue value)
    {
      key = kvp.Key;
      value = kvp.Value;
    }
  }

  extension<TKey, TValue>(SortedDictionary<TKey, TValue> dict) where TKey : notnull
  {
    public bool Remove(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
      if (dict.TryGetValue(key, out value)) return dict.Remove(key);
      return false;
    }
  }

  extension<TKey, TValue>(Dictionary<TKey, TValue> dict) where TKey : notnull
  {
    public bool Remove(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
      if (dict.TryGetValue(key, out value)) return dict.Remove(key);
      return false;
    }
  }

#if !NET6_0_OR_GREATER
  extension<T>(IEnumerable<T> source)
  {
    public bool TryGetNonEnumeratedCount(out int count)
    {
      if (source is ICollection<T> collection)
      {
        count = collection.Count;
        return true;
      }

      if (source is IReadOnlyCollection<T> rCollection)
      {
        count = rCollection.Count;
        return true;
      }

      count = 0;
      return false;
    }
  }

#endif

#if !NET8_0_OR_GREATER

  // List<T>.AddRange(ReadOnlySpan) / InsertRange(int, ReadOnlySpan) polyfill。
  // ⚠️ 实现策略（经 net48 实测）：
  //   - **不做 int 字段（_size）布局叠加** —— .NET Framework CLR 的 auto-layout 会重排
  //     polyfill 类型的 int 字段，导致读写静默错位（详见 CollectionsMarshal.cs 注释）；
  //   - 容量不足时仅对**首个引用字段（内部数组）**做叠加扩容（该偏移在 net48 实测可靠），
  //     再通过公开的 Add/Insert 逐项写入，保证 _size/_version 由运行时自身维护。
  //   - 逐项 Add 相比 BCL IEnumerable 路径省去了枚举器分配与接口虚调用。

  extension<T>(List<T> list)
  {
    public void AddRange(ReadOnlySpan<T> source)
    {
      if (source.IsEmpty) return;

      if (list.Capacity - list.Count < source.Length)
      {
        Grow(list, checked(list.Count + source.Length));
      }

      foreach (var item in source)
      {
        list.Add(item);
      }
    }

    public void InsertRange(int index, ReadOnlySpan<T> source)
    {
      if (source.IsEmpty) return;

      if (list.Capacity - list.Count < source.Length)
      {
        Grow(list, checked(list.Count + source.Length));
      }

      var i = index;
      foreach (var item in source)
      {
        list.Insert(i++, item);
      }
    }
  }

  /// <summary>
  ///   仅替换 List 的内部数组（首个引用字段）以预留容量，元素数由后续 Add/Insert 维护。
  /// </summary>
  private static void Grow<T>(List<T> list, int requiredCapacity)
  {
    ref var view = ref Unsafe.As<List<T>, CollectionsMarshal.ItemsView<T>>(ref list);
    var oldItems = view.Items;
    var newItems = new T[GetNewCapacity(oldItems.Length == 0 ? 4 : oldItems.Length * 2, requiredCapacity)];
    Array.Copy(oldItems, newItems, list.Count);
    view.Items = newItems;
  }

  private static int GetNewCapacity(int currentCapacity, int requiredCapacity)
  {
    var newCapacity = currentCapacity * 2;
    if ((uint)newCapacity > ArrayMaxLength) newCapacity = ArrayMaxLength;
    if (newCapacity < requiredCapacity) newCapacity = requiredCapacity;
    return newCapacity;
  }

#endif

  extension<T>(ObservableCollection<T> collection)
  {
    public void AddRange(IEnumerable<T> items)
    {
      foreach (var item in items)
      {
        collection.Add(item);
      }
    }

    public void AddRange(ReadOnlySpan<T> items)
    {
      foreach (var item in items)
      {
        collection.Add(item);
      }
    }

    public void SetRange(IEnumerable<T> items)
    {
      var i = 0;
      foreach (var item in items)
      {
        if (collection.Count > i)
        {
          collection[i] = item;
        }
        else
        {
          collection.Add(item);
        }

        i++;
      }

      while (collection.Count > i)
      {
        collection.RemoveAt(collection.Count - 1);
      }
    }
  }
}

#if !NET5_0_OR_GREATER
public interface IReadOnlySet<out T> : IEnumerable<T>, IReadOnlyCollection<T>
{
}
#endif
