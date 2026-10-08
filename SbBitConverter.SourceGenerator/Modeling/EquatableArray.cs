using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SbBitConverter.SourceGenerator.Modeling;

/// <summary>
///   序列可比较的不可变数组包装。
///   <para>
///     <see cref="ImmutableArray{T}" /> 的默认相等是「同一底层数组」的引用比较，
///     直接放进 record 会让增量缓存永不命中。增量管线的缓存键要求值相等语义，
///     故用本类型包裹集合成员。
///   </para>
/// </summary>
/// <typeparam name="T">元素类型，须实现 <see cref="IEquatable{T}" />。</typeparam>
internal readonly struct EquatableArray<T> : IEquatable<EquatableArray<T>>, IReadOnlyList<T>
  where T : IEquatable<T>
{
  /// <summary>空数组。</summary>
  public static readonly EquatableArray<T> Empty = new(ImmutableArray<T>.Empty);

  private readonly ImmutableArray<T> _array;

  /// <summary>包装既有数组。</summary>
  public EquatableArray(ImmutableArray<T> array) => _array = array;

  private ImmutableArray<T> Values => _array.IsDefault ? ImmutableArray<T>.Empty : _array;

  /// <summary>元素个数。</summary>
  public int Count => Values.Length;

  /// <summary>按下标取元素。</summary>
  public T this[int index] => Values[index];

  /// <inheritdoc />
  public bool Equals(EquatableArray<T> other)
  {
    var left = Values;
    var right = other.Values;
    if (left.Length != right.Length) return false;

    for (var i = 0; i < left.Length; i++)
    {
      if (!left[i].Equals(right[i])) return false;
    }

    return true;
  }

  /// <inheritdoc />
  public override bool Equals(object? obj) => obj is EquatableArray<T> other && Equals(other);

  /// <inheritdoc />
  public override int GetHashCode()
  {
    unchecked
    {
      var hash = 17;
      foreach (var item in Values) hash = (hash * 31) + item.GetHashCode();
      return hash;
    }
  }

  /// <summary>复制为数组。</summary>
  public T[] ToArray()
  {
    var values = Values;
    if (values.IsEmpty) return Array.Empty<T>();

    var result = new T[values.Length];
    for (var i = 0; i < values.Length; i++) result[i] = values[i];
    return result;
  }

  /// <inheritdoc />
  public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Values).GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  /// <inheritdoc />
  public static bool operator ==(EquatableArray<T> left, EquatableArray<T> right) => left.Equals(right);

  /// <inheritdoc />
  public static bool operator !=(EquatableArray<T> left, EquatableArray<T> right) => !left.Equals(right);
}
